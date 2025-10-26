using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---Audio Source--")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    public AudioSource SFXSource => sfxSource;

    [Header("---Audio Clip--")]
    public AudioClip background;
    public AudioClip blockSound;
    public AudioClip coinSound;
    public AudioClip jump;
    public AudioClip walk;
    public AudioClip boxExplosion;
    public AudioClip gameOver;
    public AudioClip gameWin;
    public AudioClip interfazSound;
    public AudioClip clicksUI;


    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)

    {
        sfxSource.PlayOneShot(clip);
    }


}

