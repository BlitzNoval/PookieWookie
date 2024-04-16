using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public GameObject targetPrefab;
    public Transform spawnPoint;
    public float spawnInterval = 1f;

    private float timer;

    void Start()
    {
        timer = spawnInterval;
    }

    void Update()
    {
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
        Vector3 spawnPosition = spawnPoint.position + new Vector3(Random.Range(-5f, 5f), 0f, Random.Range(-5f, 5f));
        GameObject newTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
        
        newTarget.layer = LayerMask.NameToLayer("Target");
    }
}
