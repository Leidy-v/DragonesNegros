using UnityEngine;
using System.Collections;

public class RockBlock : MonoBehaviour
{
    [Header("Movimiento vertical")]
    public float alturaMovimiento = 2f;
    public float velocidadBajada = 6f;
    public float velocidadSubida = 4f;
    public float pausaAbajo = 0.5f;
    public float pausaArriba = 1f;

    [Header("Detección del suelo")]
    public LayerMask sueloLayer; 

    [Header("Jugador")]
    public string tagJugador = "Player";
    public bool reiniciarEscena = false;
    public bool destruirJugador = true;

    private Vector3 posicionInicial;
    private bool bajando = true;
    private bool enPausa = false;
    private bool tieneSoporte = true;

    void Start()
    {
        posicionInicial = transform.position;
    }

    void Update()
    {
        if (enPausa) return;

        if (!tieneSoporte)
        {
            
            transform.Translate(Vector3.down * velocidadBajada * Time.deltaTime);

            LayerMask sueloCompleto = sueloLayer | LayerMask.GetMask("RockResting");

            if (Physics.Raycast(transform.position, Vector3.down, 0.6f, sueloCompleto))
            {
                enPausa = true;
                gameObject.layer = LayerMask.NameToLayer("RockResting"); // Se convierte en suelo para otros bloques
            }

            return;
        }

        if (bajando)
        {
            transform.Translate(Vector3.down * velocidadBajada * Time.deltaTime);

            if (Physics.Raycast(transform.position, Vector3.down, 0.6f, LayerMask.GetMask("Enemy")))
            {
                StartCoroutine(PausarYSubir(pausaAbajo));
            }
        }
        else
        {
            transform.Translate(Vector3.up * velocidadSubida * Time.deltaTime);

            if (transform.position.y >= posicionInicial.y + alturaMovimiento)
            {
                StartCoroutine(PausarYBajar(pausaArriba));
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

    IEnumerator PausarYSubir(float tiempo)
    {
        enPausa = true;
        yield return new WaitForSeconds(tiempo);
        bajando = false;
        enPausa = false;
    }

    IEnumerator PausarYBajar(float tiempo)
    {
        enPausa = true;
        yield return new WaitForSeconds(tiempo);
        bajando = true;
        enPausa = false;
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