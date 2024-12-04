using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstaclePrefab : MonoBehaviour
{
    public float minSpeed = 1.5f;
    private float speed;
    public bool collideWall;
    public string direction;
    private Vector3 dir;
    public Material farObstacle;
    public Material nearObstacle;
    private GameObject player;
    private Grapple gp;


    void Start()
    {
        player = GameObject.FindWithTag("Player");
        Vector3 scale = transform.localScale;
        float scaleFactor = (scale.x + scale.y + scale.z) / 3f;
        speed = minSpeed * scaleFactor;
        collideWall = false;
        gp = player.GetComponent<Grapple>();

        if (string.IsNullOrEmpty(direction))
        {
            Debug.LogError("Direction is null or empty. Ensure it is set correctly in ObstacleSpawner.");
        }
        if (direction == "right")
        {
            dir = Vector3.back; // This is equivalent to new Vector3(0, 0, -1)
        }
        else if (direction == "left")
        {
            dir = Vector3.forward; // This is equivalent to new Vector3(0, 0, 1)
        }
        else if (direction == "up")
        {
            dir = Vector3.up;
        }
        else if (direction == "Down")
        {
            dir = Vector3.down;
        }
   
        else
        {
            Debug.LogError("Wrong Direction. Use 'right' or 'left'.");
        }
    }

    void LateUpdate()
    {
        transform.Translate(speed * Time.deltaTime * dir);

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance < gp.maxGrappleDistance)
        {
            GetComponent<Renderer>().material = nearObstacle;
        }
        else
        {
            GetComponent<Renderer>().material = farObstacle;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wall"))
        {
            collideWall = true;
        }
        
    }

    /*    private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Obstacle"))
            {
                Debug.Log("Obstacles collision detected");
                float distance_2 = Vector3.Distance(player.transform.position, transform.position);
                float distance_1 = Vector3.Distance(player.transform.position, collision.gameObject.transform.position);

                if (transform.localScale.x > collision.gameObject.transform.localScale.x)
                {
                    if (distance_1 > 2.0f)
                    {
                        collision.gameObject.GetComponent<ObstaclePrefab>().collideWall = true;
                    }
                }
                else if (distance_2 > 2.0f)
                {
                    collideWall = true;
                }
            }
        }*/
}
