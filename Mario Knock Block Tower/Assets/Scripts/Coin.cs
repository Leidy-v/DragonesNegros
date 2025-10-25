using UnityEngine;

public class Coin : MonoBehaviour
{
    [Header("Física inicial")]
    public float jumpForce = 2f;
    public float lateralSpread = 0.3f;

    [Header("Imán hacia el jugador")]
    public float magnetRange = 5f;
    public float magnetForce = 10f;
    public float maxMagnetSpeed = 5f;

    private Transform player;
    private Rigidbody rb;
    private bool attracted = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.mass = 0.2f;
        rb.linearDamping = 1f;
        rb.angularDamping = 1f;

        Vector3 force = new Vector3(
            Random.Range(-lateralSpread, lateralSpread),
            jumpForce,
            Random.Range(-lateralSpread, lateralSpread)
        );
        rb.AddForce(force, ForceMode.Impulse);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        // ❌ Eliminado: no se destruye por tiempo
    }

    void FixedUpdate()
    {
        if (player == null || rb == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < magnetRange)
        {
            attracted = true;
        }

        if (attracted)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.AddForce(direction * magnetForce, ForceMode.Acceleration);
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxMagnetSpeed);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
