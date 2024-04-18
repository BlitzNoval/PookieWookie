using UnityEngine;

public class Target : MonoBehaviour
{
    public int scoreValue = 10;
    public TargetSpawner spawner;
    public float SpawnTime { get; set; }

    public void OnHit()
    {
    GameManager.Instance.UpdateScore(scoreValue);
    GameManager.Instance.RegisterHit(SpawnTime);
    spawner.TargetHit();
    Destroy(gameObject);
    }

}
