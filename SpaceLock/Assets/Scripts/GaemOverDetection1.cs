using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GaemOverDetection1 : MonoBehaviour {
    public GameObject player;
    public Canvas cv;
    public GameObject winWall;  // Add a reference to the winWall GameObject
    public ScreenFlickerController screenFlickerController;
    void Update()
    {
        if (this.transform.position.x - player.transform.position.x < 0.5f)
        {
            Debug.Log("Player's position.x is greater than 60.");
            screenFlickerController.StopFlickering();
            cv.PlayerLose(1);
            DisableWinWallCollider(); 
            // Disable winWall's BoxCollider when the player loses
            //StartCoroutine(RestartGameAfterDelay(2f));  // Restart after 2 seconds
            //Time.timeScale = 0f;  // Pauses the game to prevent further input
        }
    }

    /**
        // Coroutine to handle game restart after a delay
        IEnumerator RestartGameAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);  // Wait in real time, unaffected by Time.timeScale
            Debug.Log("RestartGame called.");
            Time.timeScale = 1f;  // Reset time scale to normal
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the scene
        }
        */

    // Method to disable the BoxCollider of the winWall
    private void DisableWinWallCollider()
    {
        if (winWall != null)
        {
            BoxCollider winWallCollider = winWall.GetComponent<BoxCollider>();
            if (winWallCollider != null)
            {
                winWallCollider.enabled = false;  // Disable the BoxCollider
                Debug.Log("WinWall's BoxCollider disabled.");
            }
            else
            {
                Debug.LogWarning("WinWall does not have a BoxCollider component.");
            }
        }
        else
        {
            Debug.LogWarning("WinWall reference is missing.");
        }
    }
}
