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
    public GameObject particleEffectPrefab;
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

            float randomScale = Random.Range(3f, 5f);
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

            float randomScale = Random.Range(3f, 5f);
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

        // Destroy any PowerUp component child attached to this obstacle
        foreach (Transform child in obstacle.transform)
        {
            if (child.GetComponent<PowerUp>() != null)
            {
                Destroy(child.gameObject);
            }
        }
    }

    void SpawnPowerUpAboveObstacle(GameObject obstacle)
    {
        Vector3 powerUpPosition = obstacle.transform.position;
        powerUpPosition.y += obstacle.transform.localScale.y / 2 + 6f; // Adjust height

        // Randomly select a power-up prefab
        GameObject powerUpPrefab = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
        GameObject powerUp = Instantiate(powerUpPrefab, powerUpPosition, Quaternion.identity);
        powerUp.transform.SetParent(obstacle.transform); // Attach to obstacle

        // Adjust particle effects if necessary
        powerUpPosition.x += 3.0f; // Offset for visual alignment
        GameObject particleEffect = Instantiate(particleEffectPrefab, powerUpPosition, Quaternion.identity);
        particleEffect.transform.SetParent(powerUp.transform);

        // Assign to the PowerUp script
        PowerUp powerUpScript = powerUp.GetComponent<PowerUp>();
        if (powerUpScript != null)
        {
            powerUpScript.PowerUpEffect = particleEffect.GetComponent<ParticleSystem>();
        }
    }
}
