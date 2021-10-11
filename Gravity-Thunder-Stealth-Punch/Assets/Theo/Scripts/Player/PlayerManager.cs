using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    AnimatorManager animatorManager;
    CameraManager cameraManager;
    InputManager inputManager;
    PlayerLocomotion playerLocomotion;
    PlayerAbilities playerAbilities;
    
    public bool isInteracting;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        playerAbilities = GetComponent<PlayerAbilities>();
        animatorManager = GetComponent<AnimatorManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    // Update is called once per frame
    void Update()
    {
        inputManager.HandleAllInputs();
        playerLocomotion.ToggleMovementModes();
        playerLocomotion.HandleJumping();
        playerAbilities.HandleAllAbilities();
    }

    private void FixedUpdate()
    {
        playerAbilities.HighlightObjects();
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();
        animatorManager.animator.SetBool("IsGrounded", playerLocomotion.isGrounded);
    }
}


