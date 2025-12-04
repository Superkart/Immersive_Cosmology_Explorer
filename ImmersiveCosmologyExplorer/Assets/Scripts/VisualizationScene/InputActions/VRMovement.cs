using UnityEngine;
using UnityEngine.InputSystem;

public class VRLocomotion : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotateSpeed = 60f;

    public CharacterController controller;
    public Transform headTransform;

    private InputActions inputActions;
    private Vector2 moveInput;
    private Vector2 rotateInput;

    private float pitch = 0f;

    [Header("VR UI Toggle")]
    public GameObject vrUIPanel;     // drag your VR panel here
    private InputAction menuButtonAction;  // left controller menu button


    void Awake()
    {
        inputActions = new InputActions();
    }

    void OnEnable()
    {
        inputActions.RightHand.Enable();
        inputActions.LeftHand.Enable();

        inputActions.RightHand.Move.performed += OnMove;
        inputActions.RightHand.Move.canceled += _ => moveInput = Vector2.zero;

        inputActions.LeftHand.Rotate.performed += OnRotate;
        inputActions.LeftHand.Rotate.canceled += _ => rotateInput = Vector2.zero;


        menuButtonAction = inputActions.LeftHand.Menu;   // must exist in InputActions!
        menuButtonAction.Enable();
        menuButtonAction.performed += OnMenuPressed;
    }

    void OnDisable()
    {
        inputActions.RightHand.Move.performed -= OnMove;
        inputActions.LeftHand.Rotate.performed -= OnRotate;

        if (menuButtonAction != null)
            menuButtonAction.performed -= OnMenuPressed;


        inputActions.RightHand.Disable();
        inputActions.LeftHand.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnRotate(InputAction.CallbackContext context)
    {
        rotateInput = context.ReadValue<Vector2>();
    }

    private void OnMenuPressed(InputAction.CallbackContext context)
    {
        if (vrUIPanel == null) return;

        vrUIPanel.SetActive(!vrUIPanel.activeSelf);
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector3 forward = headTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 direction = forward * moveInput.y;

        controller.Move(direction * moveSpeed * Time.deltaTime);
    }

    void HandleRotation()
    {
        // -----------------------------------------
        // YAW (left/right turn)
        // -----------------------------------------
        float yaw = rotateInput.x * rotateSpeed * Time.deltaTime;
        transform.Rotate(0, yaw, 0);

        // -----------------------------------------
        // PITCH (up/down look)
        // -----------------------------------------
        pitch += -rotateInput.y * rotateSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -60f, 60f);

        Vector3 camAngles = headTransform.localEulerAngles;
        camAngles.x = pitch;
        headTransform.localEulerAngles = camAngles;
    }


    private void OnDestroy()
    {
        if(inputActions != null)
        {
            inputActions.Dispose();
        }
    }
}
