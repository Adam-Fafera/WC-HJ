using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Npc : Health
{



    protected NavMeshAgent navMeshAgent;

    [SerializeField]
    public GameObject playerRef;
    public bool CanSeePlayer { get; private set; }

    public float radius = 10;
    [Range(1, 360)] public float angle = 45;
    public LayerMask targetLayer;
    public LayerMask obstructionLayer;

    protected Health healthComponent;

    protected void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.freezeRotation = true;
        }
    }
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();


        // Disable all automatic rotation updates
        navMeshAgent.updateRotation = false;
        navMeshAgent.angularSpeed = 0f;
        navMeshAgent.updateUpAxis = false;


        healthComponent = GetComponent<Health>();

    }



    protected IEnumerator FOVCheck()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FOV();
        }
    }
    public override void TakeDamage(int damage)
    {
        HealthGet -= damage;

        if (HealthGet <= 0)
        {
            Destroy(this.transform.parent.gameObject);
        }
    }
    protected void HandleRotation()
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

    protected void FOV()
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
    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);

        Vector3 angle01 = DirectionFromAngle(-transform.eulerAngles.z + 90, -angle / 2);
        Vector3 angle02 = DirectionFromAngle(-transform.eulerAngles.z + 90, angle / 2);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + angle01 * radius);
        Gizmos.DrawLine(transform.position, transform.position + angle02 * radius);

        if (CanSeePlayer)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, playerRef.transform.position);
        }
    }
    private Vector2 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }


}
public enum PanicMode
{
    Calm,
    Panic
}