using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GaemOverDetection1 : MonoBehaviour {
    public GameObject player;
    public Canvas cv;

    private void Start()
    {
        // Initialization, if needed
    }

    void Update()
    {
        if (this.transform.position.x - player.transform.position.x < 0.5f)
        {
            Debug.Log("Player's position.x is greater than 60.");
            cv.PlayerLose();
            StartCoroutine(RestartGameAfterDelay(2f));  // Restart after 2 seconds
            Time.timeScale = 0f;  // Pauses the game to prevent further input
        }
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
