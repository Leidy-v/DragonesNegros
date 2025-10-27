using UnityEngine;

public class DestructibleBox : MonoBehaviour
{
    [Header("Effects")]
    public GameObject explosionFXPrefab; // Prefab for the explosion particle effect
    public float destroyDelay = 0.5f;    // Optional delay before the box is destroyed

    private AudioManager audioManager;   // Reference to the AudioManager to play sounds

    private void Awake()
    {
        // Find the AudioManager in the scene if it hasn't been assigned
        if (audioManager == null)
            audioManager = Object.FindFirstObjectByType<AudioManager>();
    }

    private void OnDestroy()
    {
        // Prevent running this code if the scene is being unloaded (avoids errors on exit)
        if (!gameObject.scene.isLoaded) return;

        // Spawn explosion particle effect at the box’s position
        if (explosionFXPrefab != null)
        {
            GameObject fx = Instantiate(explosionFXPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f); // Automatically destroy the particles after 2 seconds
        }

        // Play the explosion sound effect
        if (audioManager != null && audioManager.boxExplosion != null)
        {
            audioManager.PlaySFX(audioManager.boxExplosion);
        }
        else
        {
            Debug.LogWarning("AudioManager or boxExplosion clip is not assigned.");
        }
    }
}


