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
    public GameObject pausePanel; //Pause Menu Panel

    [Header("Timer Number Sprites")]
    public UnityEngine.UI.Image numberImage;   // Reference to object Image
    public Sprite[] numberSprites;             // sprites 0 to 10

    [Header("Timer Settings")]
    //public TextMeshProUGUI timerText;   // Reference to text that displays the countdown
    public float gameDuration = 10f;    // Total time allowed for the player

    [Header("Timing Settings")]
    public float startPanelDuration = 2f; // Time the START panel stays visible
    public float restartDelay = 2f;       // Delay before restarting after Game Over

    private bool gameStarted = false;
    private bool isPaused = false;
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

    void Update()
    {
        // Press ESC to toggle pause menu
        if (gameStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
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
        int seconds = Mathf.CeilToInt(remainingTime);

        // Evitar errores si se pasa
        seconds = Mathf.Clamp(seconds, 0, numberSprites.Length - 1);

        // Change sprite
        numberImage.sprite = numberSprites[seconds];

        // Change number color
        if (seconds <= 3)
            numberImage.color = Color.red;
        else
            numberImage.color = Color.white;
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

    //Pause sistem
    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Debug.Log("RESUME button pressed");
        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }
}






