using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceProjectile_ : MonoBehaviour
{
    [SerializeField] public float speed, force, radius, powerDrainRate;

    Rigidbody rb;
    float destroyTime;
    float distance;
    float currentSpeed;
    private void Awake()
    {
        
        distance = FindObjectOfType<PlayerMovement_>().distance;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log(this);
        // 
    }

    // Update is called once per frame
    void Update()
    {
        /*Vector3 move = transform.forward * speed * Time.deltaTime;
        transform.Translate(move);*/
        if (force > 0)
        {
            force -= powerDrainRate*4;
        }
        if (radius > 0)
        {
            radius -= powerDrainRate * Time.deltaTime;
        }
        if (transform.localScale.x > 0 && transform.localScale.y >0)
        {
            transform.localScale = new Vector3(transform.localScale.x -powerDrainRate * Time.deltaTime,transform.localScale.y - powerDrainRate * Time.deltaTime, transform.localScale.z);
        }


    }

    private void FixedUpdate()
    {
        rb.velocity = transform.forward * speed * Time.deltaTime;
        currentSpeed = rb.velocity.magnitude;
        destroyTime = distance / currentSpeed;
        Debug.Log(destroyTime);
        Destroy(gameObject, destroyTime);
        Push();
    }
    public void Push()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null && collider.tag != "Player")
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }
        Debug.Log(force);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "PLayer")
        {
            Debug.Log("WAH");
            Debug.Log(other.gameObject);
        //Destroy(this.gameObject);
            //Push();
        }
    }
    /*private void OnTriggerStay(Collider other)
    {
        Push();
        Destroy(this.gameObject);
    }*/


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, radius);
    }

}
