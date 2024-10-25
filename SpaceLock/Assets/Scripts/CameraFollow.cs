using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float rotationSpeed = 2.5f;

    private float yaw = -90f; // Initialize to -90 degrees
    private float pitch = 0f;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned.");
            return;
        }

        // Set initial position
        transform.position = player.transform.position + offset;
        
        // Set initial rotation
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Update rotation based on mouse input
        yaw += rotationSpeed * Input.GetAxis("Mouse X");
        pitch -= rotationSpeed * Input.GetAxis("Mouse Y");

        // Clamp pitch to prevent camera flipping
        pitch = Mathf.Clamp(pitch, -65f, 65f);

        // Calculate new rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        Vector3 position = player.transform.position + offset;
        // Calculate new position
        //Vector3 negDistance = new Vector3(0.0f, 0.0f, -offset.magnitude);
        //Vector3 position = rotation * negDistance + player.transform.position;


        // Apply new position and rotation
        transform.rotation = rotation;
        transform.position = position;
    }
}

