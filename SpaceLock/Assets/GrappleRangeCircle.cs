using UnityEngine;

public class GrappleRangeCircle : MonoBehaviour
{
    public int segments = 100;  // Number of segments for the circle
    public float radius = 5f;   // Initial radius of the circle
    //public Color circleColor = new Color(0x56 / 255f, 0xB3 / 255f, 0x3F / 255f, 1.0f);

    private LineRenderer lineRenderer;
    private Transform playerTransform;  // Reference to the player's transform
    private Vector3 initialOffset;  // Initial offset between the camera and the circle
    private Grapple playerGrapple;  // Reference to the Grapple script

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Find the player using the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerGrapple = player.GetComponent<Grapple>();

            // Calculate the initial offset between the circle and the player
            initialOffset = transform.position - playerTransform.position;
        }
        else
        {
            Debug.LogError("Player GameObject not found. Ensure your player has the 'Player' tag.");
        }


        lineRenderer.positionCount = segments + 1;
        lineRenderer.loop = true;
        lineRenderer.startWidth = 1.0f;
        lineRenderer.endWidth = 1.0f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        //lineRenderer.startColor = circleColor;
        //lineRenderer.endColor = circleColor;

        DrawCircle(playerGrapple.maxGrappleDistance);
    }

    
    void LateUpdate()
    {
        // Maintain the initial offset relative to the player's position
        if (playerTransform != null)
        {
            transform.position = playerTransform.position + initialOffset;

            // Update the circle size based on the current max grapple distance
            if (playerGrapple != null)
            {
                DrawCircle(playerGrapple.maxGrappleDistance);
            }
        }
    }


    void DrawCircle(float radius)
    {
        float angleStep = 360f / segments;
        for (int i = 0; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;  // For 2D, use x and y

            Vector3 position = new Vector3(x, y, 0);  // Aligns with the camera's plane
            lineRenderer.SetPosition(i, position);
        }
    }
}
