using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeBoost : MonoBehaviour
{
    [SerializeField] float forceUp, forceForward;
  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            rb.AddRelativeForce(new Vector3(0, 1 * forceUp, 1 * forceForward), ForceMode.Impulse);

           
        }
    }
}
