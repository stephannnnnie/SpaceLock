using UnityEngine;
using UnityEngine.UI;

public class ScreenFlickerController : MonoBehaviour
{
    public Image brightnessOverlay;
    public float maxAlpha = 0.3f; // Maximum alpha for flickering effect
    public float flickerSpeed = 2f; // Speed of flicker effect
    private bool flickeringEnabled = false;

    private float closestDangerDistance = float.MaxValue; // Tracks the closest danger distance

    private Color originalColor;

    void Start()
    {
        originalColor = new Color(1f, 0f, 0f, maxAlpha); // Red color with max alpha
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0); // Start as transparent
    }

    void Update()
    {
        if (!flickeringEnabled || closestDangerDistance == float.MaxValue)
        {
            brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            return;
        }

        float alpha = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed)) * maxAlpha;
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
    }

    public void NotifyDangerProximity(float distance)
    {
        closestDangerDistance = Mathf.Min(closestDangerDistance, distance);
        flickeringEnabled = true; // Start flickering
    }

    public void NotifyNoDanger()
    {
        closestDangerDistance = float.MaxValue;
        flickeringEnabled = false;
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    public void TriggerFirstGrapple()
    {
        flickeringEnabled = true;
    }

    public void StopFlickering()
    {
        flickeringEnabled = false;
        closestDangerDistance = float.MaxValue;
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}
