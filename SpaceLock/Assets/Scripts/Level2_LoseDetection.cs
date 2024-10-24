using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Level2_LoseDetection : MonoBehaviour
{
    public GameObject player;
    public Canvas cv;

    // Wall z positions
    private float wall1Z = 82.4f;
    private float wall2Z = -97.5f;

    // Define the distance threshold for losing near a wall
    public float loseDistanceThreshold = 0.5f;

    void Update()
    {
        // Check if player is near wall 1
        if (Mathf.Abs(player.transform.position.z - wall1Z) < loseDistanceThreshold)
        {
            Debug.Log("Player is near Wall 1.");
            cv.PlayerLose();
        }
        // Check if player is near wall 2
        else if (Mathf.Abs(player.transform.position.z - wall2Z) < loseDistanceThreshold)
        {
            Debug.Log("Player is near Wall 2.");
            cv.PlayerLose();
        }
    }
}
