using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour 
{
    InputManager inputManager;

    [HideInInspector]public Transform targetTransform; // object that camera follows
    public Transform cameraPivot; //object the camera uses to pivot
    private Transform cameraTransform; //transform of actual camera object in scene
    private float defaultPosition;
    public LayerMask collisionLayers; // the layers the camera collides with
    private Vector3 cameraFollowVelocity = Vector3.zero;
    public float cameraFollowSpeed = 0.2f;
    private Vector3 cameraVectorPosition;
    
    public float lookAngle; //Camera looking up and down
    public float pivotAngle; //Camera looking left and right
    public float minPivotAngle = -50f;
    public float maxPivotAngle = 35f;
    public float lookSpeed;
    public float pivotSpeed;
    public float cameraCollisionRadius = 0.2f;
    public float cameraCollisionOffset = 0.2f; //How much the camera will jump off objects its colliding with
    public float minimumCollisionOffset = 0.2f; 
    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        cameraTransform = Camera.main.transform;
        defaultPosition = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }
    private void FollowTarget()
    {
        Vector3 targetPosition = Vector3.SmoothDamp(transform.position, targetTransform.position,ref cameraFollowVelocity, cameraFollowSpeed);
        transform.position = targetPosition;
    }

    private void RotateCamera()
    {
        lookAngle += inputManager.cameraInputX * lookSpeed;
        pivotAngle -= inputManager.cameraInputY * pivotSpeed;
        pivotAngle = Mathf.Clamp(pivotAngle, minPivotAngle, maxPivotAngle);
        Vector3 rotation = Vector3.zero;
        rotation.y = lookAngle;
        Quaternion targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }

    private void HandleCameraCollisions()
    {
        float targetPosition = defaultPosition;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast(cameraPivot.transform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetPosition), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position, hit.point);
            targetPosition =-( distance - cameraCollisionOffset);
            
        }

        if (Mathf.Abs(targetPosition) < minimumCollisionOffset)
        {
            targetPosition -= minimumCollisionOffset;
        }

        cameraVectorPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPosition, 0.2f);
        cameraTransform.localPosition = cameraVectorPosition;
    }
}
