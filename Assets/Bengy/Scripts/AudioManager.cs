using UnityEngine;

public enum AudioType
{
    walk,
    door,
    elevator,
    labyrinth,
    goodChoice,
    wrongChoice,
    simon,
    dino,
    slicedPuzzle,
    button,
    atmosphere,
    voiceInTheHead
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [System.Serializable]
    public struct AudioData
    {
        public AudioType type;
        public AudioSource source;
    }

    public AudioData[] audioData;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioType type)
    {
        AudioData data = GetAudioData(type);
        data.source.Play();
    }
    
    public void StopSound(AudioType type)
    {
        AudioData data = GetAudioData(type);
        data.source.Stop();
    }

    public AudioData GetAudioData(AudioType type)
    {
        for (int i = 0; i < audioData.Length; i++)
        {
            if (audioData[i].type == type)
            {
                return audioData[i];
            }
        }
        Debug.LogError("AudioManager: No clip found for type " + type);
        return new AudioData();
    }
}
    