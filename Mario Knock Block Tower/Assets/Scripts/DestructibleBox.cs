using UnityEngine;

public class DestructibleBox : MonoBehaviour
{
    [Header("Efectos")]
    public GameObject explosionFXPrefab;
    public float destroyDelay = 0.5f;

    private AudioManager audioManager;

    private void Awake()
{
    if (audioManager == null)
        audioManager = Object.FindFirstObjectByType<AudioManager>();
}

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;

        if (explosionFXPrefab != null)
        {
            GameObject fx = Instantiate(explosionFXPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 2f); // destruye el efecto después de 2 segundos
        }

        // Reproduce el sonido de explosión
        if (audioManager != null && audioManager.boxExplosion != null)
        {
            audioManager.PlaySFX(audioManager.boxExplosion);
        }

        else
        {
            Debug.LogWarning("AudioManager o clip boxExplosion no asignado.");
        }
    }
}

