using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkVelocity = 10.0f;
    [SerializeField] float rotationSpeed = 10.0f;
    [SerializeField] float maxSpeed;
    [SerializeField] LayerMask mouseAimMask;

    [SerializeField] Camera cam;
    [SerializeField] CinemachineVirtualCamera followCam; // maybe not needed?
    private PlayerInput playerInput;
    private AnimationController animationController;
    private CharacterController characterController;

    private Vector3 movement;
    private Vector3 aim;
    private bool movementPressed;
    private bool aimPressed;
    private float mouseAimResetDelay = 2f;

    private void Awake()
    {
        animationController = GetComponent<AnimationController>();
        characterController = GetComponent<CharacterController>();
        playerInput = new PlayerInput();
    }

    private void Update()
    {
        Rotate();
        animationController.HandleAnimation(movement, aim);
        // characterController.SimpleMove(movement * maxSpeed * Time.deltaTime);
        characterController.SimpleMove(movement * maxSpeed);
    }

    private void OnMovement(InputAction.CallbackContext ctx)
    {
        Vector2 movementInput = ctx.ReadValue<Vector2>();
        movement = new Vector3(movementInput.x, 0, movementInput.y);
        movementPressed = (movement.x != 0) || (movement.z != 0);
    }

    private void Rotate()
    {
        if (!aimPressed && !movementPressed) return;

        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = GetTargetRotation();
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private Quaternion GetTargetRotation()
    {
        Vector3 direction = movement;
        Vector3 target = new Vector3(direction.x, 0, direction.z);
        Quaternion targetRotation = Quaternion.LookRotation(target);
        return targetRotation;
    }
    
    void OnEnable()
    {
        playerInput.CharacterControls.Enable();

        playerInput.CharacterControls.Movement.performed += OnMovement;
        playerInput.CharacterControls.Movement.canceled += OnMovement;
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();

        playerInput.CharacterControls.Movement.performed -= OnMovement;
        playerInput.CharacterControls.Movement.canceled -= OnMovement;
    }
}
