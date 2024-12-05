using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;
    private float timeOffset;
    private float lastYOffset = 0f;

    void Start()
    {
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        // Calculate the new Y offset
        float newYOffset = amplitude * Mathf.Sin((Time.time + timeOffset) * frequency);

        Vector3 currentPos = transform.position;

        // Remove the last offset and add the new one
        float newY = currentPos.y - lastYOffset + newYOffset;

        transform.position = new Vector3(currentPos.x, newY, currentPos.z);

        // Store this offset for next frame
        lastYOffset = newYOffset;
    }
}