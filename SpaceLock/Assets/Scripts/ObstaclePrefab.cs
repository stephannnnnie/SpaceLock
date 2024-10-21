using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePrefab : MonoBehaviour
{
    private float minSpeed = 1.5f;
    private float speed;
    public bool collideWall;
    public string direction;
    private Vector3 dir;

    void Start()
    {

        Vector3 scale = transform.localScale;

        float scaleFactor = (scale.x + scale.y + scale.z) / 3f;

        speed = minSpeed * scaleFactor;

        collideWall = false;

        if (direction == "up")
        {
            dir = Vector3.up;
        }
        else if (direction == "down")
        {
            dir = Vector3.down;
        }
        else if (direction == "right")
        {
            dir = Vector3.right;
        }
        else if (direction == "left")
        {
            dir = Vector3.left;
        } else
        {
            Debug.Log("Wrong Direction");
        }
    }

    void Update()
    {
        transform.Translate(speed * Time.deltaTime * dir);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wall"))
        {
            collideWall = true;
        }
    }
}
