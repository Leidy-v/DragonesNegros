using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("Salto y Gravedad")]
    public float gravity = -9.81f;
    public float jumpForce = 5f;

    [Header("Ataque")]
    public float attackRange = 1.2f;
    public float attackRadius = 0.5f;
    public float attackCooldown = 0.5f;
    public LayerMask enemyLayer;

    CharacterController controller;
    Vector3 velocity;
    bool canAttack = true;
    Transform cam;

    private AudioManager audioManager;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;

        audioManager = Object.FindFirstObjectByType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogWarning("No se encontró AudioManager en la escena");
        }
    }

    void Update()
    {
        Mover();
        Saltar();
        Atacar();
    }

    void Mover()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 move = cam.forward * v + cam.right * h;
        move.y = 0f;

        if (move.magnitude > 0.1f)
        {
            Quaternion rot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

            //L_Reproducir sonido de caminar solo si está en el suelo
            if (controller.isGrounded && audioManager != null && audioManager.walk != null && !audioManager.SFXSource.isPlaying)
            {
                audioManager.PlaySFX(audioManager.walk);
            }

        }

        controller.Move(move.normalized * speed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }

    void Saltar()
    {
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);

            //L_Reproducir sonido de salto
            if (audioManager != null && audioManager.jump != null)
            {
                audioManager.PlaySFX(audioManager.jump);
            }
            else
            {
                Debug.LogWarning("No hay clip de salto asignado");
            }
        }
    }

    void Atacar()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canAttack)
        {
            canAttack = false;

            Vector3 attackOrigin = transform.position + transform.forward * attackRange;
            Collider[] hitObjects = Physics.OverlapSphere(attackOrigin, attackRadius, enemyLayer);

            foreach (Collider obj in hitObjects)
            {
                Debug.Log("Golpeaste a: " + obj.name);

                CoinChest chest = obj.GetComponent<CoinChest>();
                if (chest != null)
                {
                    chest.OpenChest(); // Si es un cofre, se abre
                }
                else
                {
                    Destroy(obj.gameObject); // Si es enemigo, se destruye
                }
            }

            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    void ResetAttack()
    {
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackOrigin = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(attackOrigin, attackRadius);
    }
}
