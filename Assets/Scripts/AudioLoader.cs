using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Video;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Bake
{
    public class MediaLoader : MonoBehaviour
    {
        public string audioDirectory = "AudioFiles"; // Папка AudioFiles внутри StreamingAssets
        public string videoDirectory = "VideoFiles"; // Папка VideoFiles внутри StreamingAssets
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private Text textPrefab; // Префаб текстового элемента
        [SerializeField] private Transform contentTransform; // Контейнер для текста внутри ScrollView

        private AudioManager _audioManager;
        private List<IEnumerator> audioLoaders = new List<IEnumerator>();

        void Awake()
        {
            //audioSource = gameObject.AddComponent<AudioSource>();
            //videoPlayer = gameObject.AddComponent<VideoPlayer>();

            EnsureVerticalLayoutGroup(contentTransform);

            LoadMediaFiles(audioDirectory, "*.mp3", LoadAudio);
            LoadMediaFiles(videoDirectory, "*.mp4", LoadVideo);

            // Запуск корутины для ожидания загрузки всех аудиофайлов
            StartCoroutine(WaitForAllAudioLoaders());
        }

        void LoadMediaFiles(string directory, string filePattern, System.Func<string, IEnumerator> loadFunction)
        {
            string path = Path.Combine(Application.streamingAssetsPath, directory);
            _audioManager = FindObjectOfType<AudioManager>();

            if (Directory.Exists(path))
            {
                string[] mediaFiles = Directory.GetFiles(path, filePattern);
                if (mediaFiles.Length > 0)
                {
                    foreach (string file in mediaFiles)
                    {
                        CreateTextElement(Path.GetFileNameWithoutExtension(file));
                        IEnumerator loader = loadFunction(file);
                        audioLoaders.Add(loader);
                        StartCoroutine(loader);
                    }
                }
            }
            else
            {
                Debug.LogError("Directory not found: " + path);
            }
        }

        void CreateTextElement(string fileName)
        {
            Text textElement = Instantiate(textPrefab, contentTransform);
            textElement.text = fileName;
        }

        IEnumerator LoadAudio(string path)
        {
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                    if (clip != null)
                    {
                        clip.name = Path.GetFileName(path); // Устанавливаем имя клипа как имя файла
                        Debug.Log("Loaded audio clip: " + clip.name); // Выводим имя клипа в лог
                        _audioManager.mysteries.Add(clip);
                    }
                }
            }
        }

        IEnumerator LoadVideo(string path)
        {
            using (UnityWebRequest www = UnityWebRequest.Get(path))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(www.error);
                }
                else
                {
                    videoPlayer.url = path;
                    videoPlayer.Play();
                    Debug.Log("Playing video: " + Path.GetFileName(path)); // Выводим имя видеофайла в лог
                }
            }
        }

        IEnumerator WaitForAllAudioLoaders()
        {
            foreach (var loader in audioLoaders)
            {
                yield return loader;
            }

            // Преобразуем имена файлов в числовые значения, сортируем их и преобразуем обратно в имена файлов
            var sortedClips = _audioManager.mysteries
                .OrderBy(clip => int.Parse(Path.GetFileNameWithoutExtension(clip.name)))
                .ToList();

            // Обновляем оригинальный список
            _audioManager.mysteries = sortedClips;

            // Выводим отсортированный список для проверки
            foreach (var clip in _audioManager.mysteries)
            {
                Debug.Log(clip.name);
            }
        }

        void EnsureVerticalLayoutGroup(Transform content)
        {
            VerticalLayoutGroup layoutGroup = content.GetComponent<VerticalLayoutGroup>();
            if (layoutGroup == null)
            {
                layoutGroup = content.gameObject.AddComponent<VerticalLayoutGroup>();
            }

            layoutGroup.childForceExpandHeight = false;
            layoutGroup.childForceExpandWidth = true;
            layoutGroup.childControlHeight = true;
            layoutGroup.childControlWidth = true;
            layoutGroup.spacing = 10; 
        }
    }
}
