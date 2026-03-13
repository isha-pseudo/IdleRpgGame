
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

    [Header("Movement Feel")]
    [SerializeField]private float acceleration = 10f;
    [SerializeField]private float deceleration = 25f;
    [SerializeField]private float gravityMultiplier = 2.5f;
    [SerializeField]private float jumpPower = 12f;
    [SerializeField]private float coyoteTime = 0.15f;
    [SerializeField]private float jumpBufferTime = 0.2f;

     private Vector2 moveInput;
    private Vector3 lastMoveDirection = Vector3.zero;
    public float currentSpeed = 0f;
    private float rotationSpeed = 12f;
    private float verticalVelocity = 0f;
    private bool isJumping = false;
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

        // errors //
        if (controller == null) Debug.LogError("Missing Character Controller!");
        if (data == null) Debug.LogError("Missing PlayerData on player!");
        if (playerInput == null) Debug.LogError("Missing player input");


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
            verticalVelocity = jumpPower;
            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            isJumping = true;
        }
    }
    
    void Update()
    {

        // Get input strength //
        float inputStrength = moveInput.magnitude;

        // Logic for dictating target speed //
        float targetSpeed = 0f;
        if (inputStrength > 0.01f)
        {
            // check if walk or run //
            float multiplier = (inputStrength > 0.4f) ? data.jogMultiplier : data.walkMultiplier;
            // set the target speed by multiplying base speed //
            targetSpeed = data.baseSpeed * multiplier;

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

        verticalVelocity += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -1f;
        }
        if (controller.isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpBufferCounter = 0f;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f) jumpBufferCounter -= Time.deltaTime;

        
        Vector3 movement = lastMoveDirection * currentSpeed * Time.deltaTime;
        movement.y = verticalVelocity * Time.deltaTime;
        controller.Move(movement);

        if (lastMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastMoveDirection);
            playerVisual.rotation = Quaternion.Slerp(playerVisual.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }


}
