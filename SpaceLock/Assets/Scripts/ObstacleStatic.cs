using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleStatic : MonoBehaviour
{
    private GameObject player;
    public Material farObstacle;
    public Material nearObstacle;
    private Grapple gp;

    void Start()
    {
      player = GameObject.FindWithTag("Player");
      if (player == null) {
          Debug.LogError("Player1 tag not found! Make sure your player object has the correct tag.");
          return;
      }
      gp = player.GetComponent<Grapple>();

    }

    void LateUpdate()
    {
      float distance = Vector3.Distance(player.transform.position, transform.position);

      if (distance < gp.maxGrappleDistance) {
          GetComponent<Renderer>().material = nearObstacle;
      } else {
          GetComponent<Renderer>().material = farObstacle;
      }

    }
}
