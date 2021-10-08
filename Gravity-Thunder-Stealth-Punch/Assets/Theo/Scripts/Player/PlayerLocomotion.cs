using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerLocomotion : MonoBehaviour
{

    Vector3 moveDirection;
    Transform cameraObject;



    [SerializeField] MoveMode currentMoveMode;
    [SerializeField] Vector3 gravity;
    [SerializeField] Transform direction;

    [HideInInspector] public bool isRunning, isSprinting, isGrounded, isJumping;

    [Header("Ground Movement Settings")]
    [SerializeField] float rotationSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float sprintingSpeed;

    [Header("Jump and Fall Settings")]
    [SerializeField] float inAirSpeed;
    [Range(0, 20)] [SerializeReference] float groundDrag;
    [Range(0, 20)] [SerializeReference] float airDrag;
    [Range(0, 20)] [SerializeReference] float fallDrag;

    [SerializeField] float jumpHeight = 3, jumpChargeRate = 10, minJumpHeight, maxJumpHeight, maximumVelocity, rayCastRadius = 1f;
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

    public enum MoveMode
    {
        walking, running, sprinting, inAirMovement
    }

    public void ToggleMovementModes()
    {
        //Toggle ctrl to run
        if (inputManager.leftCtrlDown) isRunning = !isRunning;

        //Hold shift to sprint
        if (inputManager.leftShift) isSprinting = true;
        else isSprinting = false;

        if (isGrounded)
        {
            if (isRunning && !isSprinting) currentMoveMode = MoveMode.running;
            else if (isSprinting) currentMoveMode = MoveMode.sprinting;
            else if (!isRunning) currentMoveMode = MoveMode.walking;
        }
        else currentMoveMode = MoveMode.inAirMovement;
    }




    private float MovementModes()
    {
        float moveSpeed = 0;
        
        switch (currentMoveMode)
        {
            case MoveMode.walking:
                moveSpeed = walkSpeed;
                break;
            case MoveMode.running:
                moveSpeed = runningSpeed;
                break;
            case MoveMode.sprinting:
                moveSpeed = sprintingSpeed;
                break;
            case MoveMode.inAirMovement:
                moveSpeed = inAirSpeed;
                break;
        }
        if(inputManager.isMoving)animatorManager.UpdateWalkBlendValue(moveSpeed/sprintingSpeed);
        else animatorManager.UpdateWalkBlendValue(0);
        return moveSpeed;
    }
                
        




    private void HandleMovement()
    {

        moveDirection = direction.forward * inputManager.verticalInput;
        moveDirection += direction.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        moveDirection *= MovementModes();
       
        Vector3 movementVelocity = moveDirection;
        // rb.velocity = movementVelocity;
        rb.AddForce(movementVelocity, ForceMode.VelocityChange);
    }

    private void HandleRotation()
    {
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
    private void HandleFallingAndGroundCheck()
    {
        //Gives gravity to player if isnt on ground

        inAirTimer += Time.deltaTime;
        rb.AddForce(gravity , ForceMode.Acceleration);
        animatorManager.UpdateJumpBlendValue(rb.velocity.y);
        //Ground check 
        if (Physics.CheckSphere(raySphere.position, rayCastRadius, groundLayer))
        {
            rb.drag = groundDrag;
            inAirTimer = 0;
            isGrounded = true;
        }
        else
        {
            rb.drag = airDrag;
           
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
                rb.AddForce(transform.up * jumpHeight,ForceMode.Impulse);
                jumpHeight = minJumpHeight;
            }
        }
    }
    public void HandleAllMovement()
    {

        direction.transform.forward = new Vector3(cameraObject.transform.forward.x, 0, cameraObject.transform.forward.z);

        HandleFallingAndGroundCheck();
        HandleRotation();
        HandleMovement();
    }

    private void OnDrawGizmos()
    {

        Gizmos.DrawSphere(raySphere.position, rayCastRadius);
    }




}

