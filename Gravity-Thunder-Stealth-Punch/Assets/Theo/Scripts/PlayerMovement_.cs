using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement_ : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    Rigidbody rb;
    float moveX, moveZ;
    Vector3 movement;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Movement();
    }
    // Update is called once per frame
    void Update()
    {
        PlayerInput();
    }

    public void PlayerInput()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveZ = Input.GetAxisRaw("Vertical");
        movement = new Vector3(moveX, 0, moveZ);
    }
    public void Movement()
    {
        

        rb.velocity += movement * movementSpeed * Time.deltaTime;
        
    }
}
