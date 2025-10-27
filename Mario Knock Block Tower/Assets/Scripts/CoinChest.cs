using UnityEngine;
using System.Collections;

public class CoinChest : MonoBehaviour
{
    [Header("Monedas")]
    public GameObject coinPrefab;      // Prefab of the coin to be instantiated
    public Transform spawnPoint;       // Position where coins will appear
    public int coinCount = 10;         // Total number of coins to spawn
    public float spawnDelay = 0.1f;    // Delay between each coin spawn for visual effect

    [Header("Efectos")]
    public ParticleSystem sparkleEffect; // Particle effect played when chest opens
    public AudioSource openSound;        // Sound played when chest opens

    private bool opened = false;         // Ensures the chest opens only once

    // Called to open the chest and start spawning coins
    public void OpenChest()
    {
        // Prevent multiple openings of the same chest
        if (opened) return;
        opened = true;

        // Play opening sound and visual effects if assigned
        if (openSound) openSound.Play();
        if (sparkleEffect) sparkleEffect.Play();

        // Start spawning coins over time
        StartCoroutine(SpawnCoins());
    }

    // Coroutine that spawns coins one by one with a small delay
    private IEnumerator SpawnCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            // Instantiate a coin at the defined spawn point
            Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);

            // Wait for the defined delay before spawning the next coin
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
