using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float rotationSpeed = 1.0f; // Lower sensitivity for smoother movement
    public float mouseSensitivity = 0.4f; // Additional sensitivity control
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

        // Update rotation based on mouse input with additional sensitivity control
        yaw += rotationSpeed * Input.GetAxis("Mouse X") * mouseSensitivity;
        pitch -= rotationSpeed * Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Clamp pitch to prevent camera flipping
        if (SceneManager.GetActiveScene().name == "3D MainMenu")
        {
            yaw = Mathf.Clamp(yaw, -140f, -45f);
        }
        pitch = Mathf.Clamp(pitch, -60f, 60f);

        // Calculate new rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Calculate new position
        Vector3 position = player.transform.position + offset;

        // Apply new position and rotation
        transform.rotation = rotation;
        transform.position = position;
    }
}

