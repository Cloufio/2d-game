using UnityEngine;
using UnityEngine.UI;         // Required for UI Image
using UnityEngine.SceneManagement; // Required for scene management
using System.Collections;     // Required for Coroutines

public class GameEndManager : MonoBehaviour
{
    [Header("Game Conditions")]
    [Tooltip("The score threshold to differentiate between endings.")]
    public int scoreThresholdForGoodEnding = 30;

    [Header("Scene Transitions")]
    [Tooltip("Build index for the scene to load if score > scoreThresholdForGoodEnding.")]
    public int goodEndingSceneIndex = 2; // Default for score > 30
    [Tooltip("Build index for the scene to load if score <= scoreThresholdForGoodEnding.")]
    public int badEndingSceneIndex = 3;  // Default for score <= 30

    [Header("Fade Settings")]
    [Tooltip("The UI Image to use for fading in the Inspector.")]
    public Image fadePanel;
    [Tooltip("How long the fade should take in seconds.")]
    public float fadeDuration = 2f;

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
        bool timeIsOver = !scoreManagerInstance.timerIsRunning && scoreManagerInstance.timeRemaining <= 0;

        // Check if game ending conditions are met (only time being over)
        if (timeIsOver)
        {
            conditionsHaveBeenMet = true;
            int sceneIndexToLoad;

            if (currentScore > scoreThresholdForGoodEnding)
            {
                Debug.Log($"Time has run out! Score ({currentScore}) is > {scoreThresholdForGoodEnding}. Loading 'Good Ending' scene (index {goodEndingSceneIndex}).");
                sceneIndexToLoad = goodEndingSceneIndex;
            }
            else
            {
                Debug.Log($"Time has run out! Score ({currentScore}) is <= {scoreThresholdForGoodEnding}. Loading 'Bad Ending' scene (index {badEndingSceneIndex}).");
                sceneIndexToLoad = badEndingSceneIndex;
            }

            StartCoroutine(PerformFadeAndLoadScene(sceneIndexToLoad));
        }
    }

    IEnumerator PerformFadeAndLoadScene(int targetSceneIndex)
    {
        // Ensure Time.timeScale is 1 for the fade to work correctly,
        // in case it was set to 0 when the timer ran out by another script.
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            Debug.Log("GameEndManager: Time.timeScale was 0, resetting to 1 for fade.");
        }

        Debug.Log($"GameEndManager: Starting fade to black for scene index {targetSceneIndex}.");
        float elapsedTime = 0f;
        Color panelColor = fadePanel.color; // Get the base color (e.g., black)

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime; // Uses scaled time, which is now ensured to be 1.
            float newAlpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, newAlpha);
            yield return null; // Wait for the next frame
        }

        // Ensure it's fully opaque before changing scene
        fadePanel.color = new Color(panelColor.r, panelColor.g, panelColor.b, 1f);
        Debug.Log($"GameEndManager: Fade complete. Loading scene with build index {targetSceneIndex}.");

        SceneManager.LoadScene(targetSceneIndex);
    }
}
