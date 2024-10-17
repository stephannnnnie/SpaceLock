using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GaemOverDetection1 : MonoBehaviour {
    public GameObject player;
    public Canvas cv;

    void Update()
    {
        if (this.transform.position.x - player.transform.position.x   < 1f )
        {

      
            Debug.Log("Player's position.x is greater than 60.");
            cv.PlayerLose();
            Invoke("RestartGame", 2f);
        }
    }
    void RestartGame() {
        Debug.Log("RestartGame called.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
