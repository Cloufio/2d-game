using UnityEngine;

public class TreeSway : MonoBehaviour
{
    public float swayAmount = 2f;
    public float swaySpeed = 1f;

    private float offset;

    void Start()
    {
        offset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float zRotation = Mathf.Sin((Time.time + offset) * swaySpeed) * swayAmount;
        transform.rotation = Quaternion.Euler(0f, 0f, zRotation);
    }
}