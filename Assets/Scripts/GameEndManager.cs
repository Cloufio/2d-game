using UnityEngine;
using UnityEngine.UI;         // Required for UI Image
using UnityEngine.SceneManagement; // Required for scene management
using System.Collections;     // Required for Coroutines

public class GameEndManager : MonoBehaviour
{
    [Header("Bad Ending Conditions")]
    public int scoreThresholdForBadEnding = 30;
    // We no longer need a 'timeOverThresholdInSeconds' here,
    // as ScoreManager will tell us when time is up.

    [Header("Fade Settings")]
    public Image fadePanel;       // Assign your UI Image for fading in the Inspector
    public float fadeDuration = 2f; // How long the fade should take
    // public string badEndingSceneName = "Scenes/BadEnding"; // Adjusted from your previous update

    private bool conditionsHaveBeenMet = false;
    private ScoreManager scoreManagerInstance;

    void Start()
    {
        // Get the instance of your existing ScoreManager
        scoreManagerInstance = ScoreManager.Instance;

        if (scoreManagerInstance == null)
        {
            Debug.LogError("GameEndManager: ScoreManager.Instance not found! Make sure ScoreManager is active in your scene and its Instance is set up correctly.");
            enabled = false; // Disable this script if ScoreManager isn't found
            return;
        }

        if (fadePanel == null)
        {
            Debug.LogError("GameEndManager: Fade Panel has not been assigned in the Inspector!");
            enabled = false; // Disable if no fade panel
            return;
        }

        // Ensure the fade panel is active but initially transparent
        fadePanel.gameObject.SetActive(true);
        fadePanel.color = new Color(fadePanel.color.r, fadePanel.color.g, fadePanel.color.b, 0f);
    }

    void Update()
    {
        // If conditions already met, or ScoreManager isn't available, do nothing
        if (conditionsHaveBeenMet || scoreManagerInstance == null)
        {
            return;
        }

        // Get current score from your ScoreManager
        int currentScore = scoreManagerInstance.currentScore;

        // Check if time has run out based on ScoreManager's state
        // Time is over if the timer is no longer running AND time remaining is zero or less.
        // ScoreManager sets timerIsRunning to false when timeRemaining hits 0.
        bool timeIsOver = !scoreManagerInstance.timerIsRunning && scoreManagerInstance.timeRemaining <= 0;

        // Check for the bad ending conditions
        if (currentScore > scoreThresholdForBadEnding && timeIsOver)
        {
            conditionsHaveBeenMet = true;
            Debug.Log("Bad Ending conditions met! Score: " + currentScore + ", Time has run out.");
            StartCoroutine(PerformFadeAndLoadScene());
        }
    }

    IEnumerator PerformFadeAndLoadScene()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f; // IMPORTANT: Unpause if time was stopped
            Debug.Log("Time.timeScale was 0, resetting to 1 for fade.");
        }

        Debug.Log("Starting fade to black...");
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, newAlpha);
            yield return null; // Wait for the next frame
        }

        // Ensure it's fully opaque before changing scene
        fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, 1f);
        // Debug.Log("Fade complete. Loading scene: " + badEndingSceneName);

        SceneManager.LoadScene(2);
    }
}