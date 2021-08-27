using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputManager : MonoBehaviour
{
    AnimatorManager animatorManager;
    [HideInInspector] public float horizontalInput, verticalInput;
     public float cameraInputX, cameraInputY;
    public Vector3 mousePos;
    float moveAmount;
    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
    }
    public void HandleAllInputs()
    {
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
    }

    
}
