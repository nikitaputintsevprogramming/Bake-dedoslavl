using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class AudioLoader : MonoBehaviour
{
    public string audioDirectory = "AudioFiles"; // Папка AudioFiles внутри StreamingAssets
    private AudioSource audioSource;
    [SerializeField] private Text textPrefab; // Префаб текстового элемента
    [SerializeField] private Transform contentTransform; // Контейнер для текста внутри ScrollView

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        string path = Path.Combine(Application.streamingAssetsPath, audioDirectory);

        // Убедимся, что у Content есть компонент VerticalLayoutGroup
        EnsureVerticalLayoutGroup(contentTransform);

        if (Directory.Exists(path))
        {
            string[] audioFiles = Directory.GetFiles(path, "*.mp3");
            if (audioFiles.Length > 0)
            {
                for (int i = 0; i < audioFiles.Length; i++)
                {
                    CreateTextElement(Path.GetFileNameWithoutExtension(audioFiles[i]));
                }

                // Загрузка и проигрывание первого MP3 файла
                StartCoroutine(LoadAudio(audioFiles[0]));
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
                    audioSource.clip = clip;
                    audioSource.Play();
                    Debug.Log("Playing clip: " + clip.name); // Выводим имя клипа в лог
                }
            }
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
        layoutGroup.spacing = 10; // Настройте это значение по вашему усмотрению
    }
}
