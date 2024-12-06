using UnityEngine;
using UnityEngine.UI;

public class ScreenFlickerController : MonoBehaviour
{
    [Header("Flicker Settings")]
    public Image brightnessOverlay;
    public float maxAlpha = 0.3f;
    public float flickerSpeed = 3f;

    [Header("Wall Proximity Settings")]
    public Transform player;
    public GameObject[] walls;
    public float dangerDistance = 35f;
    public float loseDistanceThreshold = 0.5f;
    public Canvas cv;

    private bool flickeringEnabled = false;
    private Color originalColor;

    void Start()
    {
        originalColor = new Color(1f, 0f, 0f, maxAlpha);
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        if (walls == null || walls.Length == 0)
        {
            Debug.LogWarning("No walls assigned.");
            return;
        }
    }

    void Update()
    {
        HandleFlickerEffect(); // Handles flickering visuals
        HandleWallProximity(); // Handles danger checks and lose conditions
    }

    private void HandleFlickerEffect()
    {
        if (!flickeringEnabled)
        {
            brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            return;
        }

        float alpha = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed)) * maxAlpha;
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }

    private void HandleWallProximity()
    {
        if (player == null || walls == null) return;

        bool playerInDanger = false;

        for (int i = 0; i < walls.Length; i++)
        {
            GameObject wall = walls[i];
            if (wall == null) continue;

            Renderer wallRenderer = wall.GetComponent<Renderer>();
            if (wallRenderer == null) continue;

            (bool isWithinBounds, float perpendicularDistance) = CheckWallProximity(player.position, wallRenderer, wall.transform);

            if (isWithinBounds && perpendicularDistance <= dangerDistance)
            {
                playerInDanger = true;
            }

            if (isWithinBounds && perpendicularDistance <= loseDistanceThreshold)
            {
                TriggerLose(i);
            }
        }

        if (!playerInDanger && flickeringEnabled)
        {
            brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }
    }

    private (bool, float) CheckWallProximity(Vector3 playerPosition, Renderer wallRenderer, Transform wallTransform)
    {
        Vector3 wallRotation = wallTransform.eulerAngles;

        Bounds wallBounds = wallRenderer.bounds;

        if (Mathf.Approximately(wallRotation.x, 90f) && Mathf.Approximately(wallRotation.y, 90f) && Mathf.Approximately(wallRotation.z, 0f))
        {
            bool isWithinBounds = IsPlayerWithinXZBounds(playerPosition, wallBounds);
            float perpendicularDistance = Mathf.Abs(playerPosition.y - wallTransform.position.y);
            return (isWithinBounds, perpendicularDistance);
        }
        else if (Mathf.Approximately(wallRotation.x, 0f) && Mathf.Approximately(wallRotation.y, 90f) && Mathf.Approximately(wallRotation.z, 90f))
        {
            bool isWithinBounds = IsPlayerWithinYZBounds(playerPosition, wallBounds);
            float perpendicularDistance = Mathf.Abs(playerPosition.x - wallTransform.position.x);
            return (isWithinBounds, perpendicularDistance);
        }
        else if (Mathf.Approximately(wallRotation.x, 0f) && Mathf.Approximately(wallRotation.y, 0f) && Mathf.Approximately(wallRotation.z, 0f))
        {
            bool isWithinBounds = IsPlayerWithinXYBounds(playerPosition, wallBounds);
            float perpendicularDistance = Mathf.Abs(playerPosition.z - wallTransform.position.z);
            return (isWithinBounds, perpendicularDistance);
        }

        return (false, float.MaxValue);
    }

    private bool IsPlayerWithinXYBounds(Vector3 playerPosition, Bounds wallBounds)
    {
        float extendedMinY = wallBounds.min.y - 5f;
        float extendedMaxY = wallBounds.max.y + 5f;

        bool withinX = playerPosition.x >= wallBounds.min.x && playerPosition.x <= wallBounds.max.x;
        bool withinY = playerPosition.y >= extendedMinY && playerPosition.y <= extendedMaxY;

        return withinX && withinY;
    }

    private bool IsPlayerWithinXZBounds(Vector3 playerPosition, Bounds wallBounds)
    {
        bool withinX = playerPosition.x >= wallBounds.min.x && playerPosition.x <= wallBounds.max.x;
        bool withinZ = playerPosition.z >= wallBounds.min.z && playerPosition.z <= wallBounds.max.z;

        return withinX && withinZ;
    }

    private bool IsPlayerWithinYZBounds(Vector3 playerPosition, Bounds wallBounds)
    {
        bool withinY = playerPosition.y >= wallBounds.min.y && playerPosition.y <= wallBounds.max.y;
        bool withinZ = playerPosition.z >= wallBounds.min.z && playerPosition.z <= wallBounds.max.z;

        return withinY && withinZ;
    }


    private void TriggerLose(int wallIndex)
    {
        Debug.Log($"Player touched wall {wallIndex}.");
        StopFlickering();

        Debug.Log("Game Over!");
        cv.PlayerLose(1); // Game-over logic
    }

    public void TriggerFirstGrapple()
    {
        flickeringEnabled = true;
    }

    public void StopFlickering()
    {
        flickeringEnabled = false;
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}
