using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;               // Player movement speed
    public float rotationSpeed = 10f;      // Speed of player rotation

    [Header("Salto y Gravedad")]
    public float gravity = -9.81f;         // Custom gravity value
    public float jumpForce = 5f;           // Jump strength applied to the player

    [Header("Ataque")]
    public float attackRange = 1.2f;       // Distance from the player where the attack starts
    public float attackRadius = 0.5f;      // Radius of the attack area
    public float attackCooldown = 0.5f;    // Cooldown time between attacks
    public LayerMask enemyLayer;           // Layer mask used to detect enemies or interactable objects

    CharacterController controller;        // Reference to the CharacterController component
    Vector3 velocity;                      // Player velocity used for gravity and jumping
    bool canAttack = true;                 // Controls if the player can attack again
    Transform cam;                         // Reference to the main camera transform

    private AudioManager audioManager;     // Reference to AudioManager

    void Start()
    {
        // Get the CharacterController and main camera references
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        // Find the AudioManager in the scene
        audioManager = Object.FindFirstObjectByType<AudioManager>();

        // If no AudioManager is found, log a warning message
        if (audioManager == null)
        {
            Debug.LogWarning("No se encontró AudioManager en la escena");
        }
    }

    void Update()
    {
        // Handle movement, jumping, and attacking each frame
        Mover();
        Saltar();
        Atacar();
    }

    void Mover()
    {
        // Get input values for horizontal and vertical movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // Calculate movement direction based on camera orientation
        Vector3 move = cam.forward * v + cam.right * h;
        move.y = 0f;


        // Rotate the player towards the movement direction
        if (move.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);


            //L_Play walking sound only when the player is on the ground
            if (controller.isGrounded && audioManager != null && audioManager.walk != null && !audioManager.SFXSource.isPlaying)
            {
                audioManager.PlaySFX(audioManager.walk);
            }

        }

        // Apply horizontal movement to the CharacterController
        controller.Move(move.normalized * speed * Time.deltaTime);

        // Reset downward velocity when grounded
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
        // Apply gravity manually
        velocity.y += gravity * Time.deltaTime;
        // Apply vertical velocity
        controller.Move(velocity * Time.deltaTime);

    }

    void Saltar()
    {
        // Perform jump if the space key is pressed and player is grounded
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

            //L_Play jump sound effect
            if (audioManager != null && audioManager.jump != null)
            {
                audioManager.PlaySFX(audioManager.jump);
            }
            else
            {
                Debug.LogWarning("No hay clip de salto asignado");     // Log a warning if the jump sound clip is not assigned
            }
        }
    }

    void Atacar()
    {
        // Perform attack if 'Z' key is pressed and player can attack
        if (Input.GetKeyDown(KeyCode.Z) && canAttack)
        {
            canAttack = false; // Disable attacking temporarily (cooldown)

            // Calculate attack origin point in front of the player
            Vector3 attackOrigin = transform.position + transform.forward * attackRange;

            // Detect colliders in the attack area using a sphere
            Collider[] hitObjects = Physics.OverlapSphere(attackOrigin, attackRadius, enemyLayer);

            // Loop through all detected objects
            foreach (Collider obj in hitObjects)
            {
                Debug.Log("Golpeaste a: " + obj.name); // Log hit target in console

                // Check if the object is a chest
                CoinChest chest = obj.GetComponent<CoinChest>();
                if (chest != null)
                {
                    chest.OpenChest(); // If it's a chest, open it
                }
                else
                {
                    Destroy(obj.gameObject); // If it's an enemy, destroy it
                }
            }

            // Reactivate attack after cooldown
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack()
    {
        // Reset attack permission after cooldown
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        // Visualize attack range in the Scene view
        Gizmos.color = Color.red;
        Vector3 attackOrigin = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(attackOrigin, attackRadius);
    }
}

