using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    // Reference to the CoinChest script assigned manually in the Inspector
    public CoinChest chest;

    // This method is automatically called when another collider enters this trigger area
    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Call the OpenChest() method to open the chest when the player approaches
            chest.OpenChest();
        }
    }
}