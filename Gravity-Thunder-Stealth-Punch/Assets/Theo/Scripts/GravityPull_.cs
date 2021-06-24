using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPull_ : MonoBehaviour
{
    [SerializeField] float pullForce, pullRadius;
    [SerializeField] bool active;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (active)
        {
            Pull();
        }
    }
    public void Pull()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, pullRadius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(-pullForce, transform.position, pullRadius);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255,0,0.1f);
        Gizmos.DrawSphere(transform.position, pullRadius);
    }
}
