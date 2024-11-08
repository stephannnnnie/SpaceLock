using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3_ObstacleSpeed : MonoBehaviour
{
    public float minSpeed = 1.5f;
    private float speed;
    private GameObject player;
    private bool isMovingBackward = false; // Flag to indicate backward movement

    void Start()
    {
        // Find the player GameObject by tag
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player tag not found! Make sure your player object has the correct tag.");
            return;
        }

        // Calculate the speed based on obstacle scale
        Vector3 scale = transform.localScale;
        float scaleFactor = (scale.x + scale.y + scale.z) / 3f;
        speed = minSpeed * scaleFactor;
    }

    void Update()
    {
        // Check if the player exists
        if (player != null && !isMovingBackward)
        {
            // Calculate direction towards the player
            Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

            // Move the obstacle towards the player
            transform.Translate(directionToPlayer * speed * Time.deltaTime, Space.World);
        }
        else if (isMovingBackward)
        {
            // Move the obstacle backward along the x-axis
            transform.Translate(-speed * Time.deltaTime * Vector3.right, Space.World);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the obstacle collides with the player
        if (collision.gameObject == player)
        {
            // Set the flag to start moving backward
            isMovingBackward = true;

            // Parent the player to the obstacle so they move together
            player.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // Reset the backward movement flag when the obstacle stops colliding with the player
        if (collision.gameObject == player)
        {
            isMovingBackward = false;

            // Unparent the player so it can move independently again
            player.transform.SetParent(null);
        }
    }
}
