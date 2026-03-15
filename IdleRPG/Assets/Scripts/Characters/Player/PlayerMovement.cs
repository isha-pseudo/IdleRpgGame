
using System.Runtime.ExceptionServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    private CharacterController controller;
    private PlayerData data;
    private Transform camTransform;
    private PlayerInput playerInput;
    private Transform playerVisual;
    private CharacterPhysics physics;
    private CharacterStateMachine stateMachine;

    [Header("Movement Feel")]
    [SerializeField]private float acceleration = 10f;
    [SerializeField]private float deceleration = 25f;
    [SerializeField]private float jumpPower = 12f;
    [SerializeField]private float coyoteTime = 0.15f;
    [SerializeField]private float jumpBufferTime = 0.2f;

     private Vector2 moveInput;
    private Vector3 lastMoveDirection = Vector3.zero;
    public float currentSpeed = 0f;
    private float rotationSpeed = 12f;

    private float coyoteTimeCounter = 0f;
    private float jumpBufferCounter = 0f;

    void Start()
    {
        // set the components getting read //
        controller = GetComponent<CharacterController>();
        data = GetComponent<PlayerData>();
        playerInput = GetComponent<PlayerInput>();
        camTransform = Camera.main.transform;
        playerVisual = transform.Find("PlayerVisual").transform;
        physics = GetComponent<CharacterPhysics>();
        stateMachine = GetComponent<CharacterStateMachine>();


        // errors //
        if (controller == null) Debug.LogError("Player movement is Missing Character Controller!");
        if (data == null) Debug.LogError("Player movemement is Missing PlayerData on player!");
        if (playerInput == null) Debug.LogError("Player movement cannot find player input");
        if (physics == null) Debug.LogError("Player movement cannot find the physics component");
        if (stateMachine == null) Debug.Log("Player Movement cannot find Character state machine");

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // read the input value //
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {  
        if (context.performed)
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if ((coyoteTimeCounter > 0f || controller.isGrounded) && jumpBufferCounter > 0f)
        {
            physics.verticalVelocity = jumpPower;
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
        }
    }
    
    void Update()
    {
        Vector3 movement = lastMoveDirection * currentSpeed;
        physics.ApplyMovement(movement);

        // Get input strength //
        float inputStrength = moveInput.magnitude;

        // Logic for dictating target speed //
        float targetSpeed = 0f;
        if (inputStrength > 0.01f)
        {
            // check if walk or run //
            float multiplier = (inputStrength > 0.4f) ? data.jogMultiplier : data.walkMultiplier;
            // set the target speed by multiplying base speed //
            targetSpeed = data.trueSpeed * multiplier;

            Vector3 camForward = camTransform.forward;
            Vector3 camRight = camTransform.right;
            camForward.y = 0f;
            camRight.y = 0f;
            camForward.Normalize();
            camRight.Normalize();
            lastMoveDirection = (camForward * moveInput.y + camRight * moveInput.x).normalized;
        }
        // Ramp for speed change //
        float smoothRate = (targetSpeed > currentSpeed) ? acceleration : deceleration;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, smoothRate * Time.deltaTime);

        if (currentSpeed < 0.01f) currentSpeed = 0f;

        if (stateMachine.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpBufferCounter = 0f;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        //== State Machine Sync ==//
        stateMachine.isGrounded = controller.isGrounded;

        if (stateMachine.isGrounded)
        {
            float walkThreshold = data.trueSpeed * data.walkMultiplier;
            float jogThreshold = data.trueSpeed * data.jogMultiplier;
            
            if (currentSpeed < 0.01f) stateMachine.currentState = CharacterState.Idle;
            else if (currentSpeed <= walkThreshold) stateMachine.currentState = CharacterState.Walking;
            else if (currentSpeed <= jogThreshold) stateMachine.currentState = CharacterState.Running;
            else stateMachine.currentState = CharacterState.Idle;
        }
        else
        {
            stateMachine.currentState = CharacterState.Airborne;
        }

        if (jumpBufferCounter > 0f) jumpBufferCounter -= Time.deltaTime;
        if (lastMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastMoveDirection);
            playerVisual.rotation = Quaternion.Slerp(playerVisual.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        

    }


}
