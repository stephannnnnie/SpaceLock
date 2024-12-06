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
    //public GameObject progressBar;

    [SerializeField] private GameObject wallCollision;
    [SerializeField] private GameObject outofGrapples;
    [SerializeField] GameObject InfoTab;

//    [SerializeField] Image GrappleIncrease;
    [SerializeField] Image GrappleDistance;
    [SerializeField] GameObject barr;
    //[SerializeField] GameObject BarrInfo;

    private float StartTime;
    public float CompletionTime;
    private int Powerupss;
    private bool islose;

    const float MAX_GRAPPLES = 20f;
    const float MAX_DISTANCE = 200f;

    [SerializeField] private GameObject GrappleBarContainer; // Parent container for grapple segments
    [SerializeField] private List<Image> grappleSegments; // List to hold each segment image

    // Start is called before the first frame update
    void Start()
    {
        won.SetActive(false);
        lose.SetActive(false);
        wallCollision.SetActive(false);
        outofGrapples.SetActive(false);
        croshair.SetActive(true);
        InfoTab.SetActive(false);
        noofGrapples = 0;
        islose = true;
        barr.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //BarrInfo.SetActive(true);
        StartTime = Time.time;
        CompletionTime = 0;
        if (SceneManager.GetActiveScene().name == "Test_tut") { InfoTab.SetActive(true); }
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
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && CompletionTime >= 3f) { InfoTab.SetActive(false); }
        CompletionTime =  Time.time - StartTime;
        InforTab();
    }

    public void updateGrappless() { 
        noofGrapples++;
    }

    public void updatePowerup() {
        Powerupss++;
    }

    public void InforTab() {

        if (Input.GetKeyDown(KeyCode.Tab)) { 
            InfoTab.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Tab)) {
            InfoTab.SetActive(false);
        }

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
        //progressBar.SetActive(false);

        barr.SetActive(false);
    }

    public void UpdateGrappleNumber(float remainingGrapple , float distance) {
        // Debug.Log("Remaining grapples: " + remainingGrapple);
        for (int i = 0; i < grappleSegments.Count; i++)
        {
            bool shouldEnable = i + 1 <= remainingGrapple;
            grappleSegments[i].enabled = shouldEnable;

            //Debug.Log(grappleSegments[i].name + " " + i + " " + "shouldEnable: " + shouldEnable);
        }
        //GrappleIncrease.fillAmount = remainingGrapple / MAX_GRAPPLES;
        GrappleDistance.fillAmount = distance / MAX_DISTANCE;

    }

    public void resetTime() {
        StartTime = Time.time;
        CompletionTime = 0;
    }

}