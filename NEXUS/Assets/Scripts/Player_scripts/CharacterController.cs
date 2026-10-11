using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
public class CharacterController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;

    public LayerMask WhatIsGround;



    [Header("player stats")]



    [Header("Jump")]
    [SerializeField] private int MaxJumps = 3;
    [SerializeField] public int jumpsLeft;
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float radius;
    [SerializeField] private Transform groundCheck;

    [Header("Dash Speed")]
    [SerializeField] private float DashSpeed = 3f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = true;
    public bool isDashing = false;



    [Header("Move Speed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed = 2f;
    public float currentSpeed;


    [Header("Other Variables")]


    public float rotationSpeed = 720f;
    public Vector3 moveDirection;

    // Input collection variables
    private float inputHorizontal;
    private float inputVertical;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        jumpsLeft = MaxJumps;

        currentSpeed = walkSpeed;



    }

    private void Update()
    {
        // CHECK GROUNDS SO JUMP RESETS     
        Collider[] groundObjects = Physics.OverlapSphere(groundCheck.position, radius, WhatIsGround);
        if (groundObjects.Length > 0)
        {
            jumpsLeft = MaxJumps;
        }

        
        //inputHorizontal = Input.GetAxis("Horizontal");
        //inputVertical = Input.GetAxis("Vertical");

        // 2. CALCULATE CAMERA-RELATIVE DIRECTION
        Vector3 camForward = UnityEngine.Camera.main.transform.forward;
        Vector3 camRight = UnityEngine.Camera.main.transform.right;

        if (isDashing)
            return;

        moveDirection = (camForward * inputVertical + camRight * inputHorizontal).normalized;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        //3.HANDLE ROTATION(Visuals update perfectly per frame)
        if (moveDirection != Vector3.zero && !isDashing)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);



        }

    }


    public void OnJump(InputAction.CallbackContext ctx)
    {

        if (ctx.started && jumpsLeft > 0)
        {
            Debug.Log("has jumped");
            jumpsLeft -= 1;
            Vector3 currentVel = rb.linearVelocity;
            currentVel.y = 0f;
            rb.linearVelocity = currentVel;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("jumped");

        }

    }

    public void OnMovement(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        inputHorizontal = moveInput.x;
        inputVertical = moveInput.y;
        Debug.Log("current speed: " + currentSpeed);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        if (isDashing)
            return;

        if (ctx.started)
        {
            currentSpeed = currentSpeed * sprintSpeed;
        }
        if (ctx.canceled)
        {
            currentSpeed = walkSpeed;
        }


    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && canDash && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Debug.Log("Current move input: " + moveInput);

        if (isDashing)
            return;


        rb.linearVelocity = new Vector3(moveDirection.x * currentSpeed, rb.linearVelocity.y, moveDirection.z * currentSpeed);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float dashTimer = 0f;

        // If player isn't moving an axis, dash in the direction they are facing
        Vector3 dashDir = moveDirection != Vector3.zero ? moveDirection : transform.forward;

        while (dashTimer < dashDuration)
        {
            dashTimer += Time.fixedDeltaTime; // Match physics clock loops inside coroutines

            Vector3 dashVelocity = dashDir * DashSpeed;
            dashVelocity.y = rb.linearVelocity.y; // Keep gravity working while dashing
            rb.linearVelocity = dashVelocity;

            yield return new WaitForFixedUpdate(); // Wait for next FixedUpdate frame
        }

        isDashing = false;
        currentSpeed = walkSpeed;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
   



}
