    using UnityEngine;
    using UnityEngine.InputSystem;
public class CharacterController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveInput;
    [SerializeField] private float jumpForce = 100;
    [SerializeField] private float moveSpeed = 50f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            rb.AddForce(new Vector3(0, jumpForce,0));
            Debug.Log("jumped");
        }
    }
    public void OnMovement(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        Debug.Log("jumped");

    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log("Current move input: " + moveInput);
        rb.linearVelocity = new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, moveInput.y * moveSpeed);
    }

    public void OnSprint(InputAction.CallbackContext ctx)
}
