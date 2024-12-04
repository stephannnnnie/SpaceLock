using UnityEngine;

public class WallProximityFlicker : MonoBehaviour
{
    public float dangerDistance = 35f;
    public float loseDistanceThreshold = 0.5f;
    public ScreenFlickerController flickerController;
    public Transform player;
    private bool isPlayerInDanger = false;
    public Canvas cv;
    public bool loseTriggered = false;
    private Renderer wallRenderer;

    void Start()
    {
        wallRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (player == null || flickerController == null || cv == null || wallRenderer == null) return;

        Vector3 wallNormal = transform.forward;
        float perpendicularDistance = Mathf.Abs(player.position.z - transform.position.z);

        bool isWithinWallBounds = IsPlayerWithinWallBounds();

        Debug.Log("Perpendicular distance: " + perpendicularDistance);
        if (isWithinWallBounds && perpendicularDistance <= dangerDistance)
        {
            if (!isPlayerInDanger)
            {
                flickerController.NotifyDangerProximity(perpendicularDistance);
                isPlayerInDanger = true;
            }
        }
        else
        {
            if (isPlayerInDanger)
            {
                flickerController.NotifyNoDanger();
                isPlayerInDanger = false;
            }
        }

        if (isWithinWallBounds && !loseTriggered && perpendicularDistance <= loseDistanceThreshold)
        {
            TriggerLose();
        }
    }

    private bool IsPlayerWithinWallBounds()
    {
        Bounds wallBounds = wallRenderer.bounds;

        bool withinX = player.position.x >= wallBounds.min.x && player.position.x <= wallBounds.max.x;
        bool withinY = player.position.y >= wallBounds.min.y && player.position.y <= wallBounds.max.y;

        return withinX && withinY;
    }

    private void TriggerLose()
    {
        loseTriggered = true;

        Debug.Log("Touched the wall.");
        flickerController.StopFlickering();
        cv.PlayerLose(1);
    }
}
