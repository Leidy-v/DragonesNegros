using UnityEngine;

public class RockBlock : MonoBehaviour
{
    [Header("Movimiento vertical")]
    public float alturaMovimiento = 2f;
    public float velocidadBajada = 6f;
    public float velocidadSubida = 4f;

    [Header("Detección del suelo")]
    public LayerMask sueloLayer; // Incluye "Ground"

    [Header("Jugador")]
    public string tagJugador = "Player";
    public bool reiniciarEscena = false;
    public bool destruirJugador = true;

    private Vector3 posicionInicial;
    private bool tieneSoporte = true;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        if (RockBlockManager.instancia == null) return;

        if (!tieneSoporte)
        {
            transform.Translate(Vector3.down * velocidadBajada * Time.deltaTime);

            LayerMask sueloCompleto = sueloLayer | LayerMask.GetMask("RockResting");

            if (Physics.Raycast(transform.position, Vector3.down, 0.6f, sueloCompleto))
            {
                gameObject.layer = LayerMask.NameToLayer("RockResting");
                enabled = false; // ← Se convierte en plataforma estática
            }

            return;
        }

        if (RockBlockManager.instancia.bajandoGlobal)
        {
            transform.Translate(Vector3.down * velocidadBajada * Time.deltaTime);

            if (transform.position.y <= posicionInicial.y)
            {
                transform.position = posicionInicial;
            }
        }
        else
        {
            transform.Translate(Vector3.up * velocidadSubida * Time.deltaTime);

            if (transform.position.y >= posicionInicial.y + alturaMovimiento)
            {
                transform.position = new Vector3(transform.position.x, posicionInicial.y + alturaMovimiento, transform.position.z);
            }
        }
    }

    void FixedUpdate()
    {
        if (transform.position.y <= posicionInicial.y + 0.2f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, LayerMask.GetMask("Enemy")))
            {
                tieneSoporte = true;
                return;
            }

            tieneSoporte = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(tagJugador))
        {
            if (destruirJugador)
                Destroy(collision.gameObject);

            if (reiniciarEscena)
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
