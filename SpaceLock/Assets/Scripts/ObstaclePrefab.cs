using System.Collections;
using System.Collections.Generic;
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
        if (direction == "right")
        {
            dir = Vector3.back; // This is equivalent to new Vector3(0, 0, -1)
        }
        else if (direction == "left")
        {
            dir = Vector3.forward; // This is equivalent to new Vector3(0, 0, 1)
        }
        else
        {
            Debug.LogError("Wrong Direction. Use 'right' or 'left'.");
        }
    }

    void Update()
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
}
