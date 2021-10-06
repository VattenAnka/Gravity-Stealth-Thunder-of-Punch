using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
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
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(camera.transform.position, hit.point);

        //stupid experemint lol
        //Fly();
        ForceGrab();
    }

    private void FixedUpdate()
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
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce((hit.point - camera.transform.position) * force, ForceMode.Impulse);
            }

        }
    }

    public void ForceGrab()
    {
           if(Input.GetKeyDown(forceGrab)) grabPoint.GetComponent<GravityPull_>().active = !grabPoint.GetComponent<GravityPull_>().active;
        if (Input.GetMouseButtonDown(1))
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





