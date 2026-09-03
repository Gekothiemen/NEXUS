using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermovement : MonoBehaviour
{
    [SerializeField] private float doubleTapTime = 0.3f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;

    private float lastShiftTapTime = -1f;
    private bool canDash = true;
    public bool isDashing = false;

    public float rotationSpeed = 720f;

    [SerializeField] int MaxJumps = 1;
    [SerializeField] int jumpsLeft = 1;
    [SerializeField] float jumpForce = 5f;

    Rigidbody rb;

    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float speed;
    [SerializeField] private float sprintSpeed = 1.5f;
    [SerializeField] private float DashSpeed = 3f;

    // Input collection variables
    private float inputHorizontal;
    private float inputVertical;
    private bool jumpRequested = false;

    public Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = baseSpeed;

        // CLEANUP: Clean up the redundant UnityEditor import from the top of your file
    }

    void Update()
    {
        // 1. GATHER INPUT CONSTANTLY (Never misses a keystroke)
        inputHorizontal = Input.GetAxis("Horizontal");
        inputVertical = Input.GetAxis("Vertical");

       
       

        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0)
        {
            jumpRequested = true;
        }

        HandleDashInput();

        // 2. CALCULATE CAMERA-RELATIVE DIRECTION
        Vector3 camForward = UnityEngine.Camera.main.transform.forward;
        Vector3 camRight = UnityEngine.Camera.main.transform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        moveDirection = (camForward * inputVertical + camRight * inputHorizontal).normalized;

        // 3. HANDLE ROTATION (Visuals update perfectly per frame)
        if (moveDirection != Vector3.zero && !isDashing)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    private void FixedUpdate()
    {
        // 4. APPLY ALL PHYSICS AND MOVEMENTS TOGETHER
        HandleMovement();
        HandleJump();
    }

    public void HandleMovement()
    {
        // Don't override normal movement velocity calculation if currently dashing
        if (isDashing) return;

        // Calculate sprinting modifiers
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = baseSpeed * sprintSpeed;
        }
        else
        {
            speed = baseSpeed;
        }

        // Calculate horizontal velocity targets
        Vector3 targetVelocity = moveDirection * speed;

        // Retain the current falling/rising physics speed
        targetVelocity.y = rb.linearVelocity.y;

        // Apply safely to the rigidbody
        rb.linearVelocity = targetVelocity;
    }

    private void HandleJump()
    {
        if (jumpRequested)
        {
            // Zero out vertical velocity so double jumps (if MaxJumps > 1) feel consistent
            Vector3 currentVel = rb.linearVelocity;
            currentVel.y = 0f;
            rb.linearVelocity = currentVel;

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpsLeft -= 1;

            jumpRequested = false; // Reset the request flag
        }
    }

    private void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Time.time - lastShiftTapTime <= doubleTapTime && canDash)
            {
                StartCoroutine(Dash());
            }
            lastShiftTapTime = Time.time;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float dashTimer = 0f;
        float currentDashSpeed = baseSpeed * DashSpeed;

        // If player isn't moving an axis, dash in the direction they are facing
        Vector3 dashDir = moveDirection != Vector3.zero ? moveDirection : transform.forward;

        while (dashTimer < dashDuration)
        {
            dashTimer += Time.fixedDeltaTime; // Match physics clock loops inside coroutines

            Vector3 dashVelocity = dashDir * currentDashSpeed;
            dashVelocity.y = rb.linearVelocity.y; // Keep gravity working while dashing
            rb.linearVelocity = dashVelocity;

            yield return new WaitForFixedUpdate(); // Wait for next FixedUpdate frame
        }

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Matches your exact ground layer assignment string setup
        if (collision.gameObject.layer == LayerMask.NameToLayer("WhatIsGround"))
        {
            jumpsLeft = MaxJumps;
        }
    }
}