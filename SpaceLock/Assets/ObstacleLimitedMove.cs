using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLimitedMove : MonoBehaviour
{

    public float minSpeed = 1.5f;
    private float speed;
    private GameObject player;
    public Material farObstacle;
    public Material nearObstacle;
    private Grapple gp;
    private Vector3 pointA = new Vector3(-69.1F, 8.2F, 32.4F);
    private Vector3 pointB = new Vector3(-69.1F, 8.2F, -39.8F);

    void Start()
    {

      player = GameObject.FindWithTag("Player");
      if (player == null) {
          Debug.LogError("Player1 tag not found! Make sure your player object has the correct tag.");
          return;
      }
      gp = player.GetComponent<Grapple>();
      Vector3 scale = transform.localScale;

      float scaleFactor = (scale.x + scale.y + scale.z) / 3f;

      speed =  minSpeed * scaleFactor;
    }

    void Update()
    {
      // transform.Translate(speed * Time.deltaTime * Vector3.right);
      // tried making the block move back and forth but not working as intended had to comment out
      float time = Mathf.PingPong(Time.deltaTime * speed, 1);
      // transform.Translate(-69.1F, 8.2F, Vector3.Lerp(pointA, pointB, time));

      float distance = Vector3.Distance(player.transform.position, transform.position);

      if (distance < gp.maxGrappleDistance) {
          GetComponent<Renderer>().material = nearObstacle;
      } else {
          GetComponent<Renderer>().material = farObstacle;
      }

    }
}

