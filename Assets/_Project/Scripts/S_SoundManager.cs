using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Channels")]
    [SerializeField] private SFXChannel sfxChannel;
    [SerializeField] private MusicChannel musicChannel;

    public SFXChannel SFX => sfxChannel;
    public MusicChannel Music => musicChannel;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    
}

[System.Serializable]
public class SFXChannel
{
    [SerializeField] private AudioSource source;

    public void PlayOneShot(AudioClip clip, float volume = 1f)
    {
        source.PlayOneShot(clip, volume);
    }

    public void PlayOnPosition(AudioClip clip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }
}

[System.Serializable]
public class MusicChannel
{
    [SerializeField] private AudioSource source;
    public void Play(AudioClip clip, bool loop = true)
    {
        if (source.clip == clip && source.isPlaying) return;

        source.clip = clip;
        source.loop = loop;
        source.Play();
    }
    public void Stop() => source.Stop();
    public void Pause() => source.Pause();
    public void Resume() => source.UnPause();
}