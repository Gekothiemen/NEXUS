using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;


public class Playermovement : MonoBehaviour
{
    public float rotationSpeed = 720f;

    [SerializeField]
    InputAction jump;

    [SerializeField]
    float jumpForce = 5f;

    Rigidbody rb;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float mouseSensitivity = 2f;

    private Vector3 moveDirection;
    private float rotationY;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();


        // Moves player rotation to input AWSD
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        jump.Enable();
    }
    private void FixedUpdate()
    {
        if (jump.IsPressed())
        {
            // needs to be changed later in the future when using height    
            if (gameObject.transform.position.y < 1.1)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(horizontal, 0f, vertical);
        moveDirection.Normalize();

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }

    // Mouse rotation
    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        rotationY += mouseX * mouseSensitivity;

        
    }
}
