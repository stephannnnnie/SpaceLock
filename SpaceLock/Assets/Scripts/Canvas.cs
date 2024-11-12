using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class Canvas : MonoBehaviour
{

    public GameObject won;
    public GameObject lose;
    public GameObject croshair;
    public GameObject GrapplesNumber;
    public int noofGrapples = 0;
    public SendToGoogle se;
    private float StartTime;
    private float CompletionTime;
    private int Powerupss;
    private bool islose;
    [SerializeField] private GameObject wallCollision;
    [SerializeField] private GameObject outofGrapples;
    public GameObject progressBar;


    // Start is called before the first frame update
    void Start()
    {
        won.SetActive(false);
        lose.SetActive(false);
        wallCollision.SetActive(false);
        outofGrapples.SetActive(false);
        croshair.SetActive(true);
        GrapplesNumber.SetActive(true);
        noofGrapples = 0;
        islose = true;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        StartTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        // if (tutorialVisible && Input.anyKey) {
        //     tutorial.SetActive(false);
        //     GrapplesNumber.SetActive(true);
        //     Cursor.visible = false;
        //     Cursor.lockState = CursorLockMode.Locked;
        //     Debug.Log("set cursor invisible");
        //     StartTime = Time.time;
        // }
        CompletionTime =  Time.time - StartTime;
    }

    public void updateGrappless() { 
        noofGrapples++;
    }

    public void updatePowerup() {
        Powerupss++;
    }

    public void PlayerWon() {

        Reset();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        won.SetActive(true);
        se.Send(CompletionTime ,noofGrapples ,SceneManager.GetActiveScene().name, "WON" , Powerupss );
        Debug.Log(CompletionTime);
    }

    public void PlayerLose(int reasonCode) {
        
        Reset();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        se.Send(CompletionTime, noofGrapples, SceneManager.GetActiveScene().name, "Lose", Powerupss);
        lose.SetActive(true);

        if (reasonCode == 1)
        {
            wallCollision.SetActive(true);
        }
        else if (reasonCode == 2)
        {
            outofGrapples.SetActive(true);
        }

        if (islose) {
            
            islose = false;
        }
    }


    public void Reset()
    {
        won.SetActive(false);
        lose.SetActive(false);

        wallCollision.SetActive(false);
        outofGrapples.SetActive(false);

        croshair.SetActive(false);
        GrapplesNumber.SetActive(false);
        progressBar.SetActive(false);
    }

}
