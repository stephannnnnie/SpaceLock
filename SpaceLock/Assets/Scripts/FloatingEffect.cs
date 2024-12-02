using UnityEngine;

public class FloatingEffect : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float amplitude = 0.5f;    // How far up and down the object moves
    [SerializeField] private float frequency = 1f;      // How fast the object moves up and down

    private Vector3 startPosition;
    private float timeOffset;

    void Start()
    {
        // Store the starting position of the object
        startPosition = transform.position;

        // Add a random offset to make multiple objects float out of sync
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPosition.y + amplitude * Mathf.Sin((Time.time + timeOffset) * frequency);

        // Update the object's position
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}