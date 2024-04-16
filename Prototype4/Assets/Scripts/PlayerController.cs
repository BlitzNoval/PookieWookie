using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float maxInteractDistance = 10f;
    public GameObject reticle; // Reference to the reticle GameObject

    private Camera playerCamera;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
        // Movement input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Mouse look input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 rotation = transform.localEulerAngles;
        rotation.y += mouseX * lookSpeed;
        transform.localEulerAngles = rotation;

        Vector3 cameraRotation = playerCamera.transform.localEulerAngles;
        cameraRotation.x -= mouseY * lookSpeed;
        playerCamera.transform.localEulerAngles = cameraRotation;

        // Raycast from the center of the screen
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        // Draw debug ray
        Debug.DrawRay(ray.origin, ray.direction * maxInteractDistance, Color.red);

        RaycastHit hit;

        // Check if the ray hits an interactable object within maxInteractDistance
        if (Physics.Raycast(ray, out hit, maxInteractDistance))
        {
            if (hit.collider.CompareTag("Target"))
            {
                // Display reticle at hit point
                reticle.SetActive(true);

                // Check for mouse click to destroy the target
                if (Input.GetMouseButtonDown(0)) // 0 for left mouse button
                {
                    // Retrieve the Target component and destroy the target
                    Target target = hit.collider.GetComponent<Target>();
                    if (target != null)
                    {
                        target.OnHit(); // Trigger destruction logic in Target script
                    }

                    // Hide reticle after interaction
                    reticle.SetActive(false);
                }
            }
            else
            {
                reticle.SetActive(false); // Hide reticle if not looking at a target
            }
        }
        else
        {
            reticle.SetActive(false); // Hide reticle if not hitting anything
        }
    }
}
