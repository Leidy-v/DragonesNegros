using UnityEngine;

public class ChestTrigger : MonoBehaviour
{
    public CoinChest chest; // Asignas el cofre desde el Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chest.OpenChest(); // Llama al método del cofre
        }
    }
}
