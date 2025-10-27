using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Física inicial")]
    public float jumpForce = 2f;          // Upward force applied when the coin spawns
    public float lateralSpread = 0.3f;    // Random horizontal variation when the coin is ejected

    [Header("Imán hacia el jugador")]
    public float magnetRange = 5f;        // Distance at which the coin starts being attracted to the player
    public float magnetForce = 10f;       // Strength of the magnetic attraction force
    public float maxMagnetSpeed = 5f;     // Maximum speed the coin can reach while moving toward the player

    private Transform player;             // Reference to the player's transform
    private Rigidbody rb;                 // Reference to the coin's Rigidbody
    private bool attracted = false;       // True when the coin starts moving toward the player

    private AudioManager audioManager; // Reference to AudioManager
    void Start()
    {
        // Get or add the Rigidbody component
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        // Configure Rigidbody physics properties
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = 0.2f;
        rb.linearDamping = 1f;   // Slows down linear motion over time
        rb.angularDamping = 1f;  // Reduces rotation speed over time

        // Apply an initial random force to make the coin jump and scatter
        Vector3 force = new Vector3(
            Random.Range(-lateralSpread, lateralSpread),
            jumpForce,
            Random.Range(-lateralSpread, lateralSpread)
        );
        rb.AddForce(force, ForceMode.Impulse);

        // Find the player in the scene using its tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // Note: The coin is not destroyed automatically after time


        //L_ Find the AudioManager in the scene
        audioManager = Object.FindFirstObjectByType<AudioManager>();  
        if (audioManager != null && audioManager.coinSound != null)   // Check if the AudioManager and the coin sound clip are assigned
        {
            audioManager.PlaySFX(audioManager.coinSound);   // Play the coin sound effect
        }
        else
        {
            Debug.LogWarning("AudioManager o clip de moneda no asignado."); // Log a warning if the AudioManager or the coin sound clip is missing
        }
    }



    void FixedUpdate()
    {
        // Skip logic if there is no player or Rigidbody reference
        if (player == null || rb == null) return;

        // Calculate distance between the coin and the player
        float distance = Vector3.Distance(transform.position, player.position);

        // Activate magnetic attraction if the player is within range
        if (distance < magnetRange)
        {
            attracted = true;
        }

        // If attraction is active, move the coin toward the player
        if (attracted)
        {
            Vector3 direction = (player.position - transform.position).normalized; // Direction toward player
            rb.AddForce(direction * magnetForce, ForceMode.Acceleration);          // Apply continuous force
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxMagnetSpeed); // Limit speed
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // When the player touches the coin, destroy it (simulate collection)
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<UIManager>()?.ShowFinishPanel();

            Destroy(gameObject);
        }
    }
}
