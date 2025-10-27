using UnityEngine;

public class RockBlock : MonoBehaviour
{
    [Header("Vertical Movement Settings")]
    public float moveHeight = 2f;         // Maximum height the block moves upward
    public float fallSpeed = 6f;          // Speed when moving downward
    public float riseSpeed = 4f;          // Speed when moving upward

    [Header("Ground Detection")]
    public LayerMask groundLayer;         // Layer mask used to detect ground ("Ground" layer must be included)

    [Header("Player Settings")]
    public string playerTag = "Player";   // Tag used to identify the player object
    public bool restartScene = false;     // If true, the scene will reload when the player is crushed
    public bool destroyPlayer = true;     // If true, destroys the player on collision

    private Vector3 initialPosition;      // Stores the starting position of the block
    private bool hasSupport = true;       // Determines whether the block is supported from below

    void Start()
    {
        // Save the initial position for movement reference
        initialPosition = transform.position;
    }

    void Update()
    {
        // Prevent logic execution if the global RockBlockManager is missing
        if (RockBlockManager.instancia == null) return;

        // If the block has no support, it will fall continuously until it finds ground
        if (!hasSupport)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            // Combine the ground layer and the resting block layer
            LayerMask fullGround = groundLayer | LayerMask.GetMask("RockResting");

            // Check if the block is touching any valid ground
            if (Physics.Raycast(transform.position, Vector3.down, 0.1f, fullGround))
            {
                gameObject.layer = LayerMask.NameToLayer("RockResting"); // Change layer to resting
                enabled = false; // Disable the script to make the block static
            }

            return;
        }

        // Controlled up and down motion managed globally by RockBlockManager
        if (RockBlockManager.instancia.bajandoGlobal)
        {
            // Move block downward
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

            // Stop block at its initial Y position
            if (transform.position.y <= initialPosition.y)
            {
                transform.position = initialPosition;
            }
        }
        else
        {
            // Move block upward
            transform.Translate(Vector3.up * riseSpeed * Time.deltaTime);

            // Limit upward movement to the configured height
            if (transform.position.y >= initialPosition.y + moveHeight)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    initialPosition.y + moveHeight,
                    transform.position.z
                );
            }
        }
    }

    void FixedUpdate()
    {
        // Only check for support when near the starting height
        if (transform.position.y <= initialPosition.y + 0.2f)
        {
            RaycastHit hit;

            // Cast a ray downward to check if an enemy is under the block
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 5f, LayerMask.GetMask("Enemy")))
            {
                hasSupport = true; // There is support below
                return;
            }

            // No support detected below the block
            hasSupport = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // When the block collides with the player
        if (collision.gameObject.CompareTag(playerTag))
        {
            // Destroy the player object
            if (destroyPlayer)
                Destroy(collision.gameObject);

            // Restart the scene if configured
            if (restartScene)
            {
                FindFirstObjectByType<UIManager>().ShowGameOverPanel();
            }
        }
    }
}