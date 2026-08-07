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

    //[SerializeField]
    // private float mouseSensitivity = 2f;

    private Vector3 moveDirection;
    private float rotationY;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();

        // Turn character towards the direction of movement ONLY when giving input
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
        // When moveDirection == Vector3.zero (standing still/idle), 
        // the player does NOT rotate, allowing you to orbit the camera freely around them!
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

    // to change delay of the input go to "Project-Settings -> Input Manager and dropdown the axis, then dropdown the horizontal and vertical"
    // Gravity is for delay when stopping (Higher number = stops faster)
    // Sensitivity is for delay when beginning to walk (Higher number = faster response)
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 1. Get the main camera's forward and right vectors
        Vector3 camForward = UnityEngine.Camera.main.transform.forward;
        Vector3 camRight = UnityEngine.Camera.main.transform.right;

        // 2. Ignore pitch (up/down looking) by flattening Y to 0
        camForward.y = 0f;
        camRight.y = 0f;

        // 3. Re-normalize to keep movement speed consistent
        camForward.Normalize();
        camRight.Normalize();

        // 4. Calculate movement direction strictly along the horizontal plane
        moveDirection = (camForward * vertical + camRight * horizontal).normalized;

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
    }




    /*
    // Mouse rotation
    private void HandleRotation()
    {
        float mouseX = Input.GetAxis("Mouse X");
        rotationY += mouseX * mouseSensitivity;

        
    }
    */

}
