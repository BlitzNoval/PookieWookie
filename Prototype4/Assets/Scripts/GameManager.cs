using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI scoreText, timerText, accuracyText;
    public Button playButton;
    public Button restartButton, quitButton;
    public GameObject startPanel, endGamePanel;

    public int score, totalShots, shotsHit;
    public float timer;
    public bool gameStarted = false;

    private int hits = 0;
    public List<float> reactionTimes = new List<float>();

    public TextMeshProUGUI averageReactionTimeText;

    public GameObject pauseMenuPanel;
    public Slider sensitivitySlider;

    public bool isPaused = false;

    public TextMeshProUGUI sensitivityValueText;
    public PlayerController playerController;

    public int highestConsecutiveHits = 0;
    public TextMeshProUGUI highestHitsText;


    void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    else
    {
        Destroy(gameObject);
    }
}


    void Start()
    {
        sensitivitySlider.value = PlayerController.sensitivity;
        sensitivitySlider.onValueChanged.AddListener(HandleSensitivityChange);
        UpdateSensitivityText(sensitivitySlider.value);
        endGamePanel.SetActive(false);
        startPanel.SetActive(true);
        playButton.onClick.AddListener(StartGame);
        ManageCursor(true);
        pauseMenuPanel.SetActive(false);
        sensitivitySlider.onValueChanged.AddListener(HandleSensitivityChange);
    }

    public void StartGame()
{
    gameStarted = true;
    isPaused = false;
    Time.timeScale = 1;

    score = 0;
    totalShots = 0;
    shotsHit = 0;
    hits = 0;
    reactionTimes.Clear();

    timer = 60f;
    UpdateScoreUI();
    UpdateTimerUI();
    startPanel.SetActive(false);
    endGamePanel.SetActive(false);
    pauseMenuPanel.SetActive(false);
    ManageCursor(false);

    if (playerController != null)
    {
        playerController.ResetCameraToStartPoint();
    }
}


    void Update()
{
    if (gameStarted && timer > 0)
    {
        timer -= Time.deltaTime;
        UpdateTimerUI();
    }
    else if (gameStarted && timer <= 0)
    {
        EndGame();
    }

    if (Input.GetKeyDown(KeyCode.Escape))
    {
        TogglePause();
    }
}


    void EndGame()
    {
    gameStarted = false;
    endGamePanel.SetActive(true);

    float accuracy = (shotsHit / (float)totalShots) * 100;
    accuracyText.text = "Accuracy: " + accuracy.ToString("F2") + "%";
    averageReactionTimeText.text = "Average Reaction Time: " + CalculateAverageReactionTime().ToString("F2") + " seconds";  // Display average reaction time

    highestHitsText.text = "Highest Hits in a Row: " + highestConsecutiveHits;

    ManageCursor(true);
    }




    void ManageCursor(bool visible)
    {
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (gameStarted)
        {
            ManageCursor(!hasFocus);
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score;
    }

    void UpdateTimerUI()
    {
        timerText.text = "Time: " + Mathf.Ceil(timer);
    }

    public void UpdateScore(int value)
    {
        score += value;
        UpdateScoreUI();
    }

    public void RegisterShot()
    {
    if (gameStarted)
    {
        totalShots++;
    }
    }

    public void RegisterHit(float spawnTime)
{
    if (gameStarted)
    {
        shotsHit++;
        hits++;

        float reactionTime = Time.time - spawnTime;
        reactionTimes.Add(reactionTime);

        if (hits > highestConsecutiveHits)
        {
            highestConsecutiveHits = hits;
        }
    }
}


    public void RegisterMiss()
{
    totalShots++;
    hits = 0;
}

    public float CalculateAccuracy()
    {
        return (float)hits / totalShots * 100;
    }

    public float CalculateAverageReactionTime()
    {
        if (reactionTimes.Count == 0) return 0;
        float total = 0;
        foreach (float time in reactionTimes)
        {
            total += time;
        }
        return total / reactionTimes.Count;
    }

    public void DisplayResults()
    {
        Debug.Log($"Accuracy: {CalculateAccuracy()}%");
        Debug.Log($"Average Reaction Time: {CalculateAverageReactionTime()} seconds");
    }

    public void RestartGame()
{
    gameStarted = false;
    isPaused = false;
    Time.timeScale = 1;

    score = 0;
    totalShots = 0;
    shotsHit = 0;
    hits = 0;
    reactionTimes.Clear();
    UpdateScoreUI();
    UpdateTimerUI();
    accuracyText.text = "Accuracy: 0%";
    averageReactionTimeText.text = "Average Reaction Time: 0 seconds";
    startPanel.SetActive(true);
    endGamePanel.SetActive(false);
    pauseMenuPanel.SetActive(false);
    ManageCursor(true);

    if (playerController != null)
    {
        playerController.ResetCameraToStartPoint();
    }
}



public void QuitGame()
{
    Application.Quit();

    #if UNITY_EDITOR
    UnityEditor.EditorApplication.isPlaying = false;
    #endif
}

void TogglePause()
{
    isPaused = !isPaused;
    pauseMenuPanel.SetActive(isPaused);
    ManageCursor(isPaused);
    Time.timeScale = isPaused ? 0 : 1;
}


public void ResumeGame()
{
    TogglePause();

    if (!isPaused && playerController != null)
    {
        playerController.ResetCameraToStartPoint();
    }
}

    private void UpdateSensitivityText(float sensitivity)
    {
        if (sensitivityValueText != null)
            sensitivityValueText.text = "Sensitivity: " + sensitivity.ToString("F2");
        else
            Debug.LogError("Sensitivity Value Text is not assigned!");
    }

    void HandleSensitivityChange(float value)
    {
        PlayerController.sensitivity = value;
        UpdateSensitivityText(value);
    }

}
