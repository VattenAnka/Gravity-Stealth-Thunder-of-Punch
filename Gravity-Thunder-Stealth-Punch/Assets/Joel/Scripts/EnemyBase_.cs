using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase_ : MonoBehaviour
{
    [SerializeField] protected StateMachine state = StateMachine.Patrol;

    [Header("Patrol route configuration")]
    [Tooltip("If set to true, the AI will walk between random points. If false it will follow the path in order from 0 and up")]
    [SerializeField] protected bool randomPath;
    [SerializeField] GameObject patrolRoute;
    [SerializeField] protected float startWaitTime;
    [SerializeField] Transform[] destinations;
    protected Transform targetPlayer => GameObject.FindGameObjectWithTag("Player").transform; //GameObject.Find("Player").transform;
    //[SerializeField] protected float giveUpTimerStart = 5f;
    protected float waitTime;
    protected Transform path;
    protected int destination;

    [Header("Stats")]
    [SerializeField] protected float walkSpeed;
    [SerializeField] protected float runSpeed;
    [SerializeField] protected float rotSpeed;

    //Components
    protected Rigidbody rb;

    public enum StateMachine
    {
        Patrol, Detect, Attack, Chase
    }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();

        SetPatrolRoute();
    }

    protected virtual void Start()
    {
        waitTime = startWaitTime;
        path = destinations[destination];
    }

    protected virtual void Update()
    {
        
    }

    protected virtual void FixedUpdate()
    {
        switch (state)
        {
            case StateMachine.Patrol:
                //Debug.Log("I am patrolling");
                Rotate(path);
                Patrolling(); 
                break;
        }
    }

    protected void SetPatrolRoute()
    {
        if (patrolRoute == null)
        {
            patrolRoute = gameObject;
        }
        destinations = new Transform[patrolRoute.transform.childCount];
        for (int i = 0; i < patrolRoute.transform.childCount; i++)
        {
            destinations[i] = patrolRoute.transform.GetChild(i).GetComponent<Transform>();
        }
    }

    protected void Rotate(Transform target)
    {
        Vector3 currentDirection = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(currentDirection.x, 0, currentDirection.z)), Time.deltaTime * rotSpeed);
    }

    protected virtual void MoveTowardsTarget(Transform target, float moveSpeed)
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        //Debug.Log(pos + "wut");
        rb.MovePosition(pos);
    }

    protected virtual void Patrolling()
    {
        if (patrolRoute != gameObject)
        {
            if (Vector3.Distance(transform.position, destinations[destination].position) <= 1.5f)
            {
                if (waitTime <= 0)
                {
                    //If path route is not random
                    if (!randomPath)
                    {
                        destination++;
                        Debug.Log("destination value: " + destination);
                        if (destination >= destinations.Length)
                        {
                            destination = 0;
                            Debug.Log("destination reset to first path");
                        }
                        path = destinations[destination];
                        waitTime = startWaitTime;

                    }
                    //If path route is random
                    else
                    {
                        destination = Random.Range(0, destinations.Length);
                        path = destinations[destination];
                        waitTime = startWaitTime;
                    }
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
            else
            {
                MoveTowardsTarget(path, walkSpeed);
            }
        }
    }
}