using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform[] spawnPoints; // Array of spawn points
    public float spawnInterval = 1f;

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
        // Check if GameManager instance exists and game timer is positive
        if (GameManager.Instance != null && GameManager.Instance.timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                SpawnTarget();
                timer = spawnInterval;
            }
        }
    }

    void SpawnTarget()
    {
        // Pick a random spawn point from the array
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Calculate spawn position with some random offset around the selected spawn point
        Vector3 spawnPosition = randomSpawnPoint.position + new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));

        // Instantiate the target at the calculated position
        GameObject newTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
        
        // Optionally, set the layer of the instantiated target
        newTarget.layer = LayerMask.NameToLayer("Target");
    }
}
