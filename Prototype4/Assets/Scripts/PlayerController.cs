using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float lookSpeed = 2f;
    public GameObject reticle;
    public static float sensitivity = 0.4f;
    private Camera playerCamera;
    public Transform startPoint;

    public AudioClip[] hitSounds;
    public AudioClip missSound;
    public ParticleSystem hitEffectPrefab;
    private AudioSource audioSource;
    private static int consecutiveHits = 0;
    private bool isPlayingFinalSound = false;
    private float pitch = 0f;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        audioSource = GetComponent<AudioSource>();
        if (startPoint != null)
        {
            playerCamera.transform.LookAt(startPoint.position);
            pitch = playerCamera.transform.localEulerAngles.x;
        }
        reticle.SetActive(true);
    }

    void Update()
    {
        if (GameManager.Instance.gameStarted)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * lookSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * lookSpeed;

            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, -90f, 90f);

            transform.Rotate(0f, mouseX, 0f);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, playerCamera.transform.localEulerAngles.y, 0f);

            HandleShooting();
        }
    }

    private void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    consecutiveHits++;
                    if (!isPlayingFinalSound) {
                        StartCoroutine(PlayHitSound());
                    }
                    ShowHitEffect(hit.point);
                    GameManager.Instance.RegisterShot();
                    target.OnHit();
                }
            }
            else
            {
                ResetHits();
                GameManager.Instance.RegisterShot();
            }
        }
    }

    private IEnumerator PlayHitSound()
    {
        int soundIndex = Mathf.Clamp(consecutiveHits - 1, 0, hitSounds.Length - 1);
        audioSource.clip = hitSounds[soundIndex];
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        if (consecutiveHits >= 5)
        {
            isPlayingFinalSound = true; 
            audioSource.clip = hitSounds[4];
            audioSource.Play();
            yield return new WaitForSeconds(audioSource.clip.length);
            isPlayingFinalSound = false;
            consecutiveHits = 0;
        }
    }

    private void ResetHits()
    {
        consecutiveHits = 0;
        audioSource.clip = missSound;
        audioSource.Play();
    }

    private void ShowHitEffect(Vector3 hitPosition)
    {
        Instantiate(hitEffectPrefab, hitPosition, Quaternion.identity);
    }

    public void ResetCameraToStartPoint()
    {
        if (startPoint != null)
        {
            playerCamera.transform.LookAt(startPoint.position);
            pitch = playerCamera.transform.localEulerAngles.x;
        }
        else
        {
            Debug.LogError("StartPoint not set in PlayerController");
        }
    }
}
