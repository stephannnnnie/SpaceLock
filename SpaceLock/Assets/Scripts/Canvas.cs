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
    public GameObject tutorial;
    private bool tutorialVisible;
    public int noofGrapples = 0;
    public SendToGoogle se;
    private float StartTime;
    private float CompletionTime;
    private int Powerupss;
    private bool islose;


    // Start is called before the first frame update
    void Start()
    {
        won.SetActive(false);
        lose.SetActive(false);
        croshair.SetActive(true);
        GrapplesNumber.SetActive(false);
        tutorialVisible = true;
        // StartCoroutine(ClearTutorial());
        noofGrapples = 0;
        islose = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialVisible && Input.anyKey) {
            tutorial.SetActive(false);
            GrapplesNumber.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("set cursor invisible");
            tutorialVisible = false;
            StartTime = Time.time;
        }
        CompletionTime =  Time.time - StartTime;
    }

    /*
    IEnumerator ClearTutorial(){
        tutorial.SetActive(true);
        yield return new WaitForSeconds(20);
        tutorial.SetActive(false);
    }
    */

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

    public void PlayerLose() {
        
        Reset();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        se.Send(CompletionTime, noofGrapples, SceneManager.GetActiveScene().name, "Lose", Powerupss);
        lose.SetActive(true);
        if (islose) {
            
            islose = false;
        }
    }


    public void Reset()
    {
        won.SetActive(false);
        lose.SetActive(false);
        croshair.SetActive(false);
        GrapplesNumber.SetActive(false);
    }
    
}
