using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(transform.parent.gameObject); // destruye la moneda completa
        }
    }
}
