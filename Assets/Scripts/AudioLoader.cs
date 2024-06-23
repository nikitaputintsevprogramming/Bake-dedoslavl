using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class AudioLoader : MonoBehaviour
{
    public string audioDirectory = "AudioFiles"; // Папка AudioFiles внутри StreamingAssets
    private AudioSource audioSource;
    [SerializeField] private Text TextAudioData;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        string path = Path.Combine(Application.streamingAssetsPath, audioDirectory);

        if (Directory.Exists(path))
        {
            string[] audioFiles = Directory.GetFiles(path, "*.mp3");
            if (audioFiles.Length > 0)
            {
                StartCoroutine(LoadAudio(audioFiles[0])); // Загрузка первого MP3 файла
            }
        }
        else
        {
            Debug.LogError("Directory not found: " + path);
        }
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
                audioSource.clip = clip;
                audioSource.Play();
                //Debug.Log(clip.loadState);
                TextAudioData.text = clip.name;
            }
        }
    }
}

