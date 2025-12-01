using UnityEngine;

public class DesktopInputController : MonoBehaviour
{
    [Header("References")]
    public Transform cameraPitch;      // Assign CameraPitch object
    public Camera desktopCamera;       // Assign DesktopCamera

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float fastSpeedMultiplier = 2f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 2.0f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        yaw = transform.eulerAngles.y;
        pitch = cameraPitch.localEulerAngles.x;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontal rotation on the rig (yaw)
        yaw += mouseX;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Vertical rotation on the camera pitch object (pitch)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        cameraPitch.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S

        // ⭐ Fly-mode: Move exactly where the camera is looking
        Vector3 moveDir = cameraPitch.forward * vertical + cameraPitch.right * horizontal;

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= fastSpeedMultiplier;

        transform.position += moveDir * speed * Time.deltaTime;
    }
}
