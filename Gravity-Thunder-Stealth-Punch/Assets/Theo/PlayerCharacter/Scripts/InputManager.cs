using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;
    [HideInInspector] public float horizontalInput, verticalInput ;
    public float moveAmount;
     public float cameraInputX, cameraInputY;
    public Vector3 mousePos;
    bool isMoving;
    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    public void HandleAllInputs()
    {
        //hide cursor when clicking on screen
        if (Input.GetMouseButtonDown(0)) Cursor.visible = false;
        
        HandleMovementInput();
        //Handlejumpinput
        //Handle Camera
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

        if (!playerLocomotion.isSprinting&&playerLocomotion.isRunning && isMoving) moveAmount = .6f;
        else if (playerLocomotion.isSprinting && isMoving) moveAmount = 1f;
        else if (!playerLocomotion.isRunning && isMoving) moveAmount = 0.35f;
        else moveAmount = 0;
    }

    
}
