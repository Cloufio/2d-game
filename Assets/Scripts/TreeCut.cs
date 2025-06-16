using UnityEngine;

public class TreeCut : Tool // Make sure TreeCut still inherits from Tool
{
    [Header("Tree Stats")]
    [SerializeField] int treeHealth = 25; // The tree's starting health
    [SerializeField] int damagePerHit = 10; // How much damage each hit does

    [Header("Scoring")]
    [SerializeField] int pointsForCutting = 1; // How many points this tree gives when cut

    // This 'Hit' method is called by the ToolController.
    // It now handles health reduction before destroying the object.
    public override void Hit()
    {
        // --- 1. Reduce Health ---
        treeHealth -= damagePerHit;
        Debug.Log(gameObject.name + " was hit! Remaining health: " + treeHealth);

        // --- 2. Check if Health is Depleted ---
        // The rest of the code only runs if the tree's health is 0 or less.
        if (treeHealth <= 0)
        {
            Debug.Log(gameObject.name + " has been cut down! Adding score and removing object.");

            // --- 3. Add Score ---
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.AddScore(pointsForCutting);
            }
            else
            {
                Debug.LogError("ScoreManager.Instance is not found in the scene! Cannot add score.");
            }

            // --- 4. Destroy the Tree GameObject ---
            // This now only happens when the tree runs out of health.
            Destroy(gameObject);
        }
    }
}