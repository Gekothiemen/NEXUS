using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class Camera : MonoBehaviour
{
    [SerializeField] private CinemachineCamera vCam;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minFOV = 20f;
    [SerializeField] private float maxFOV = 60f;

    [SerializeField]
    private Transform followTarget;

    [SerializeField]
    private float rotationalSpeed = 30f;

    [SerializeField]
    private float BottomClamp = -40f;

    [SerializeField]
    private float TopClamp = 70f;

    Rigidbody rb;

    private float cinemachineTargetPitch;
    private float cinemachineTargetYaw;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
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

        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0)
        {
            // Scrolling up lowers FOV (zooms in), scrolling down raises FOV (zooms out)
            float targetFOV = vCam.Lens.FieldOfView - (scrollInput * zoomSpeed);
            vCam.Lens.FieldOfView = Mathf.Clamp(targetFOV, minFOV, maxFOV);
        }
    }





    private void LateUpdate()
    {
        CameraLogic();
    }
    private float getMouseInput(string axis)
    {
        return Input.GetAxis(axis) * rotationalSpeed * Time.deltaTime;
    }
    private void CameraLogic()
    {
        float mouseX = getMouseInput("Mouse X");
        float mouseY = getMouseInput("Mouse Y");

        cinemachineTargetPitch = UpdateRotation(cinemachineTargetPitch, mouseY, BottomClamp, TopClamp, true);
        cinemachineTargetYaw = UpdateRotation(cinemachineTargetYaw, mouseX, float.MinValue, float.MaxValue, false);

        ApplyRotations(cinemachineTargetPitch, cinemachineTargetYaw);
    }

    private void ApplyRotations(float pitch, float yaw)
    {
        // Apply BOTH pitch (up/down) and yaw (left/right) strictly to the camera target!
        // This allows the camera to orbit freely 360 degrees around the player.
        followTarget.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }


    private float UpdateRotation(float currentRotation, float input, float min, float max, bool isXAxis)
    {
        currentRotation += isXAxis ? -input : input;
        return Mathf.Clamp(currentRotation, min, max);
    }
}
