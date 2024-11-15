using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallProximityFlicker : MonoBehaviour
{
    public Transform player;
    public Transform[] walls;
    public float proximityThreshold = 5f;
    public float flickerSpeed = 5f;
    public float maxAlpha = 0.5f;
    public Image brightnessOverlay;

    private Color originalColor;

    void Start()
    {
        originalColor = new Color(1f, 0f, 0f, maxAlpha);
        brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    void Update()
    {
        foreach (Transform wall in walls)
        {
            float distance = Vector3.Distance(player.position, wall.position);
            if (distance <= proximityThreshold)
            {
                StartFlickering();
                return;
            }
        }

        StopFlickering();
    }

    void StartFlickering()
    {
            float alpha = Mathf.Abs(Mathf.Sin(Time.time * flickerSpeed)) * maxAlpha;
            brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            
    }

    void StopFlickering()
    {
            brightnessOverlay.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }
}
