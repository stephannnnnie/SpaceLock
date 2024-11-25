using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class GameOverScript : MonoBehaviour
{

    public GameObject Player;
    public Canvas cv;
    public float DistanceZ;
    public float DistanceX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xx = Mathf.Abs(this.transform.position.x - Player.transform.position.x);
        float zz = Mathf.Abs(this.transform.position.z - Player.transform.position.z);

        if (xx < DistanceX && zz < DistanceZ )
        {
            cv.PlayerLose(1);
        }
    }
}
