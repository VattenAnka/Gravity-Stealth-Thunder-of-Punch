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
    public float offsetSpeed;
    public float maxOffset;
    public float minOffset;
    public GameObject jet;
    public Transform feetPos;
    bool grab;
    public float pullForce;
    public float dragToGive;
    [SerializeField] KeyCode forceGrab, grabTowards, grabAway;
    Collider lastHitCollider;
    Collider currentHitCollider;
    //GravityPull_ gravitySphere;
    InputManager inputManager;
    Outline outline;
    GameObject grabObject;
    Rigidbody playerRb;
    bool hold;
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        // gravitySphere = grabPoint.GetComponent<GravityPull_>();
        camera = Camera.main;
    }

    public void HandleAllAbilities()
    {
        Debug.DrawLine(camera.transform.position, hit.point);
        if (inputManager.mouseLeftDown) ForcePush();
        ForceGrab();

    }

    public void HandleAbilitiesFixed()
    {

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
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForceAtPosition((hit.point - camera.transform.position) * force, hit.point, ForceMode.Impulse);
            }

        }
    }
    RaycastHit forceGrabHit;
    public void ForceGrab()
    {
        //if (Input.GetKeyDown(forceGrab)) gravitySphere.active = !gravitySphere.active;

        /*if (gravitySphere.active && inputManager.mouseLeftDown)
        {
            gravitySphere.active = false;
            gravitySphere.Push(grabPushForce);

        }*/
        GameObject hitObject;
        Rigidbody hitRB;
        
        if (inputManager.mouseRightDown)
        {
            hold = !hold;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out forceGrabHit, maxOffset))
            {

                forwardOffset = Vector3.Distance(camera.transform.position, forceGrabHit.point);
            }
        }

        if (forceGrabHit.collider != null )
        {
            hitObject = forceGrabHit.collider.gameObject;
            hitRB = hitObject.GetComponent<Rigidbody>();
            KeepObjectInPlace(hitRB, hitObject);
        }



        if (Input.GetKey(grabAway) && forwardOffset < maxOffset) forwardOffset += offsetSpeed * Time.deltaTime;
        if (Input.GetKey(grabTowards) && forwardOffset > minOffset) forwardOffset -= offsetSpeed * Time.deltaTime;
        grabPoint.transform.position = camera.transform.position + camera.transform.forward * forwardOffset;

    }

    private void KeepObjectInPlace(Rigidbody rb, GameObject hitObject)
    {
        float mass = rb.mass;

        if (rb != null && hold && rb != playerRb)
        {
            rb.AddExplosionForce(-pullForce, grabPoint.position, Mathf.Infinity);
            //rb.isKinematic = true;
           // rb.mass = 5;
            rb.drag = dragToGive;
            rb.angularDrag = dragToGive;
          //   rb.freezeRotation = false;


        }
        else if (rb != null && rb != playerRb)
        {
            rb.drag = 0;
            rb.angularDrag = 0;
           // rb.mass = mass;
           // rb.isKinematic = true;
            //rb.freezeRotation = false;
           // rb.useGravity = true;
        }

    }

    public void HighlightObjects()
    {
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, maxOffset))
        {


            Outline hitOutline = hit.collider.GetComponent<Outline>();
            
            if (hitOutline == null)
            {
                if (outline != null)
                {
                    outline.enabled = false;
                    outline = null;
                }
                return;
            }

            if (outline == null)
            {
                outline = hitOutline;
                outline.enabled = true;
            }
            else if (outline != hitOutline)
            {
                outline.enabled = false;
                outline = hitOutline;
                outline.enabled = true;
            }


        }
        else if (outline != null)
        {
            outline.enabled = false;
            outline = null;
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





