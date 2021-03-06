using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPull_ : MonoBehaviour
{
    [SerializeField] float pullForce, pullRadius;
    [SerializeField] public bool active;
    PlayerAbilities playerAbilities;
    Collider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        playerAbilities = FindObjectOfType<PlayerAbilities>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (!active) GetObjects();
        if (active)
        {
            Pull();
        }
    }

    public void GetObjects()
    {
        colliders = Physics.OverlapSphere(transform.position, pullRadius);

    }
       
    public void Pull()
    {
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null && collider.tag != "Player")
            {
                rb.AddExplosionForce(-pullForce, transform.position, pullRadius);
            }
        }
    }


    public void Push(float force)
    {
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null && collider.tag != "Player")
            {
                rb.AddForce(Camera.main.transform.forward * force,ForceMode.VelocityChange);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, pullRadius);
    }
}
