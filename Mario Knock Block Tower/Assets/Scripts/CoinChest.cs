using UnityEngine;
using System.Collections;

public class CoinChest : MonoBehaviour
{
    [Header("Monedas")]
    public GameObject coinPrefab;
    public Transform spawnPoint;
    public int coinCount = 10;
    public float spawnDelay = 0.1f;

    [Header("Efectos")]
    public ParticleSystem sparkleEffect;
    public AudioSource openSound;

    private bool opened = false;

    public void OpenChest()
    {
        if (opened) return;
        opened = true;

        if (openSound) openSound.Play();
        if (sparkleEffect) sparkleEffect.Play();

        StartCoroutine(SpawnCoins());
    }

    private IEnumerator SpawnCoins()
    {
        for (int i = 0; i < coinCount; i++)
        {
            Instantiate(coinPrefab, spawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
