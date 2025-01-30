using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource defaultSource;

    public bool musicPaused = false;
    public bool effectsPaused = false;

    public AudioSource MusicSource { get { return musicSource; } }
    public AudioSource EffectsSource { get { return defaultSource; } }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (Instance) Destroy(Instance.gameObject);

        Instance = this;

        DontDestroyOnLoad(gameObject);
        musicSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {
        defaultSource.clip = clip;
        defaultSource.Play();
    }
}
