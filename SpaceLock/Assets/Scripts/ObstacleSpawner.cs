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
    public float spawnInterval = 2.0f;
    [SerializeField] private float powerUpSpawnChance = 0.2f; // 20% chance to spawn with power-up
    public GameObject[] powerUpPrefabs; // Array to hold different power-up prefabs
    //[SerializeField] private GameObject particleEffectPrefab;
    [SerializeField] private string direction;
    [SerializeField] float MinObstcileSpeed;

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
            // Get the plane's renderer to determine its bounds
            Renderer planeRenderer = GetComponent<Renderer>();

            // Calculate the plane's bounds
            Bounds planeBounds = planeRenderer.bounds;

            // Generate random position within the plane's bounds
            float randomX = Random.Range(planeBounds.min.x, planeBounds.max.x);
            float randomZ = Random.Range(planeBounds.min.z, planeBounds.max.z);
            float randomY = Random.Range(planeBounds.min.y, planeBounds.max.y );

            // Use the plane's Y position to ensure the obstacle spawns on the plane surface
            Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

            Debug.Log(spawnPosition);

            obstacle.transform.position = spawnPosition;
            obstacle.transform.rotation = obstaclePrefab.transform.rotation;
            obstacle.GetComponent<ObstaclePrefab>().direction = direction;
            obstacle.SetActive(true);
            obstacle.GetComponent<ObstaclePrefab>().minSpeed = MinObstcileSpeed;

            float randomScale = Random.Range(5f, 8f);
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
            if (child.GetComponent<Grapple>() != null) {
                transform.parent = null;
            }
            
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
                powerUp = Instantiate(powerUpPrefab, powerUpPosition, Quaternion.identity); // may need to consider for different direction value
                break;

            case 1:  // ring
                powerUpPosition.y += obstacle.transform.localScale.y / 2 + 2f; // Adjust height for power-up
                powerUp = Instantiate(powerUpPrefab, powerUpPosition, Quaternion.identity);
                break;
        }

        if (powerUp != null)
        {
            powerUp.transform.SetParent(obstacle.transform); // Attach power-up to obstacle
        }
    }
}
