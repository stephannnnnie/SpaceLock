using UnityEngine;
using System.Collections.Generic;

public class RandomObstacleSpawner : MonoBehaviour
{

    public GameObject obstaclePrefab; // Single obstacle prefab
    public int numberOfObstacles = 20; // Number of obstacles to spawn
    private readonly float startDelay = 0f;
    private readonly float spawnInterval = 0.5f;
    private List<GameObject> obstaclePool;
    public GameObject drespawn;
    public GameObject[] powerUpPrefabs; // Array to hold different power-up prefabs
    //public GameObject particleEffectPrefab;
    public float xDistance;
    [Range(0f, 1f)]
    public float powerUpSpawnChance = 0.2f; // 20% chance to spawn with power-up

    void Start()
    {
        obstaclePool = new List<GameObject>();

        for (int i = 0; i < numberOfObstacles; i++)
        {
            GameObject obstacle = Instantiate(obstaclePrefab);
            obstacle.SetActive(false);
            obstaclePool.Add(obstacle);
        }

        for (int i = 0; i < 60; i++)
        {
            SpawnOriginalObstacles();
        }
        
        InvokeRepeating(nameof(SpawnObstacles), startDelay, spawnInterval);
    }

    void SpawnOriginalObstacles()
    {
        GameObject obstacle = GetPooledObstacle();
        if (obstacle != null)
        {
            float randomZ = Random.Range(-48f, 44f);
            float x = Random.Range(-160f,200f);
            float randomY = Random.Range(2f, 48f);

            Vector3 spawnPosition = new Vector3(x, randomY, randomZ);
            obstacle.transform.position = spawnPosition;
            obstacle.transform.rotation = obstaclePrefab.transform.rotation;
            obstacle.SetActive(true);

            float randomScale = Random.Range(6f, 9f);
            obstacle.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            float mass = randomScale * 10f;

            if (obstacle.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.mass = mass;
            }

            // Spawn power-up on top of the obstacle based on probability
            if (Random.value < powerUpSpawnChance)
            {
                SpawnPowerUpAboveObstacle(obstacle);
            }
        }
    }
    
    void SpawnObstacles()
    {
        GameObject obstacle = GetPooledObstacle();
        if (obstacle != null)
        {
            float randomZ = Random.Range(-48f, 44f);
            float x = xDistance;
            float randomY = Random.Range(2f, 48f);

            Vector3 spawnPosition = new Vector3(x, randomY, randomZ);
            obstacle.transform.position = spawnPosition;
            obstacle.transform.rotation = obstaclePrefab.transform.rotation;
            obstacle.SetActive(true);

            float randomScale = Random.Range(6f, 9f);
            obstacle.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            float mass = randomScale * 10f;

            if (obstacle.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.mass = mass;
            }

            // Spawn power-up on top of the obstacle based on probability
            if (Random.value < powerUpSpawnChance)
            {
                SpawnPowerUpAboveObstacle(obstacle);
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

        // Collect all child objects of the obstacle
        List<Transform> childrenToDestroy = new List<Transform>();
        foreach (Transform child in obstacle.transform)
        {
            childrenToDestroy.Add(child); // Add child to the list
        }

        // Destroy all collected children
        foreach (Transform child in childrenToDestroy)
        {
            Destroy(child.gameObject); // Destroy each child
        }
    }

    void SpawnPowerUpAboveObstacle(GameObject obstacle)
    {
        Vector3 powerUpPosition = obstacle.transform.position;

        int randomIndex = Random.Range(0, powerUpPrefabs.Length);
        GameObject powerUpPrefab = powerUpPrefabs[randomIndex];
        GameObject powerUp = null;
        
        switch (randomIndex)
        {
            case 0:  // bullet
                powerUpPosition.y += obstacle.transform.localScale.y / 2 + 0.5f;
                powerUp = Instantiate(powerUpPrefab, powerUpPosition, Quaternion.Euler(0, -90, 0));
                break;

            case 1:  // ring
                powerUpPosition.y += obstacle.transform.localScale.y / 2 + 5f; // Adjust height for power-up
                powerUp = Instantiate(powerUpPrefab, powerUpPosition, Quaternion.identity);
                break;
        }

        if (powerUp != null)
        {
            powerUp.transform.SetParent(obstacle.transform); // Attach power-up to obstacle
        }
    }
}
