using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 2f;
    public float maxInteractDistance = 10f;

    private Camera playerCamera;

    void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked; 
    }

    void Update()
    {
     
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

      
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        Vector3 rotation = transform.localEulerAngles;
        rotation.y += mouseX * lookSpeed;
        transform.localEulerAngles = rotation;

        Vector3 cameraRotation = playerCamera.transform.localEulerAngles;
        cameraRotation.x -= mouseY * lookSpeed;
        playerCamera.transform.localEulerAngles = cameraRotation;

        
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

     
        Debug.DrawRay(ray.origin, ray.direction * maxInteractDistance, Color.red);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxInteractDistance))
        {
           
            if (hit.collider.CompareTag("Target"))
            {
             
                Destroy(hit.collider.gameObject);
            }
        }
    }
}
