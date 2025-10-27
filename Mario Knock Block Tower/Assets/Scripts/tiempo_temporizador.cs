using UnityEngine;
using TMPro; // 1. NECESITAS ESTA LIBRERÍA PARA USAR TextMeshPro

public class tiempo_temporizador : MonoBehaviour
{
    // 2. Variables públicas visibles en el Inspector de Unity
    [Tooltip("Tiempo inicial en segundos (300 segundos = 5 minutos)")]
    public float TimeRemaining = 300f; //
    
    // Campo para conectar el componente de texto desde el Inspector
    [Tooltip("Arrastra el objeto 'Temporizador' aquí desde la Jerarquía")]
    public TextMeshProUGUI TimerText; //

    // Variable privada para detener el temporizador
    private bool timerIsRunning = true; 

    void Start()
    {
        // Asegúrate de que el temporizador comience a correr al inicio
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (TimeRemaining > 0)
            {
                // Disminuye el tiempo restante con el tiempo real transcurrido
                TimeRemaining -= Time.deltaTime; 
                // Llama a la función para actualizar el texto en pantalla
                DisplayTime(TimeRemaining); 
            }
            else
            {
                // El tiempo ha terminado
                Debug.Log("¡El tiempo ha terminado!");
                TimeRemaining = 0;
                timerIsRunning = false;
                // Aquí podrías agregar código para terminar el juego o la ronda
            }
        }
    }

    // Función para convertir los segundos a formato de minutos:segundos y actualizar la UI
    void DisplayTime(float timeToDisplay)
    {
        // Asegura que el tiempo no sea negativo
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        // 3. Cálculo de minutos y segundos
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);  // Minutos
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);  // Segundos restantes

        // 4. Formato y actualización
        // El formato "00" asegura que siempre tenga dos dígitos (Ej: 05 en lugar de 5)
        TimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); //
    }
}