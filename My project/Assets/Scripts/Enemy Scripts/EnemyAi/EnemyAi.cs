using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : Health
{
    [SerializeField] public GameObject patrolPointPrefab;
    [SerializeField] public float rotationSpeed = 5f;

    public List<GameObject> patrolPoints = new List<GameObject>(); // Used for patrolling
    public States currentState = States.Patrol; // Handles states

    private NavMeshAgent navMeshAgent;

    private int currentPointIndex = 0; // Helps iterate through the patrol points

    public float radius = 10;
    [Range(1,360)]public float angle = 45;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;

    [SerializeField]
    public GameObject playerRef;

    public bool CanSeePlayer { get; private set; }


    public EnemyAi(int health) : base(health) { }


    private void Awake()
    {
       

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.freezeRotation = true;  // Prevent rotation via Rigidbody2D
        }
    }

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();



        // Disable all automatic rotation updates
        navMeshAgent.updateRotation = false;
        navMeshAgent.angularSpeed = 0f;  // Set angular speed to 0 to prevent rotation
        navMeshAgent.updateUpAxis = false;

        // Check if there are patrol points
        if (patrolPoints.Count > 0)
        {
            MoveToNextPoint();
        }
        else
        {
            Debug.LogError("No patrol points defined.");
        }

        StartCoroutine(FOVCheck());

    }

    private void Update()
    {
      

      

        // Handle state machine (can be expanded with other states like idle, chase, etc.)
        switch (currentState)
        {
            case States.Idle:
                navMeshAgent.isStopped = true;
                break;

            case States.Patrol:
                // Move between points
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
                {
                    MoveToNextPoint();
                }
                break;

                // Future states like chasing, attacking, etc.
        }

        // Rotate the character towards the direction it's moving
        HandleRotation();
    }
    

   
    private void MoveToNextPoint()
    {
        navMeshAgent.isStopped = false;

        // Set the destination of the NavMeshAgent to the next patrol point
        navMeshAgent.SetDestination(patrolPoints[currentPointIndex].transform.position);

        // Update current point index to loop through patrol points
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
    }

    private void HandleRotation()
    {
        // Check if the NavMeshAgent is moving (velocity is not zero)
        if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
        {
            // Get direction from the velocity (the direction the agent is moving)
            Vector3 direction = navMeshAgent.velocity.normalized;
            direction.z = 0f; // Ensure the rotation stays in 2D (only on the Z-axis)

            // Calculate the angle in degrees for rotation based on direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Apply the rotation directly (no smoothing)
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    public void ChangeState(States newState) // Changes the state for the state machine
    {
        currentState = newState;
    }

    public void StartSpawning() // Used for making a list of points for the patrol route
    {
        float currentOffset = (patrolPoints.Count + 1);
        Vector3 spawnPosition = transform.position + Vector3.right * currentOffset;
        GameObject newObject = Instantiate(patrolPointPrefab, spawnPosition, Quaternion.identity);

        Transform parentTransform = transform.parent.Find("pPoints");

        if (parentTransform == null)
        {
            GameObject pPoints = new GameObject("pPoints");
            pPoints.transform.SetParent(transform.parent);
            parentTransform = pPoints.transform;
        }

        newObject.transform.SetParent(parentTransform);
        patrolPoints.Add(newObject);
        newObject.SetActive(true);
    }
    public override void TakeDamage(int damage)
    {
        HealthGet -= damage;

        if (HealthGet <= 0)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }

    private IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FOV();
        }
    }


    private void FOV()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(transform.right, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer))
                {
                    CanSeePlayer = true;
                }
                else
                {
                    CanSeePlayer = false;
                }
            }
            else
            {
                CanSeePlayer = false;
            }
        }
        else if (CanSeePlayer)
        {
            CanSeePlayer = false;
        }

         
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position,Vector3.forward,radius);

        Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z + 90, -angle/2);
        Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z + 90, angle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.position+angle01*radius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * radius);

        if (CanSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,playerRef.transform.position);
        }
    }
    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),Mathf.Cos(angleInDegrees* Mathf.Deg2Rad));
    }
}

public enum States
{
    Idle,
    Patrol,
    Chasing,
    Attacking
}
public enum PanicMode
{
    Calm,
    Panic
}
