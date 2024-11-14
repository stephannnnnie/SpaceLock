using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Canvas : MonoBehaviour
{

    public GameObject won;
    public GameObject lose;
    public GameObject croshair;
    public int noofGrapples = 0;
    public SendToGoogle se;
    public GameObject progressBar;

    [SerializeField] private GameObject wallCollision;
    [SerializeField] private GameObject outofGrapples;

    [SerializeField] Image GrappleIncrease;
    [SerializeField] Image GrappleDistance;
    [SerializeField] GameObject barr;
    [SerializeField] GameObject BarrInfo;

    private float StartTime;
    public float CompletionTime;
    private int Powerupss;
    private bool islose;

    const float MAX_GRAPPLES = 20f;
    const float MAX_DISTANCE = 200f;



    // Start is called before the first frame update
    void Start()
    {
        won.SetActive(false);
        lose.SetActive(false);
        wallCollision.SetActive(false);
        outofGrapples.SetActive(false);
        croshair.SetActive(true);
        noofGrapples = 0;
        islose = true;
        barr.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        BarrInfo.SetActive(true);
        StartTime = Time.time;
        CompletionTime = 0;
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && CompletionTime >= 3f && BarrInfo != null) { BarrInfo.SetActive(false); }
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
        if (CompletionTime >= 13f) { se.Send(CompletionTime, noofGrapples, SceneManager.GetActiveScene().name, "WON", Powerupss); }
        Debug.Log(CompletionTime);
    }

    public void PlayerLose(int reasonCode) {
        
        Reset();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        lose.SetActive(true);

        if (reasonCode == 1)
        {
            wallCollision.SetActive(true);
        }
        else if (reasonCode == 2)
        {
            outofGrapples.SetActive(true);
        }
        if (islose && CompletionTime >= 5f ) {
            se.Send(CompletionTime, noofGrapples, SceneManager.GetActiveScene().name, "Lose", Powerupss);
            islose = false;
            Debug.Log("Time Completedddddd");
        }
    }


    public void Reset()
    {
        won.SetActive(false);
        lose.SetActive(false);

        wallCollision.SetActive(false);
        outofGrapples.SetActive(false);

        croshair.SetActive(false);
        progressBar.SetActive(false);

        barr.SetActive(false);
    }

    public void UpdateGrappleNumber(float remainingGrapple , float distance) {

        GrappleIncrease.fillAmount = remainingGrapple / MAX_GRAPPLES;
        GrappleDistance.fillAmount = distance / MAX_DISTANCE;

    }

    public void resetTime() {
        StartTime = Time.time;
        CompletionTime = 0;
    }

}
