using UnityEngine;
using System.Collections;

public class RockBlockManager : MonoBehaviour
{
    // Static instance to allow global access from any RockBlock script
    public static RockBlockManager instancia;

    // Global flag that determines whether all RockBlocks are moving down
    public bool bajandoGlobal = true;

    // Time interval (in seconds) before toggling the movement direction
    public float intervaloCambio = 1f;

    void Awake()
    {
        // Store this script instance for global reference
        instancia = this;

        // Start the coroutine that periodically changes the movement direction
        StartCoroutine(CicloGlobal());
    }

    IEnumerator CicloGlobal()
    {
        // Infinite loop that alternates the global movement direction of blocks
        while (true)
        {
            // Wait for the specified real-time interval before toggling
            yield return new WaitForSecondsRealtime(intervaloCambio);

            // Reverse the direction: if moving down, switch to up (and vice versa)
            bajandoGlobal = !bajandoGlobal;
        }
    }
}
