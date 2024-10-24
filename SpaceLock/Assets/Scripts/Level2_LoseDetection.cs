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
            TriggerLoss();
        }
        // Check if player is near wall 2
        else if (Mathf.Abs(player.transform.position.z - wall2Z) < loseDistanceThreshold)
        {
            Debug.Log("Player is near Wall 2.");
            TriggerLoss();
        }
    }

    // Method to trigger player loss and restart the game
    private void TriggerLoss()
    {
        cv.PlayerLose();
        StartCoroutine(RestartGameAfterDelay(2f));  // Restart after 2 seconds
        Time.timeScale = 0f;  // Pauses the game to prevent further input
    }

    // Coroutine to handle game restart after a delay
    IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);  // Wait in real time, unaffected by Time.timeScale
        Debug.Log("RestartGame called.");
        Time.timeScale = 1f;  // Reset time scale to normal
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the scene
    }
}
