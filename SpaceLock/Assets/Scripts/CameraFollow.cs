using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float rotationSpeed = 1f;
    public float smoothSpeed = 0.125f;
    public float rotationSmoothSpeed = 0.1f;

    private float yaw = -90f;
    private float pitch = 0f;
    private Vector3 currentVelocity = Vector3.zero;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned.");
            return;
        }

        transform.position = player.transform.position + offset;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0);
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Update rotation based on mouse input
        yaw += rotationSpeed * Input.GetAxis("Mouse X");
        pitch -= rotationSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -65f, 65f);

        // Calculate target rotation
        Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);

        // Smoothly interpolate rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed);

        // Calculate target position
        Vector3 targetPosition = player.transform.position + offset;

        // Smoothly move the camera towards the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVelocity, smoothSpeed);
    }
}

