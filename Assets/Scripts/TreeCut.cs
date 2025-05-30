using UnityEngine;

public class TreeCut : Tool // Make sure TreeCut still inherits from Tool
{
    [Header("Scoring")] // Keeps the Inspector organized
    [SerializeField] int pointsForCutting = 1; // How many points this tree gives when cut

    // This 'Hit' method is called when the ToolController successfully "hits" this tree.
    // It overrides the Hit() method from the base 'Tool' class.
    public override void Hit()
    {
        Debug.Log(gameObject.name + " was hit! Adding score and removing object.");

        // --- 1. Add Score ---
        // This assumes you have the ScoreManager set up as we discussed before.
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(pointsForCutting);
        }
        else
        {
            // This message is helpful if you forget to put the ScoreManager in your scene
            Debug.LogError("ScoreManager.Instance is not found in the scene! Cannot add score. Make sure ScoreManagerObject is in your scene.");
        }

        // --- 2. Destroy the Tree GameObject ---
        // This happens after scoring.
        Destroy(gameObject);
    }
}