using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;
    PlayerAbilities playerAbilities;
    [HideInInspector] public float horizontalInput, verticalInput;
    [HideInInspector] public bool jumpDown, jumpUp, mouseLeftDown, mouseRightDown, leftCtrlDown, leftShift,sneakInput , moveInput;
    Rigidbody rb;

    public float moveAmount;
    public float cameraInputX, cameraInputY;
    public Vector3 mousePos;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerAbilities = GetComponent<PlayerAbilities>();
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    public void HandleAllInputs()
    {
        //hide cursor when clicking on screen
        
        mouseLeftDown = Input.GetMouseButtonDown(0);
        mouseRightDown = Input.GetMouseButtonDown(1);
        if (mouseLeftDown) Cursor.visible = false;
     
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
        
       

        leftCtrlDown = Input.GetKeyDown(KeyCode.LeftControl);
        leftShift = Input.GetKey(KeyCode.LeftShift);
        sneakInput = Input.GetKeyDown(KeyCode.C);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        moveInput = !(horizontalInput == 0 && verticalInput == 0);

        cameraInputX = Input.GetAxis("Mouse X");
        cameraInputY = Input.GetAxis("Mouse Y");
       
    
    }


}


      


