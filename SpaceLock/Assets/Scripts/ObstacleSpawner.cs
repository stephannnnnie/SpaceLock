using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    private Vector3 origin;
    [SerializeField] private GameObject obstaclePrefab;
    [SerializeField] private int numObstacles = 10;
    private List<GameObject> obstaclePool;
    private Vector3 spawnerSize;
    private readonly float startDelay = 0f;
    private readonly float spawnInterval = 2.0f;
    [SerializeField] private float powerUpSpawnChance = 0.2f; // 20% chance to spawn with power-up
    [SerializeField] private GameObject powerUpPrefab; // Prefab for power-up
    [SerializeField] private GameObject particleEffectPrefab;
    [SerializeField] private string direction;

    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        spawnerSize = transform.localScale;

        obstaclePool = new List<GameObject>();

        for (int i = 0; i < numObstacles; i++)
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
            float randomX = Random.Range(origin.x, origin.x + 1.0f); ;
            float randomY = Random.Range(origin.y - spawnerSize.z * 3.0f, origin.y + spawnerSize.z * 3.0f);
            float randomZ = Random.Range(origin.z - spawnerSize.x * 3.0f, origin.z + spawnerSize.x * 3.0f);
            
            Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);
            obstacle.transform.position = spawnPosition;
            obstacle.transform.rotation = obstaclePrefab.transform.rotation;
            obstacle.GetComponent<ObstaclePrefab>().direction = direction;
            obstacle.SetActive(true);

            float randomScale = Random.Range(4f, 8f);
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

    // Update is called once per frame
    void Update()
    {
        foreach (var obstacle in obstaclePool)
        {
            bool collideWall = obstacle.GetComponent<ObstaclePrefab>().collideWall;

            if (obstacle.activeInHierarchy && collideWall)
            {
                obstacle.GetComponent<ObstaclePrefab>().collideWall = false;
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
        powerUpPosition.y += obstacle.transform.localScale.y / 2 + 3f; // Adjust to place the power-up on top of the obstacle

        GameObject powerUp = Instantiate(powerUpPrefab, powerUpPosition, Quaternion.identity);
        powerUp.transform.SetParent(obstacle.transform); // Attach to obstacle so it moves with it

        // bc effect is kinda in front will have to move it back a bit
        powerUpPosition.z -= 2.5f;
        GameObject particleEffect = Instantiate(particleEffectPrefab, powerUpPosition, Quaternion.identity);
        particleEffect.transform.SetParent(powerUp.transform);

        // Dynamically set the PowerUpType
        PowerUp powerUpScript = powerUp.GetComponent<PowerUp>();
        if (powerUpScript != null)
        {
            powerUpScript.powerUpType = (PowerUp.PowerUpType)Random.Range(0, 2); // Randomly select between ExtraGrapple and IncreaseGrappleDistance
            powerUpScript.PowerUpEffect = particleEffect.GetComponent<ParticleSystem>();
        }
    }
}
