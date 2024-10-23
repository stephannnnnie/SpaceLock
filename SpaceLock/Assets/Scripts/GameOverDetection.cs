using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverDetection : MonoBehaviour
{
    public GameObject player;      // Reference to the player object
    public GameObject winWall;     // Reference to the win wall object
    public GameObject loseWall;    // Reference to the lose wall object
    public Canvas cv;              // Reference to the canvas (UI) object
    private bool gameEnded = false; // Flag to track if the game has ended

    void Start()
    {
        // Ensure the player and winWall have colliders
        if (player.GetComponent<Collider>() == null || winWall.GetComponent<Collider>() == null)
        {
            Debug.LogError("Player and WinWall must have colliders.");
        }

        if (player.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("Player must have a Rigidbody.");
        }

        // Ensure the winWall is set to trigger
        winWall.GetComponent<Collider>().isTrigger = true;
    }

    void Update()
    {
        // Lose condition based on position relative to loseWall
        if (!gameEnded && loseWall.transform.position.x - player.transform.position.x < 0.5f)
        {
            Debug.Log("Player's position.x is greater than loseWall's.");
            gameEnded = true;  // Mark the game as ended
            cv.PlayerLose();   // Call the lose function
            Invoke("RestartGame", 2f);  // Restart the game after 2 seconds
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Win condition based on collision with winWall
        if (!gameEnded && other.gameObject == player && other.gameObject == winWall)
        {
            Debug.Log("Trigger detected between player and WinWall.");
            gameEnded = true;  // Mark the game as ended
            cv.PlayerWon();    // Call the win function
            Invoke("RestartGame", 2f);  // Restart the game after 2 seconds
        }
    }

    void RestartGame()
    {
        Debug.Log("RestartGame called.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the current scene
    }
}
