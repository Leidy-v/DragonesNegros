using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource sfxSource;
    public AudioSource musicSource;

    [Header("Clips de sonido")]
    public AudioClip hitBlockClip;
    public AudioClip blockBreakClip;
    public AudioClip starAppearClip;
    public AudioClip winClip;
    public AudioClip jumpClip;
    public AudioClip backgroundMusic;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        PlayMusic(backgroundMusic);
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}
