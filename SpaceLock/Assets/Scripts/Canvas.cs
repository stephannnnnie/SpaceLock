using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{

    public GameObject won;
    public GameObject lose;
    public GameObject croshair;
    public GameObject GrapplesNumber;


    // Start is called before the first frame update
    void Start()
    {
        won.SetActive(false);
        lose.SetActive(false);
        croshair.SetActive(true);
        GrapplesNumber.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
