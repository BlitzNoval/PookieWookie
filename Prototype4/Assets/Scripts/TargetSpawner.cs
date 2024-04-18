using UnityEngine;
using System.Collections;

public class TargetSpawner : MonoBehaviour
{
    public static TargetSpawner Instance;
    public GameObject targetPrefab;
    public Transform[] spawnPoints;
    private GameObject currentTarget = null;
    private float targetLifetime = 3.0f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        SpawnTarget();
    }

    void Update()
    {
        if (currentTarget == null)
        {
            SpawnTarget();
        }
    }

    void SpawnTarget()
    {
        Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector3 spawnPosition = randomSpawnPoint.position;
        currentTarget = Instantiate(targetPrefab, spawnPosition, Quaternion.identity);
        currentTarget.GetComponent<Target>().spawner = this;
        currentTarget.GetComponent<Target>().SpawnTime = Time.time;
        StartCoroutine(DestroyTargetAfterDelay(targetLifetime));
    }

    IEnumerator DestroyTargetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
            GameManager.Instance.RegisterMiss(); 
        }
    }

    public void TargetHit()
    {
        StopAllCoroutines(); 
        if (currentTarget != null)
        {
            Destroy(currentTarget);
            currentTarget = null;
        }
    }

    public void ResetSpawner()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

}
