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

    private bool uiMode = false;

    void Start()
    {
        LockCursor();
        yaw = transform.eulerAngles.y;
        pitch = cameraPitch.localEulerAngles.x;
    }

    void Update()
    {

        HandleToggleUiMode();

        // If UI mode is on → DO NOT move or rotate camera
        if (uiMode)
            return;

        HandleMouseLook();
        HandleMovement();
    }

    // -------------------------------------------------------------
    // MOUSE LOOK
    // -------------------------------------------------------------
    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Horizontal rotation (yaw)
        yaw += mouseX;
        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        // Vertical rotation (pitch)
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
        cameraPitch.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }

    // -------------------------------------------------------------
    // MOVEMENT (fly-style movement)
    // -------------------------------------------------------------
    void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S

        // Full free-fly movement following camera direction
        Vector3 moveDir = cameraPitch.forward * vertical + cameraPitch.right * horizontal;

        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= fastSpeedMultiplier;

        transform.position += moveDir.normalized * speed * Time.deltaTime;
    }

    // -------------------------------------------------------------
    // UI MODE TOGGLE
    // -------------------------------------------------------------

    void HandleToggleUiMode()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiMode = !uiMode;
            if (uiMode)
                UnlockCursor();
            else
                LockCursor();
        }
    }
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
