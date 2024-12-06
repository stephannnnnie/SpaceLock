using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { ExtraGrapple, IncreaseGrappleDistance }
    public PowerUpType powerUpType;
    private float triggerRadius = 10f; // Distance within which the power-up is activated
    private GameObject player; // Internal reference to the player
    public int GrapplesIncrese;
    public int GrapplesDistance;
    public ParticleSystem PowerUpEffect = null; // To trigger effect
    private bool isActivated = false;
    private FloatingTextAnimation numGrapplePowerUpText;
    private FloatingTextAnimation distancePowerUpText;


    void Start()
    {
        // Find the player GameObject by tag
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("Player with tag 'Player' not found. Make sure the player object is tagged correctly.");
        }

        if (powerUpType == PowerUpType.IncreaseGrappleDistance)
        {
            ChangeColor();
        }

        // Find the Text objects by tag
        GameObject numTextObj = GameObject.FindWithTag("NumGrappleText");
        GameObject distanceTextObj = GameObject.FindWithTag("DistanceText");

        // Get the FloatingTextAnimation component from each text object
        if (numTextObj != null)
            numGrapplePowerUpText = numTextObj.GetComponent<FloatingTextAnimation>();
            Debug.Log("NumGrappleText assigned successfully.");

        if (distanceTextObj != null)
            distancePowerUpText = distanceTextObj.GetComponent<FloatingTextAnimation>();
            Debug.Log("DistanceText assigned successfully.");
    }

    void Update()
    {
        if (player != null && !isActivated)
        {
            // Calculate the distance between the player and the PowerUp
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            // Check if the player is within the trigger radius
            if (distanceToPlayer <= triggerRadius)
            {
                ActivatePowerUp();
            }
        }
    }

    void ActivatePowerUp()
    {
        isActivated = true;
        if (PowerUpEffect != null)
        {
            // Detach the particle system from the power-up object
            PowerUpEffect.transform.parent = null;
            PowerUpEffect.Play();
        }

        // Get the Grapple component from the player
        var grappleScript = player.GetComponent<Grapple>();
        if (grappleScript != null)
        {
            ApplyPowerUp(grappleScript);
        }
        // Destroy the power-up object immediately
        Debug.Log("Destroying power-up object.");
        Destroy(gameObject);
    }

    IEnumerator DestroyAfterEffect()
    {
        if (PowerUpEffect != null)
        {
            // Wait until the particle system stops playing
            while (PowerUpEffect.isPlaying)
            {
                yield return null;
            }
        }

        // Destroy the particle system GameObject after the effect finishes
        Destroy(PowerUpEffect.gameObject);
    }

    void ApplyPowerUp(Grapple grappleScript)
    {
        switch (powerUpType)
        {
            case PowerUpType.ExtraGrapple:
                if (grappleScript.remainingGrapples >= 20)
                {
                    grappleScript.remainingGrapples = 20;
                }
                else {
                    grappleScript.remainingGrapples += GrapplesIncrese;
                }
                // Trigger the "+10 Grapples" floating text animation
                if (numGrapplePowerUpText != null)
                {
                    numGrapplePowerUpText.PlayFloatingText("+5");
                }
                grappleScript.UpdateGrappleCountText(); // Update the UI text
                //Debug.Log("Increased grapples by 10. New total: " + grappleScript.remainingGrapples);
                break;

            case PowerUpType.IncreaseGrappleDistance:
                ExpandCircle(grappleScript.maxGrappleDistance, grappleScript.maxGrappleDistance + 10f);
                // Trigger the "+10 Distance" floating text animation
                if (distancePowerUpText != null)
                {
                    distancePowerUpText.PlayFloatingText("+10");
                }
                grappleScript.maxGrappleDistance += 10f; // Increase by 10 (adjustable)
                //Debug.Log("Increased grapple distance by 10. New distance: " + grappleScript.maxGrappleDistance);
                break;
        }
        // Update UI to reflect changes after power-up
        player.GetComponent<Grapple>().cv.updatePowerup();
        grappleScript.UpdateGrappleCountText();
    }

    void ChangeColor()
    {
        // Get the MeshRenderer component of the current GameObject
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer != null)
        {
            // Parse the color from a hex string
            Color newColor;
            if (ColorUtility.TryParseHtmlString("#00EDFF", out newColor))
            {
                // Change the color of the material
                meshRenderer.material.color = newColor;
            }
            else
            {
                Debug.LogWarning("Failed to parse color.");
            }
        }
        else
        {
            Debug.LogWarning("MeshRenderer component not found.");
        }
    }

    public void ExpandCircle(float currentGrappleDistance, float newMaxGrappleDistance)
    {
        Transform circleTransform = player.transform.Find("GrappleDistanceCircle");

        if (circleTransform != null)
        {
            circleTransform.GetComponent<LineRenderer>().enabled = true;
            circleTransform.gameObject.GetComponent<GrappleRangeCircle>().AnimateCircleExpansion(currentGrappleDistance, newMaxGrappleDistance, 0.5f);
        }
        else
        {
            Debug.LogError("GrappleDistanceCircle child not found under the player.");
        }
    }
}
