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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
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
        }
    }

    void Atacar()
    {
        if (Input.GetKeyDown(KeyCode.Z) && canAttack)
        {
            canAttack = false;

            Collider[] hitEnemies = Physics.OverlapSphere(transform.position + transform.forward * attackRange, attackRadius, enemyLayer);

            foreach (Collider enemy in hitEnemies)
            {
                Debug.Log("Golpeaste a: " + enemy.name);
                Destroy(enemy.gameObject);
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
        Gizmos.DrawWireSphere(transform.position + transform.forward * attackRange, attackRadius);
    }
}
