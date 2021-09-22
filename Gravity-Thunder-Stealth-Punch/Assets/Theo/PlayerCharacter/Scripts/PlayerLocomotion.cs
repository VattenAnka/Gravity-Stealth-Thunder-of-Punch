using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    Vector3 moveDirection;
    Transform cameraObject;

    public float inAirTimer, rayCastHeightOffset = 0.5f, rayCastRadius = 1f;

    [HideInInspector] public bool isRunning, isSprinting;
    public bool isGrounded, isJumping;
    [SerializeField] float rotationSpeed, walkingSpeed, runningSpeed, sprintingSpeed, leapingVelocity, fallingVelocity;
    public LayerMask groundLayer;
    Rigidbody rb;
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    public Vector3 rayCastOrigin;
    public Transform raySphere;

    public float gravityIntensity = -15, jumpHeight = 3;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerManager = GetComponent<PlayerManager>();
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }


    private void HandleMovement()
    {
        if (isJumping)
            return;
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        //If sprinting we select sprinting speed
        //If we are walking we select walking speed
        //If we are running we will select running speed
        //If we are sneaking we will select sneaking speed

        //Switch between movement speeds
        if (inputManager.moveAmount >= 0.5f && inputManager.moveAmount < 0.8f) moveDirection *= runningSpeed;
        else if (inputManager.moveAmount >= 0.8f) moveDirection *= sprintingSpeed;
        else moveDirection *= walkingSpeed;


        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        if (isJumping)
            return;
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * Input.GetAxis("Vertical");
        targetDirection += cameraObject.right * Input.GetAxis("Horizontal");
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }
    private void HandleFallingAndLanding()
    {
        RaycastHit hitInfo;


        if (!isGrounded&& !isJumping)
        {
              if (!playerManager.isInteracting)
              {
                  animatorManager.PlayTargetAnimation("Falling", true);
              }
            inAirTimer += Time.deltaTime;
            rb.AddForce(transform.forward * leapingVelocity);
            rb.AddForce(Vector3.down * fallingVelocity * inAirTimer);
        }


        if (Physics.CheckSphere(raySphere.position, rayCastRadius, groundLayer))
        {
            if (!isGrounded && !playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Land", true);

            }
            inAirTimer = 0;
            
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    public void HandleJumping()
    {
        if (isGrounded)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);
            float jumpingVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight);
            Vector3 playerVelocity = moveDirection;
            playerVelocity.y = jumpingVelocity;
           // rb.velocity = playerVelocity;
            rb.AddForce(playerVelocity);
        }
    }
    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if (!isGrounded)
            return;
        HandleMovement();
        HandleRotation();
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawSphere(raySphere.position, rayCastRadius);
    }
}


