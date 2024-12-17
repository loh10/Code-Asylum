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

public enum AudioSourceType
{
    game,
    player
}

public class AudioManager : MonoBehaviour
{
    static public AudioManager Instance;

    public float volume = 1.0f;

    public AudioSource gameSource;
    public AudioSource playerSource;

    [System.Serializable]
    public struct AudioData
    {
        public AudioClip clip;
        public AudioType type;
    }

    public AudioData[] audioData;

    void Awake()
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

    void Start()
    {
        gameSource.volume = volume;
        playerSource.volume = volume;
    }

    public void PlaySound(AudioType type, AudioSourceType sourceType)
    {
        AudioClip clip = getClip(type);

        if (clip == null) return;

        if (sourceType == AudioSourceType.game)
        {
            gameSource.PlayOneShot(clip);
        }

        else if (sourceType == AudioSourceType.player)
        {
            playerSource.PlayOneShot(clip);
        }
    }

     public AudioClip getClip(AudioType type)
    {
        foreach (AudioData data in audioData)
        {
            if (data.type == type)
            {
                return data.clip;
            }
        }

        Debug.LogError("AudioManager: No clip found for type " + type);
        return null;
    }

    public void StopSound(AudioType type, AudioSourceType sourceType)
    {
        AudioClip clip = getClip(type);

        if (sourceType == AudioSourceType.game)
        {
            gameSource.Stop();
        }

        else if (sourceType == AudioSourceType.player)
        {
            playerSource.Stop();
        }
    }
}
    