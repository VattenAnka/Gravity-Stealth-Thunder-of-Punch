using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;
    [HideInInspector] public float horizontalInput, verticalInput, moveAmount;
     public float cameraInputX, cameraInputY;
    public Vector3 mousePos;
  
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
      
        cameraInputX = Input.GetAxis("Mouse X");
        cameraInputY = Input.GetAxis("Mouse Y");


        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount);


        //toggle running, walking and sprinting
        if (Input.GetKeyDown(KeyCode.LeftControl)) playerLocomotion.isRunning = !playerLocomotion.isRunning;

        if (playerLocomotion.isRunning) moveAmount = 1f;
        else moveAmount = 0.1f;
    }

    
}
