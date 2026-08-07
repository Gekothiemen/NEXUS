using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;


public class Playermovement : MonoBehaviour
{
    public float rotationSpeed = 720f;

    [SerializeField] InputAction jump;

    [SerializeField] int MaxJumps = 1;

    [SerializeField] int jumpsLeft = 1;

    [SerializeField] float jumpForce = 5f;

    Rigidbody rb;

    [SerializeField] private float baseSpeed = 5f;

    [SerializeField] private float speed;

    [SerializeField] private float sprintSpeed = 1.4f;

    [SerializeField] private LayerMask groundLayer;

    private Vector3 moveDirection;
    private float rotationY;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
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


    private void FixedUpdate()
    {
        
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

        speed = baseSpeed;

        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = baseSpeed * sprintSpeed;
            transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        }
    }
    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsLeft > 0 )
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpsLeft -= 1;
        }
        else if (Input.GetKey(KeyCode.Space) && jumpsLeft == 0)
        {
            Debug.Log("ya cant jump mate");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WhatIsGround"))
        {
            jumpsLeft = MaxJumps;
        }
    }




}
