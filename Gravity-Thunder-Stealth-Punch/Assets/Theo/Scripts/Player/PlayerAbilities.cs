using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [SerializeField] private float grabPushForce;
    public Transform grabPoint;
    Camera camera;
    RaycastHit hit;
    public float maxForce;
    float force;
    public float forwardOffset;
    public GameObject jet;
    public Transform feetPos;
    bool grab;
    [SerializeField] KeyCode forceGrab, grabTowards, grabAway;
    Collider lastHitCollider;
    Collider currentHitCollider;
    GravityPull_ gravitySphere;
    InputManager inputManager;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        gravitySphere = grabPoint.GetComponent<GravityPull_>();
        camera = Camera.main;
    }

    public void HandleAllAbilities()
    {
        Debug.DrawLine(camera.transform.position, hit.point);
        if (inputManager.mouseLeftDown) ForcePush();
        ForceGrab();
        HighlightObjects();
    }
    public void ForcePush()
    {

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {

            Debug.Log(hit.collider.gameObject);

            if (hit.collider.gameObject.GetComponent<Rigidbody>() != null)
            {
                force = 0;
                if (maxForce - Vector3.Distance(hit.point, camera.transform.position) > 0) force = maxForce - Vector3.Distance(hit.point, camera.transform.position);
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce((hit.point - camera.transform.position) * force, ForceMode.Impulse);
            }

        }
    }

    public void ForceGrab()
    {
        if (Input.GetKeyDown(forceGrab)) gravitySphere.active = !gravitySphere.active;

        if (gravitySphere.active && inputManager.mouseLeftDown)
        {
            gravitySphere.active = false;
            gravitySphere.Push(grabPushForce);

        }
        if (inputManager.mouseRightDown)
        {
            //grab = !grab;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
            {

                forwardOffset = Vector3.Distance(camera.transform.position, hit.point);
            }
        }

        if (Input.GetKey(grabAway)) forwardOffset += .05f;
        if (Input.GetKey(grabTowards)) forwardOffset -= .05f;
        grabPoint.transform.position = camera.transform.position + camera.transform.forward * forwardOffset;

    }


    public void HighlightObjects()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit))
        {
           
            currentHitCollider = hit.collider;
           
            if (currentHitCollider.GetComponent<Outline>() != null && currentHitCollider.gameObject != lastHitCollider.gameObject)
            {
                lastHitCollider = hit.collider;
                lastHitCollider.GetComponent<Outline>().enabled = true;
            }
            else 
            {
                lastHitCollider = hit.collider;

                lastHitCollider.GetComponent<Outline>().enabled = false;
            }
        }
    }




    public void Fly()
    {
        if (Input.GetMouseButton(1))
        {
            GameObject newJet = Instantiate(jet, feetPos.position, transform.rotation);
            Destroy(newJet, 0.2f);
            Debug.Log("SPAWN");
        }
    }
}





