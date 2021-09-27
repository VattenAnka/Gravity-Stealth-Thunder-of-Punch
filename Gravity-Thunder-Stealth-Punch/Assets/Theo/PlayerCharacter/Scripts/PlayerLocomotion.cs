using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerLocomotion : MonoBehaviour
{
    public Text magnitudeUI;
    
    Vector3 moveDirection;
    Transform cameraObject;


    [HideInInspector] public bool isRunning, isSprinting, isGrounded, isJumping;
   
    [Header("Ground Movement Settings")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float sprintingSpeed;
    [Header("Jump and Fall Settings")]
    [Range(0, 1)] [SerializeField] float inAirControl;
    [SerializeField] float jumpHeight = 3, jumpChargeRate = 10, minJumpHeight, maxJumpHeight, fallingVelocity, maximumVelocity, rayCastRadius = 1f;
    float inAirTimer;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform raySphere;
    

    Rigidbody rb;
    InputManager inputManager;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    

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
        // if (isJumping) return;

        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;
        //  Vector3 gravityVelocity -= gravity * Time.deltaTime;

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

    private void InAirMovement()
    {
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection *= inAirControl;
        moveDirection.y = 0;
        rb.AddForce(moveDirection, ForceMode.VelocityChange);

    }

    private void HandleRotation()
    {
        // if (isJumping) return;

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
        //Gives extra gravity to player if isnt on ground
        if (!isGrounded)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }
            inAirTimer += Time.deltaTime;
            // clamps velocity by pushing player in opposite direction if velocity is above maximum velocity
            /* if (rb.velocity.x >= maximumVelocity || rb.velocity.x <= maximumVelocity|| rb.velocity.z >= maximumVelocity || rb.velocity.z <= maximumVelocity)
             {
                 rb.AddForce(cameraObject.forward * -1, ForceMode.Force);
             }*/
            //adds extra gravity pull to player
            rb.AddForce(Vector3.down * fallingVelocity * inAirTimer);
        }

        //Ground check 
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
            if (inputManager.jumpDown && jumpHeight <= maxJumpHeight)
            {
                jumpHeight += Time.deltaTime * jumpChargeRate;
                Debug.Log(jumpHeight);
            }
            if (inputManager.jumpUp)
            {
                animatorManager.animator.SetBool("IsJumping", true);
                animatorManager.PlayTargetAnimation("Jump", false);
                rb.AddForce(transform.up * jumpHeight);
                jumpHeight = minJumpHeight;
            }
        }
    }






    public void HandleAllMovement()
    {
        magnitudeUI.text = rb.velocity.magnitude.ToString();
        HandleFallingAndLanding();
        HandleRotation();
        if (isGrounded)
        {

            HandleMovement();
        }
        else InAirMovement();


    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawSphere(raySphere.position, rayCastRadius);
    }
}


