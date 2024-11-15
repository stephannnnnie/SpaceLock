using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTutorial : MonoBehaviour
{
    public Canvas cv;
    public GameObject Player;
    public float distance = 10f;

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(this.transform.position.z - Player.transform.position.z) < distance) {
            cv.PlayerLose(1);
        }
    }
}
