using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement_ : MonoBehaviour
{
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    Transform cam;

    [SerializeField] float movementSpeed;
    Rigidbody rb;
    float moveX, moveZ;
    Vector3 movement;
    [SerializeField] MovementState movementState;
    bool isGrounded;

    [SerializeField] GameObject standardCam, aimCam;
    bool aimPosRight = true;
    [SerializeField] Transform aimTarget;
    Vector3 aimTargetStartPos;

    [SerializeField] GameObject forceProjectile;
    [SerializeField] Transform projectileSpawnPoint;
    float destroyTime, speed, startDistance;
    [SerializeField] public float distance;
    // Start is called before the first frame update
    void Start()
    {
        startDistance = distance;
        aimTargetStartPos = aimTarget.localPosition;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
    }

    public enum MovementState
    {
        Standard, Aim
    }

    private void FixedUpdate()
    {
        // Movement();

        switch (movementState)
        {
            case MovementState.Standard:
                PlayerStandardMovement();
                break;

            case MovementState.Aim:
                PlayerAimMovement();
                if (aimPosRight)
                {
                    aimTarget.localPosition = aimTargetStartPos;
                }
                else aimTarget.localPosition = new Vector3(-aimTargetStartPos.x, aimTargetStartPos.y, aimTargetStartPos.z);
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        aimCam.SetActive(movementState == MovementState.Aim);
        standardCam.SetActive(movementState == MovementState.Standard);
        Shoot();
        ChangeMode();
        //PlayerInput();
    }


    private void Shoot()
    {

        if (Input.GetMouseButton(0))
        {
            distance += 20 * Time.deltaTime;

        }
        if (Input.GetMouseButtonUp(0))
        {

            GameObject newProj = Instantiate(forceProjectile, projectileSpawnPoint.position, projectileSpawnPoint.rotation) as GameObject;
            newProj.transform.forward = projectileSpawnPoint.forward;
            speed = newProj.GetComponent<ForceProjectile_>().speed;
            distance = startDistance;
        }

    }

    public void ChangeMode()
    {
        if (Input.GetMouseButton(1) && isGrounded)
        {
            movementState = MovementState.Aim;
        }
        else movementState = MovementState.Standard;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            aimPosRight = !aimPosRight;
        }
    }

    public void PlayerStandardMovement()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;


        if (direction.magnitude >= 0.1f)
        {


            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            // Vector3 move = new Vector3(moveDir.x, rb.velocity.y, moveDir.z);
            rb.velocity = moveDir.normalized * movementSpeed * Time.deltaTime;
            //rb.AddForce(moveDir.normalized * movementSpeed * Time.deltaTime,ForceMode.Force);


        }

    }
    public void PlayerAimMovement()
    {
        //transform.rotation = Quaternion.Euler(0, cam.transform.rotation.y, 0);//cam.rotation;
        // transform.forward = new Vector3(0, cam.transform.rotation.y , 0);
        var playerRotation = cam.transform.rotation;
        playerRotation.x = 0;
        playerRotation.z = 0;
        transform.rotation = playerRotation;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        Vector3 movementFor = transform.forward * vertical * movementSpeed;
        Vector3 movementHor = transform.right * horizontal * movementSpeed;

        rb.velocity = (movementFor + movementHor) * Time.deltaTime;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * distance);

    }

}
