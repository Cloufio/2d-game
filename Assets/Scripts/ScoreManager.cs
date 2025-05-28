// ScoreManager.cs
using UnityEngine;
using UnityEngine.UI; // Ensure this is present for legacy UI Text

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    // --- Score Variables (Existing) ---
    public Text scoreTextElement;
    private int currentScore = 0;

    // --- Timer Variables (Existing) ---
    public Text timerTextElement;
    public float timeRemaining = 60f;
    public bool timerIsRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentScore = 0;
        UpdateScoreDisplay();

        timerIsRunning = true;
        DisplayTime(timeRemaining); // Initialize timer display
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                DisplayTime(timeRemaining); // Ensure display shows 00:00:00
                // TimeUpActions(); // Call your game over logic
            }
        }
    }

    public void AddScore(int pointsToAdd)
    {
        currentScore += pointsToAdd;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreTextElement != null)
        {
            scoreTextElement.text = "Score : " + currentScore;
        }
        else
        {
            Debug.LogError("Score Text Element is not assigned in the ScoreManager!");
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }

    // --- Timer Display Method (MODIFIED) ---
    void DisplayTime(float timeToDisplay)
    {
        if (timerTextElement == null)
        {
            Debug.LogError("Timer Text Element is not assigned in the ScoreManager!");
            return;
        }

        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        // Calculate minutes, seconds, and hundredths of a second (milliseconds)
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        // To get the hundredths of a second part:
        float milliseconds = Mathf.FloorToInt((timeToDisplay * 100f) % 100f);

        // Update the timerTextElement with the new format
        timerTextElement.text = string.Format("Time : {0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
    }

    // void TimeUpActions()
    // {
    //    Debug.Log("GAME OVER - Time's Up!");
    //    // Time.timeScale = 0;
    // }
}