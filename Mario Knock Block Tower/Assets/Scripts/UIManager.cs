using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject startPanel;
    public GameObject finishPanel;
    public GameObject gameOverPanel;
    public GameObject timerPanel;

    [Header("Timer Settings")]
    public TextMeshProUGUI timerText;   // Reference to text that displays the countdown
    public float gameDuration = 10f;    // Total time allowed for the player

    [Header("Timing Settings")]
    public float startPanelDuration = 2f; // Time the START panel stays visible
    public float restartDelay = 2f;       // Delay before restarting after Game Over

    private bool gameStarted = false;
    private float remainingTime;

    private AudioManager audioManager; // Reference to AudioManager


    void Start()
    {
        // Find the AudioManager in the scene
        audioManager = FindFirstObjectByType<AudioManager>();

        // Initialize UI state
        startPanel.SetActive(true);
        finishPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        timerPanel.SetActive(false);

        Time.timeScale = 0f; // Pause game while showing START panel

        // Begin showing the START panel, then automatically start the game
        StartCoroutine(StartPanelRoutine());
    }

    private IEnumerator StartPanelRoutine()
    {
        yield return new WaitForSecondsRealtime(startPanelDuration);

        startPanel.SetActive(false);
        gameStarted = true;
        Time.timeScale = 1f; // Resume game

        // Start the timer
        remainingTime = gameDuration;
        timerPanel.SetActive(true);
        StartCoroutine(TimerCountdown());
    }

    private IEnumerator TimerCountdown()
    {
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            UpdateTimerUI();
            yield return null;
        }

        // Time’s up and show Game Over
        if (gameStarted)
        {
            ShowGameOverPanel();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        int seconds = Mathf.CeilToInt(remainingTime);
        timerText.text = seconds.ToString();

        // Change color based on remaining time
        if (seconds <= 3)
            timerText.color = Color.red;
        else if (seconds <= 5)
            timerText.color = Color.yellow;
        else
            timerText.color = Color.white;
    }

    // Called when all coins are collected
    public void ShowFinishPanel()
    {
        if (!gameStarted) return;
        gameStarted = false;

        finishPanel.SetActive(true);
        timerPanel.SetActive(false);
        Time.timeScale = 0f;

        // Play game win sound
        if (audioManager != null && audioManager.gameWin != null)
        {
            audioManager.PlaySFX(audioManager.gameWin);
        }
    }

    // Called when player hits a rock or timer ends
    public void ShowGameOverPanel()
    {
        if (!gameStarted) return;
        gameStarted = false;

        timerPanel.SetActive(false);
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        gameOverPanel.SetActive(true);

        // Play game over sound
        if (audioManager != null && audioManager.gameOver != null)
        {
            audioManager.PlaySFX(audioManager.gameOver);
        }

        yield return new WaitForSecondsRealtime(restartDelay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}





