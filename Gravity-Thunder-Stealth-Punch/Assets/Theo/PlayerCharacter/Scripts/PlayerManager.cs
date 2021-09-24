using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    AnimatorManager animatorManager;
    CameraManager cameraManager;
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    
    public bool isInteracting;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        animatorManager = GetComponent<AnimatorManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    // Update is called once per frame
    void Update()
    {
        inputManager.HandleAllInputs();
        playerLocomotion.HandleJumping();
    }

    private void FixedUpdate()
    {

        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
        isInteracting = animatorManager.animator.GetBool("IsInteracting");
        playerLocomotion.isJumping = animatorManager.animator.GetBool("IsJumping");
        animatorManager.animator.SetBool("IsGrounded",playerLocomotion.isGrounded);
    }
}
