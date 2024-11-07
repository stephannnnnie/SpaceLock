using UnityEngine;
using System.Collections;

public class GrappleRangeCircle : MonoBehaviour
{
    public int segments = 100;  // Number of segments for the circle
    public float radius = 5f;   // Initial radius of the circle
    public bool isTutorial;

    private LineRenderer lineRenderer;
    private Transform playerTransform;  // Reference to the player's transform
    private Vector3 initialOffset;  // Initial offset between the camera and the circle
    private Grapple playerGrapple;  // Reference to the Grapple script

    private string currentColor;
    private Color greenColor;
    private Color redColor;


    void Start()
    {
        if (!isTutorial)
        {
            return;
        }

        ColorUtility.TryParseHtmlString("#56B23F", out greenColor);
        ColorUtility.TryParseHtmlString("#B23F42", out redColor);

        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found. Ensure the GameObject has a LineRenderer attached.");
            return;
        }

        // Find the player using the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerGrapple = player.GetComponent<Grapple>();

            if (playerGrapple == null)
            {
                Debug.LogError("Grapple script not found on the player.");
                return;
            }

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

        DrawCircle(playerGrapple.maxGrappleDistance);
        lineRenderer.enabled = false;
    }

    
    void LateUpdate()
    {
        // Maintain the initial offset relative to the player's position
        if (isTutorial && playerTransform != null)
        {
            transform.position = playerTransform.position + initialOffset;
        }
    }

    public void AnimateCircleExpansion(float currentRadius, float newRadius, float duration)
    {
        if (isTutorial && lineRenderer != null)
        {
            StartCoroutine(StartExpand(currentRadius, newRadius, duration));
        }
    }

    private IEnumerator StartExpand(float currentRadius, float targetRadius, float duration)
    {
        // Set the color to #56B23F (green)
        if (lineRenderer != null)
        {
            DrawCircle(currentRadius);

            lineRenderer.startColor = greenColor;
            lineRenderer.endColor = greenColor;
            SetTransparency(0.7f);

            currentColor = "green";
        }

        // Wait for 0.5 seconds before starting the expansion
        yield return new WaitForSeconds(0.5f);

        //float currentRadius = playerGrapple.maxGrappleDistance;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float radius = Mathf.Lerp(currentRadius, targetRadius, elapsedTime / duration);
            DrawCircle(radius);
            elapsedTime += Time.deltaTime;
            yield return null;  // Wait for the next frame
        }

        // Ensure the final radius is set
        DrawCircle(targetRadius);

        // Wait for an additional 0.5 seconds after the animation completes
        yield return new WaitForSeconds(1.0f);

        // Disable the GameObject after the delay
        Debug.Log("Disabling GrappleDistanceCircle");
        lineRenderer.enabled = false;
    }

    public void ShowRedCircle(float currentRadius)
    {
        if (isTutorial && lineRenderer != null)
        {
            DrawCircle(currentRadius);

            lineRenderer.startColor = redColor;
            lineRenderer.endColor = redColor;
            SetTransparency(0.7f);

            currentColor = "red";
        }
    }

    public void HideRedCircle()
    {
        if (isTutorial && lineRenderer.enabled && currentColor == "red")
        {
            lineRenderer.enabled = false;
        }
    }

    /*
    public void ShowRedCircle(float currentRadius, float duration)
    {
        if (lineRenderer != null)
        {
            StartCoroutine(RedCircleCoroutine(currentRadius, duration));
        }
        else
        {
            Debug.LogError("LineRenderer is not initialized.");
        }
    }

    private IEnumerator RedCircleCoroutine(float currentRadius, float duration)
    {
        // Set the color to #B23F42 (red)
        if (lineRenderer != null)
        {
            DrawCircle(currentRadius);

            Color redColor;
            if (ColorUtility.TryParseHtmlString("#B23F42", out redColor))
            {
                lineRenderer.startColor = redColor;
                lineRenderer.endColor = redColor;
                SetTransparency(0.7f);
            }
            else
            {
                Debug.LogError("Failed to parse the color #B23F42.");
            }
        }

        yield return new WaitForSeconds(duration);

        // Disable the GameObject after the delay
        Debug.Log("Disabling RedCircle");
        lineRenderer.enabled = false;
    }
    */

    private void SetTransparency(float newAlpha)
    {
        // Get the current start and end colors
        Color startColor = lineRenderer.startColor;
        Color endColor = lineRenderer.endColor;

        // Apply the alpha value to the colors
        startColor.a = newAlpha;
        endColor.a = newAlpha;

        // Set the new colors back to the LineRenderer
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }

    void DrawCircle(float radius)
    {
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer is not initialized.");
            return;
        }

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
