using UnityEngine;

public class Stairs_Handler : MonoBehaviour
{
    Rigidbody rb;

    [SerializeField] GameObject stepRayUpper;

    [SerializeField] GameObject stepRayLower;

    [SerializeField] float stepHeight  = 0.3f;

    [SerializeField] float stepSmooth  = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        stepClimb();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        stepRayUpper.transform.position = new Vector3(stepRayUpper.transform.position.x, stepHeight, stepRayUpper.transform.position.z);
        Debug.Log("stepclimbed");
    }

    private void stepClimb()
    {
        bool steppedUp = false;

        // Forward Raycast
        RaycastHit hitLower;
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(Vector3.forward), out hitLower, 0.1f))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(Vector3.forward), out hitUpper, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
                steppedUp = true;
            }
        }

        // 45-degree Raycast
        RaycastHit hitLower45;
        if (!steppedUp && Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitLower45, 0.1f))
        {
            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(1.5f, 0, 1), out hitUpper45, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
                steppedUp = true;
            }
        }

        // -45-degree Raycast
        RaycastHit hitLowerminus45;
        if (!steppedUp && Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitLowerminus45, 0.1f))
        {
            RaycastHit hitUpperminus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, transform.TransformDirection(-1.5f, 0, 1), out hitUpperminus45, 0.2f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
                steppedUp = true;
            }
        }

        if (steppedUp)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.position += new Vector3(0f, stepSmooth, 0f);
            Debug.Log($"Player teleported up at position: {transform.position}");
        }
    }
}
