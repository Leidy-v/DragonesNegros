using UnityEngine;
using TMPro;
using System.Collections;

public class Tiempo_temporizador : MonoBehaviour
{
    [Header("Configuración")]
    public float TimeRemaining = 10f;
    public TextMeshProUGUI TimerText;
    [Tooltip("Cuánto más arriba se posicionan los números respecto al centro (anchoredPosition Y).")]
    public float numberYOffset = 600f;

    [Header("Mensajes de Estado")]
    public string startMessage = "START";
    public string winMessage = "FINISH";
    public string lossMessage = "GAME OVER";

    public enum GameState { START, RUNNING, FINISH, GAME_OVER }
    public GameState currentState = GameState.START;
    private RectTransform timerRect;
    private Vector2 centerPosition;
    private Vector2 topPosition;

    void Start()
    {
        if (TimerText == null)
        {
            Debug.LogError("[Tiempo_temporizador] TimerText no está asignado en el Inspector.");
            enabled = false;
            return;
        }

        timerRect = TimerText.GetComponent<RectTransform>();
        centerPosition = timerRect.anchoredPosition;
        topPosition = new Vector2(centerPosition.x, centerPosition.y + numberYOffset);
        StartCoroutine(StartCountdownSequence());
    }

    IEnumerator StartCountdownSequence()
    {
        TimerText.text = startMessage;
        TimerText.fontSize = 220;
        timerRect.anchoredPosition = centerPosition;
        currentState = GameState.START;

        yield return new WaitForSeconds(1.5f);

        float elapsed = 0f;
        float duration = 0.9f;
        Vector2 initialPos = centerPosition;
        float startSize = 220f;
        float endSize = 110f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            timerRect.anchoredPosition = Vector2.Lerp(initialPos, topPosition, t);
            TimerText.fontSize = Mathf.Lerp(startSize, endSize, t);
            yield return null;
        }

        timerRect.anchoredPosition = topPosition;
        TimerText.fontSize = endSize;
        currentState = GameState.RUNNING;
        DisplayTime(TimeRemaining);
    }

    void Update()
    {
        if (currentState == GameState.RUNNING)
        {
            if (TimeRemaining > 0)
            {
                TimeRemaining -= Time.deltaTime;
                DisplayTime(TimeRemaining);
            }
            else
            {
                TimeRemaining = 0;
                currentState = GameState.GAME_OVER;
                StartCoroutine(ShowFinishOrGameOver(lossMessage));
            }
        }
    }

    public void PlayerWins()
    {
        if (currentState == GameState.RUNNING)
        {
            currentState = GameState.FINISH;
            StartCoroutine(ShowFinishOrGameOver(winMessage));
        }
    }

    IEnumerator ShowFinishOrGameOver(string message)
    {
        TimerText.text = message;

        float elapsed = 0f;
        float duration = 0.8f;
        Vector2 initialPos = timerRect.anchoredPosition;
        float startSize = TimerText.fontSize;
        float endSize = 300f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / duration);
            timerRect.anchoredPosition = Vector2.Lerp(initialPos, centerPosition, t);
            TimerText.fontSize = Mathf.Lerp(startSize, endSize, t);
            yield return null;
        }

        timerRect.anchoredPosition = centerPosition;
        TimerText.fontSize = endSize;
    }

    void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0f)
            timeToDisplay = 0f;

        int seconds = Mathf.CeilToInt(timeToDisplay);
        TimerText.text = seconds.ToString();
        if (currentState == GameState.RUNNING)
            timerRect.anchoredPosition = topPosition;
    }
}