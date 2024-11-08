using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level3_ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // Single obstacle prefab
    public int numberOfObstacles = 20; // Number of obstacles to spawn
    private readonly float startDelay = 0f;
    private readonly float spawnInterval = 0.5f;
    private List<GameObject> obstaclePool;
    public GameObject drespawn;
    public float xDistance;

    void Start()
    {
        obstaclePool = new List<GameObject>();

        for (int i = 0; i < numberOfObstacles; i++)
        {
            GameObject obstacle = Instantiate(obstaclePrefab);
            obstacle.SetActive(false);
            obstaclePool.Add(obstacle);
        }

        InvokeRepeating(nameof(SpawnObstacles), startDelay, spawnInterval);
    }

    void SpawnObstacles()
    {
        GameObject obstacle = GetPooledObstacle();
        if (obstacle != null)
        {
            float randomZ = Random.Range(-95f, 103f);
            float x = xDistance;
            float randomY = Random.Range(2f, 200f);

            Vector3 spawnPosition = new Vector3(x, randomY, randomZ);
            obstacle.transform.position = spawnPosition;
            obstacle.transform.rotation = obstaclePrefab.transform.rotation;
            obstacle.SetActive(true);

            float randomScale = Random.Range(4f, 8f);
            obstacle.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            float mass = randomScale * 10f;

            if (obstacle.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.mass = mass;
            }
        }
    }

    GameObject GetPooledObstacle()
    {
        foreach (var obstacle in obstaclePool)
        {
            if (!obstacle.activeInHierarchy)
            {
                return obstacle;
            }
        }

        return null;
    }

    void Update()
    {
        foreach (var obstacle in obstaclePool)
        {
            if (obstacle.activeInHierarchy && obstacle.transform.position.x > drespawn.transform.position.x)
            {
                RecycleObstacle(obstacle);
            }
        }
    }

    void RecycleObstacle(GameObject obstacle)
    {
        obstacle.SetActive(false);
    }
}
