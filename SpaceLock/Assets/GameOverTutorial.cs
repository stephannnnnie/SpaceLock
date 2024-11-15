using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverTutorial : MonoBehaviour
{
    public Canvas cv;
    public GameObject Player;
    public float distanceZ = 0.5f;
    public float distacneX = 0.1f;

    // Update is called once per frame
    void Update()
    {
        float zz = Mathf.Abs(this.transform.position.z - Player.transform.position.z);
        float yy = Mathf.Abs(this.transform.position.x - Player.transform.position.x);

        if (zz < distanceZ && yy < distacneX) {
            cv.PlayerLose(1);
            
        }

    }
}
