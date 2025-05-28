using UnityEngine;

public class TreeCut : Tool
{
    [SerializeField] GameObject drop;
    [SerializeField] int dropCount = 15;
    [SerializeField] float spread = 2f;

    public override void Hit()
    {
        //

        Destroy(gameObject);
    }
}
