using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType { ExtraGrapple, IncreaseGrappleDistance }
    public PowerUpType powerUpType;
    private float triggerRadius = 10f; // Distance within which the power-up is activated
    private GameObject player; // Internal reference to the player

    void Start()
    {
        // Find the player GameObject by tag
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player with tag 'Player' not found. Make sure the player object is tagged correctly.");
        }
    }

    void Update()
    {
        if (player != null)
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
        // Get the Grapple component from the player
        var grappleScript = player.GetComponent<Grapple>();
        if (grappleScript != null)
        {
            ApplyPowerUp(grappleScript);
        }

        Destroy(gameObject); // Destroy the PowerUp after activation
    }

    void ApplyPowerUp(Grapple grappleScript)
    {
        switch (powerUpType)
        {
            case PowerUpType.ExtraGrapple:
                grappleScript.remainingGrapples += 10;
                Debug.Log("Increased grapples by 10. New total: " + grappleScript.remainingGrapples);
                break;

            case PowerUpType.IncreaseGrappleDistance:
                grappleScript.maxGrappleDistance += 10f; // Increase by 10 (adjustable)
                Debug.Log("Increased grapple distance by 10. New distance: " + grappleScript.maxGrappleDistance);
                break;
        }
    }
}
