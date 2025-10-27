using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("--- Audio Sources ---")]
    [SerializeField] AudioSource musicSource; // Plays background music
    [SerializeField] AudioSource sfxSource;   // Plays sound effects (SFX)

    // Public read-only access to the SFX AudioSource (for external scripts)
    public AudioSource SFXSource => sfxSource;

    [Header("--- Audio Clips ---")]
    public AudioClip background;     // Background music
    public AudioClip blockSound;     // Sound when hitting or breaking a block
    public AudioClip coinSound;      // Sound when a coin is collected
    public AudioClip jump;           // Sound when the player jumps
    public AudioClip walk;           // Sound when the player walks
    public AudioClip boxExplosion;   // Sound for box explosion
    public AudioClip gameOver;       // Sound for game over
    public AudioClip gameWin;        // Sound for winning
    public AudioClip interfazSound;  // Sound for UI interaction
    public AudioClip clicksUI;       // Sound for button clicks

    private void Start()
    {
        // Assign and start playing the background music when the game begins
        musicSource.clip = background;
        musicSource.Play();
    }

    /// Plays a one-shot sound effect (SFX).
    /// This method is used by other scripts to trigger sound effects.
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip); // Plays the given sound once without interrupting others
    }
}


