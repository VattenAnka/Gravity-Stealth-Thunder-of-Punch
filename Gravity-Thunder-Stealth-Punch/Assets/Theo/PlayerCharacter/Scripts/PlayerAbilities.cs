using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public Transform grabPoint;
    Camera camera;
    RaycastHit hit;
    public float maxForce;
    float force;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(camera.transform.position, hit.point);

    }

   
    public void ForcePush()
    {

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {

            Debug.Log(hit.collider.gameObject);

            if (hit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                force = 0;
                if (maxForce - Vector3.Distance(hit.point, camera.transform.position)>0) force = maxForce- Vector3.Distance(hit.point, camera.transform.position);
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce((hit.point - camera.transform.position) * force, ForceMode.Impulse);
            }

        }
    }
}

                



