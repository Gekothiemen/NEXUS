using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
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
