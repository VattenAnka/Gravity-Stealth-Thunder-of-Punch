using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StupidRBTEst_ : MonoBehaviour
{
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (rb.velocity.magnitude < 10)
        {

            rb.velocity += new Vector3(20, rb.velocity.y, 0);
        }
    }
}
