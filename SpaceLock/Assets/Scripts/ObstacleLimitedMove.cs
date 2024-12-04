using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleLimitedMove : MonoBehaviour
{
    public float minSpeed = 0.5f;
    public float speed;
    private GameObject player;
    public Material farObstacle;
    public Material nearObstacle;
    private Grapple gp;
    public Vector3 pointA;
    public Vector3 pointB;

    public Vector3 loopBoundaryLeft;
    public Vector3 loopBoundaryRight;

    public bool isLooping;

    public bool movingRight = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player tag not found! Make sure your player object has the correct tag.");
            return;
        }

        gp = player.GetComponent<Grapple>();

        /*Vector3 scale = transform.localScale;
        float scaleFactor = (scale.x + scale.y + scale.z) / 3f;
        speed = minSpeed * scaleFactor;*/
    }

    void Update()
    {
        if (isLooping)
        {
            HandleLoopingMovement();
        }
        else
        {
            HandlePingPongMovement();
        }

        UpdateObstacleMaterial();
    }

    void HandlePingPongMovement()
    {
        transform.position = Vector3.Lerp(pointA, pointB, Mathf.PingPong(Time.time / 2.5f, 1));
    }

    void HandleLoopingMovement()
    {
        Vector3 position = transform.position;

        if (movingRight)
        {
            position.z += speed * Time.deltaTime;
        }
        else
        {
            position.z -= speed * Time.deltaTime;
        }

        transform.position = position;

        if (position.z > loopBoundaryRight.z && movingRight)
        {
            position.z = loopBoundaryLeft.z;
            transform.position = position;
        }
        else if (position.z < loopBoundaryLeft.z && !movingRight)
        {
            position.z = loopBoundaryRight.z;
            transform.position = position;
        }
    }

    void UpdateObstacleMaterial()
    {
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
}
