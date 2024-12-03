using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;

    private float startY;
    private float timeOffset;

    void Start()
    {
        // Only store the starting Y position
        startY = transform.position.y;
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        // Calculate the new Y position
        float newY = startY + amplitude * Mathf.Sin((Time.time + timeOffset) * frequency);

        // Get current X and Z from the transform
        Vector3 currentPos = transform.position;

        // Only update the Y position, keeping current X and Z
        transform.position = new Vector3(currentPos.x, newY, currentPos.z);
    }
}