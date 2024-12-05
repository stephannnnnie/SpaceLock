using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameOverDetection : MonoBehaviour {
    public GameObject player;
    public Canvas cv;

    public GameObject NotInRange;
    public GameObject InRange;

    // public Button restartButton;
    void Start() {
        if (player.GetComponent<Collider>() == null || this.GetComponent<Collider>() == null)
        {
            Debug.LogError("Player, Cube3, and backWall must all have colliders.");
        }

        if (player.GetComponent<Rigidbody>() == null && this.GetComponent<Rigidbody>() == null)
        {
            Debug.LogError("At least one of the objects must have a Rigidbody.");
        }

        player.GetComponent<Collider>().isTrigger = true;
        this.GetComponent<Collider>().isTrigger = true;


    }

    void Update() {

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < player.GetComponent<Grapple>().maxGrappleDistance)
        {
            NotInRange.SetActive(false);
            InRange.SetActive(true);
        }
        else
        {
           NotInRange.SetActive(true);
           InRange.SetActive(false);
        }

    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player || other.gameObject == this)
        {
            Debug.Log("Trigger detected between player and wall.");
            cv.PlayerWon();

        }
    }

    void RestartGame() {
        Debug.Log("RestartGame called.");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
