using UnityEngine;
using System.Collections;

public class RockBlockManager : MonoBehaviour
{
    public static RockBlockManager instancia;

    public bool bajandoGlobal = true;
    public float intervaloCambio = 1f;

    void Awake()
    {
        instancia = this;
        StartCoroutine(CicloGlobal());
    }

    IEnumerator CicloGlobal()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(intervaloCambio);
            bajandoGlobal = !bajandoGlobal;
        }
    }
}
