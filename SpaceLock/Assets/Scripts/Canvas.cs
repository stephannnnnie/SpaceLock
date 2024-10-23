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
        if (Input.anyKey) {
            tutorial.SetActive(false);
            GrapplesNumber.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
        Reset();
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
