using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    Vector3 moveDirection;
    Transform cameraObject;
    [HideInInspector] public bool isRunning, isSprinting;
  
    [SerializeField] float  rotationSpeed, walkingSpeed, runningSpeed, sprintingSpeed;
    Rigidbody rb;
    InputManager inputManager;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }
        
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void HandleMovement(){
        moveDirection = cameraObject.forward * inputManager.verticalInput;
        moveDirection += cameraObject.right * inputManager.horizontalInput;
        moveDirection.Normalize();
        moveDirection.y = 0;

        //If sprinting we select sprinting speed
        //If we are walking we select walking speed
        //If we are running we will select running speed
        //If we are sneaking we will select sneaking speed
        

        if (inputManager.moveAmount >= 0.5f ) moveDirection *= runningSpeed;
        else moveDirection *= walkingSpeed;

        

        Vector3 movementVelocity = moveDirection;
        rb.velocity = movementVelocity;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;
        targetDirection = cameraObject.forward * Input.GetAxis("Vertical");
        targetDirection += cameraObject.right* Input.GetAxis("Horizontal");
        targetDirection.Normalize();
        targetDirection.y = 0;

        if(targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }
}
