using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuCanvas : MonoBehaviour
{
    public GameObject croshair;
    public int noofGrapples = 0;

    [SerializeField] GameObject barr;
    [SerializeField] Image GrappleDistance;

    const float MAX_GRAPPLES = 20f;
    const float MAX_DISTANCE = 200f;

    [SerializeField] private GameObject GrappleBarContainer; // Parent container for grapple segments
    [SerializeField] private List<Image> grappleSegments; // List to hold each segment image

    // Start is called before the first frame update
    void Start()
    {
        croshair.SetActive(true);
        noofGrapples = 0;
        barr.SetActive(true);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UpdateGrappleNumber(float remainingGrapple, float distance)
    {
        Debug.Log("Remaining grapples: " + remainingGrapple);
        for (int i = 0; i < grappleSegments.Count; i++)
        {
            // Enable the segment if the index is less than remainingGrapple, otherwise disable it
            bool shouldEnable = i + 1 <= remainingGrapple;
            grappleSegments[i].enabled = shouldEnable;

            Debug.Log(grappleSegments[i].name + " " + i + " " + "shouldEnable: " + shouldEnable);
        }
        //GrappleIncrease.fillAmount = remainingGrapple / MAX_GRAPPLES;
        GrappleDistance.fillAmount = distance / MAX_DISTANCE;

    }

    public void updateGrappless()
    {
        noofGrapples++;
    }
}