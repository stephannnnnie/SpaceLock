using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{

    public GameObject won;
    public GameObject lose;
    public GameObject croshair;
    public GameObject GrapplesNumber;
    public GameObject tutorial;


    // Start is called before the first frame update
    void Start()
    {
        won.SetActive(false);
        lose.SetActive(false);
        croshair.SetActive(true);
        GrapplesNumber.SetActive(false);
        // StartCoroutine(ClearTutorial());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            tutorial.SetActive(false);
            GrapplesNumber.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("set cursor invisible");
        }
    }

    /*
    IEnumerator ClearTutorial(){
        tutorial.SetActive(true);
        yield return new WaitForSeconds(20);
        tutorial.SetActive(false);
    }
    */

    public void PlayerWon() { 
        
        Reset();
        won.SetActive(true);
    }

    public void PlayerLose() {
        // Reset();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        lose.SetActive(true);
    }


    public void Reset()
    {
        won.SetActive(false);
        lose.SetActive(false);
        croshair.SetActive(false);
        GrapplesNumber.SetActive(false);
    }

}
