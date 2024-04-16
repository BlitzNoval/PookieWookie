using UnityEngine;

public class Interactable : MonoBehaviour
{
    public void OnLookInteraction()
    {
        Debug.Log("Player is looking at: " + gameObject.name);
        
    }
}
