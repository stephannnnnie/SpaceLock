using System;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFlickerController : MonoBehaviour
{
    public Image brightnessOverlay;
    public Transform player;
    public Transform backWall;
    public float dangerDistance = 100f;
    public float maxAlpha = 0.3f;
    public float flickerSpeed = 5f;
    private bool flickeringEnabled = false;

    private Color originalColor;

    void Start()
    {
        originalColor = new Color(1f, 0f, 0f, maxAlpha); // Red color with max alpha
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0); // Start as transparent
    }

    void Update()
    {
        if (!flickeringEnabled)
        {
            brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
            return;
        }


        float distanceToBackWall = Vector3.Distance(player.position, backWall.position);


        if (flickeringEnabled && distanceToBackWall <= dangerDistance)
        {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed)) * maxAlpha;
            brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        }
        else
        {
            brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        }
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
