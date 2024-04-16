using UnityEngine;

public class Target : MonoBehaviour
{
    public int scoreValue = 10;

    public void OnHit()
    {
        
        GameManager.Instance.UpdateScore(scoreValue);
        Destroy(gameObject);
    }
}
