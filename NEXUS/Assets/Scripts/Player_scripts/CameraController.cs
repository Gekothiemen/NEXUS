using Unity.Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineCamera vCam;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 60f;

    [SerializeField] private Transform followTarget;

    [Header("Rotation Settings")]
    [SerializeField] private float rotationalSpeed = 3f; // Reduced value (Time.deltaTime is removed from calculation loop)
    [SerializeField] private float BottomClamp = -40f;
    [SerializeField] private float TopClamp = 70f;

    private float cinemachineTargetPitch;
    private float cinemachineTargetYaw;

    // Cache inputs per frame
    private float cachedMouseX;
    private float cachedMouseY;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Initialize targets with current starting rotation to prevent camera snaps on spawn
        if (followTarget != null)
        {
            cinemachineTargetYaw = followTarget.eulerAngles.y;
            cinemachineTargetPitch = followTarget.eulerAngles.x;
        }
    }

    void Awake()
    {
        if (vCam == null)
        {
            vCam = GetComponent<CinemachineCamera>();
        }
    }

    void Update()
    {
        if (vCam == null) return;

        // 1. Handle Zoom
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            float targetFOV = vCam.Lens.FieldOfView - (scrollInput * zoomSpeed);
            vCam.Lens.FieldOfView = Mathf.Clamp(targetFOV, minFOV, maxFOV);
        }

        // 2. Gather Raw Mouse Input in Update (Crucial for smooth tracking)
        cachedMouseX = Input.GetAxisRaw("Mouse X") * rotationalSpeed;
        cachedMouseY = Input.GetAxisRaw("Mouse Y") * rotationalSpeed;
    }

    private void LateUpdate()
    {
        if (followTarget == null) return;

        CameraLogic();
    }

    private void CameraLogic()
    {
        // 3. Process accumulated rotation variables
        cinemachineTargetPitch -= cachedMouseY; // Invert Y directly
        cinemachineTargetYaw += cachedMouseX;

        // Clamp the values safely
        cinemachineTargetPitch = Mathf.Clamp(cinemachineTargetPitch, BottomClamp, TopClamp);

        // Reset Yaw wrap arounds so the float numbers don't scale infinitely
        if (cinemachineTargetYaw < -360f) cinemachineTargetYaw += 360f;
        if (cinemachineTargetYaw > 360f) cinemachineTargetYaw -= 360f;

        // 4. Apply rotations directly to the tracking target anchor
        followTarget.rotation = Quaternion.Euler(cinemachineTargetPitch, cinemachineTargetYaw, 0f);

        // 5. Clear inputs so they don't drift if the player stops moving the mouse
        cachedMouseX = 0f;
        cachedMouseY = 0f;
    }
}