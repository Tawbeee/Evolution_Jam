using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementTutorial : MonoBehaviour
{

    [Header("Wwise Events")] public AK.Wwise.Event myFootstep;
    
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [HideInInspector] public float walkSpeed;
    [HideInInspector] public float sprintSpeed;

    [Header("Gravity")]
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Animation")]
    public Animator animator;
    public float rotationSpeed = 10f; // Speed at which character rotates to face direction
    public float jumpUpDuration = 0.5f; // How long to show JumpUp animation before switching to MidAir

    public Transform orientation;

    Vector2 moveInput;
    Vector3 moveDirection;

    Rigidbody rb;

    private InputAction moveAction;
    private InputAction jumpAction;

    public PlayerInput playerInput;

    [SerializeField] private CapsuleCollider capsuleCollider;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [SerializeField] private float maxFallHeight = 10f; // hauteur maximale tolérée
    private float lastGroundY;
    private bool isFalling;

    [Header("Audio")]
    [SerializeField] private float footstepInterval = 0.5f; // Time between footsteps
    private float lastFootstepTime;

    // Animation states
    private bool isIdle;
    private bool isRunning;
    private bool isJumpUp;
    private bool isMidAir;
    
    // Jump animation timing
    private float jumpStartTime;
    private bool hasJumped;

    private void Awake()
    {
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    private void Update()
    {
        grounded = IsGrounded();
        MyInput();
        SpeedControl();
        UpdateAnimationStates();
        HandleRotation();

        // Handle drag
        rb.linearDamping = grounded ? groundDrag : 0;

        // Appliquer un boost de gravité quand on tombe
        if (rb.linearVelocity.y < 0)
        {
            rb.AddForce(Vector3.up * Physics.gravity.y * (fallMultiplier - 1), ForceMode.Acceleration);
        }
        // Raccourcir le saut si on relâche le bouton avant le sommet
        else if (rb.linearVelocity.y > 0 && !jumpAction.IsPressed())
        {
            rb.AddForce(Vector3.up * Physics.gravity.y * (lowJumpMultiplier - 1), ForceMode.Acceleration);
        }

        if (IsGrounded())
        {
            if (isFalling)
            {
                float fallDistance = lastGroundY - transform.position.y;
                if (fallDistance > maxFallHeight)
                {
                    PlayerEvents.Kill(KillingObjectType.Gravity);
                }
            }

            // Réinitialiser la hauteur du sol quand on est au sol
            lastGroundY = transform.position.y;
            isFalling = false;
        }
        else
        {
            if (!isFalling)
            {
                lastGroundY = transform.position.y;
                isFalling = true;
            }
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (jumpAction.triggered && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);

            // Check for footsteps - only when actively moving with input
            bool hasMovementInput = moveInput.magnitude > 0.1f;
            Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            bool isActuallyMoving = horizontalVelocity.magnitude > 0.1f;
            
            if (hasMovementInput && isActuallyMoving && Time.time - lastFootstepTime >= footstepInterval)
            {
                myFootstep.Post(gameObject);
                lastFootstepTime = Time.time;
            }
        }
        else
            rb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // Reset Y velocity and apply jump force
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
        // Track jump timing for animation
        jumpStartTime = Time.time;
        hasJumped = true;
        
        // Immediately set JumpUp animation state
        isJumpUp = true;
        isMidAir = false;
        isIdle = false;
        isRunning = false;
        
        if (animator != null)
        {
            animator.SetBool("isJumpUp", true);
            animator.SetBool("isMidAir", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isRunning", false);
            Debug.Log("Jump triggered - Setting isJumpUp to true");
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private bool IsGrounded()
    {
        Vector3 point1 = transform.position + capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);
        Vector3 point2 = transform.position + capsuleCollider.center - Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius);

        return Physics.CapsuleCast(point1, point2, capsuleCollider.radius * 0.95f, Vector3.down, out RaycastHit hit, groundCheckDistance, groundLayer);
    }

    private void UpdateAnimationStates()
    {
        if (hasJumped && Time.time - jumpStartTime < 0.1f)
        {
            return;
        }
        
        // Determine current state
        bool hasMovementInput = moveInput.magnitude > 0.1f;
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        bool isMoving = horizontalVelocity.magnitude > 0.1f;

        if (!grounded)
        {
            if (hasJumped && Time.time - jumpStartTime < jumpUpDuration)
            {
                isJumpUp = true;
                isMidAir = false;
            }
            else
            {
                isJumpUp = false;
                isMidAir = true;
            }
            
            isIdle = false;
            isRunning = false;
        }
        else
        {
            hasJumped = false;
            isJumpUp = false;
            isMidAir = false;
            
            isRunning = hasMovementInput && isMoving;
            isIdle = !isRunning;
        }

        if (animator != null)
        {
            animator.SetBool("isIdle", isIdle);
            animator.SetBool("isRunning", isRunning);
            animator.SetBool("isJumpUp", isJumpUp);
            animator.SetBool("isMidAir", isMidAir);
        }
    }

    private void HandleRotation()
    {
        if (moveInput.magnitude > 0.1f)
        {
            Vector3 targetDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
            
            if (targetDirection.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}
