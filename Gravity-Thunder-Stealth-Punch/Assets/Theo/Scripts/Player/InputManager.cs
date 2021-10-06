using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;
    PlayerAbilities playerAbilities;
    [HideInInspector] public float horizontalInput, verticalInput;
    [HideInInspector] public bool jumpDown, jumpUp;

    public float moveAmount;
    public float cameraInputX, cameraInputY;
    public Vector3 mousePos;
    bool isMoving;
    private void Awake()
    {
        playerAbilities = GetComponent<PlayerAbilities>();
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    public void HandleAllInputs()
    {
        //hide cursor when clicking on screen
        if (Input.GetMouseButtonDown(0))
        {
            animatorManager.PlayTargetAnimation("Punch", false);
            playerAbilities.ForcePush();
            Cursor.visible = false;
        }
       
     
        HandleMovementInput();
        HandleJumpInput();
        //Handle Camera
    }

    private void HandleJumpInput()
    {
        jumpDown = Input.GetButton("Jump");
        jumpUp = Input.GetButtonUp("Jump");
        
            
    }
    private void HandleMovementInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        isMoving = !(horizontalInput == 0 && verticalInput == 0);

        cameraInputX = Input.GetAxis("Mouse X");
        cameraInputY = Input.GetAxis("Mouse Y");
        animatorManager.UpdateAnimatorValues(moveAmount);


        //moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        //toggle running, walking and sprinting
        if (Input.GetKeyDown(KeyCode.LeftControl)) playerLocomotion.isRunning = !playerLocomotion.isRunning;
        playerLocomotion.isSprinting = Input.GetKey(KeyCode.LeftShift);

        if (!playerLocomotion.isSprinting && playerLocomotion.isRunning && isMoving) moveAmount = .6f;
        else if (playerLocomotion.isSprinting && isMoving) moveAmount = 1f;
        else if (!playerLocomotion.isRunning && isMoving) moveAmount = 0.35f;
        else moveAmount = 0;
    }


}
