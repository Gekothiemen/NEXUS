    using UnityEngine;
    using UnityEngine.InputSystem;
public class CharacterController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;

    public LayerMask WhatIsGround;
    public Collider playercollision;

    [Header("player stats")]

    [Header("Jump")]
    [SerializeField] int MaxJumps = 3;
    [SerializeField] int jumpsLeft;
    [SerializeField] private float jumpForce = 10;
    public bool jumpRequested;

    [Header("move")]
    [SerializeField] private float sprintSpeed = 2f;
    [SerializeField] private float walkSpeed;
    public float currentSpeed;

    private void OnCollisionEnter(Collision collision)
    {
      // kaas

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        WhatIsGround = GetComponent<LayerMask>();
        // formula velocity jump
        Vector3 currentVel = rb.linearVelocity;
        currentVel.y = 0f;
        rb.linearVelocity = currentVel;

        jumpsLeft = MaxJumps;
        currentSpeed = walkSpeed;
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        
    }


    public void OnJump(InputAction.CallbackContext ctx)
    {

        if (ctx.started)
        {
            if (jumpsLeft > 0)
            {
                jumpsLeft -= 1;
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                Debug.Log("jumped");
            }

        }

    }


    public void OnMovement(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        Debug.Log("current speed: " + currentSpeed);
    }


   


    public void OnSprint(InputAction.CallbackContext ctx)
    {

        if (ctx.started)
        {
            currentSpeed = currentSpeed * sprintSpeed;
        }
        if (ctx.canceled)
        {
            currentSpeed = walkSpeed;
        }


    }


    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log("Current move input: " + moveInput);
        rb.linearVelocity = new Vector3(moveInput.x * currentSpeed, rb.linearVelocity.y, moveInput.y * currentSpeed);
    }
}
