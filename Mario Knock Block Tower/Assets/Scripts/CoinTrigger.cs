using UnityEngine;

public class CoinTrigger : MonoBehaviour
{
    // This method is automatically called when another collider enters the trigger zone
    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Destroy the entire coin object (the parent of this trigger)
            // This simulates the player collecting the coin
            Destroy(transform.parent.gameObject);
        }
    }
}
