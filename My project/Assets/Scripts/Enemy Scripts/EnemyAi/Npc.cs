using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//will be inherited by the civilian class
//Class used for all Npc's
public class Npc : Health
{



    protected NavMeshAgent navMeshAgent;

    [SerializeField]
    public GameObject playerRef;//player ref, used to referance the Player Character
    public bool inShootRange {  get; private set; }
    public bool CanSeePlayer { get; private set; } //bool used to check line of sight with the player 
    public float radius = 10; //radius of the line of sight
    public float innerRadius = 2; //radius for detecting presence when the player is extreamly close to the npc
    [Range(1, 360)] public float angle = 45; //angle of the line of sight


    public LayerMask targetLayer; //layer on which the player is
    public LayerMask obstructionLayer; //layer on which obstacles are

    protected Health healthComponent;

    protected Vector3 lastKnownPlayerPosition = Vector3.zero; //last known player position (lkpp)

    protected void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.freezeRotation = true; //freze rotation to avoid problems with navMeshAgent
        }
       
    }
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();


        //disable all automatic rotation updates
        navMeshAgent.updateRotation = false;
        navMeshAgent.angularSpeed = 0f;
        navMeshAgent.updateUpAxis = false;



        healthComponent = GetComponent<Health>();

    }
    private void Update()
    {
        
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }



    protected IEnumerator FOVCheck() //makes a vision check every 0.2 seconds
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);
        while (true)
        {
            yield return wait;
            FOV();
        }
    }
    public override void TakeDamage(int damage) //function used for taking damage
    {
        HealthGet -= damage;

        if (HealthGet <= 0)
        {
            Debug.Log("me me dead");
            Destroy(this.transform.parent.gameObject);
        }
    }
    protected void HandleRotation()
    {
        //check if the object is mobing
        if (navMeshAgent.velocity.sqrMagnitude > 0.1f)
        {
            //get velocity from direction of movement
            Vector3 direction = navMeshAgent.velocity.normalized;
            direction.z = 0f; //block movement to a 2D plane

            //calculate the angle in degrees
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            //aply the rotation
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    protected void FOV()
    {
        Collider2D[] rangeCheck = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer); //colider used to see if something is within the range of vision
        Collider2D[] innerRangeCheck = Physics2D.OverlapCircleAll(transform.position, innerRadius, targetLayer); //colider used to see if something is right beside the enemy
        Collider2D[] shootRangeCheck = Physics2D.OverlapCircleAll(transform.position, radius/2, targetLayer); //colider used to see if something is in shooting range

        //checking if the player is in our line of sight
        if (rangeCheck.Length > 0)
        {
            Transform target = rangeCheck[0].transform;
            

            Vector2 directionToTarget = (target.position - transform.position).normalized;
            

            if (Vector2.Angle(transform.right, directionToTarget) < angle / 2) //checks if the player is within the vision cone
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);
                

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionLayer)) //casts a raycast to the player, if it hits the player can see it
                {
                    CanSeePlayer = true;
                    if (shootRangeCheck.Length > 0)
                    {
                        Transform shootRangeTarget = shootRangeCheck[0].transform;
                        Vector2 directionToShoot = (shootRangeTarget.position - transform.position).normalized;
                        float distanceToShoot = Vector2.Distance(transform.position, shootRangeTarget.position);
                        if (!Physics2D.Raycast(transform.position, directionToShoot, distanceToShoot, obstructionLayer))
                        {
                            inShootRange = true;
                        }
                        else
                        {
                            inShootRange = false;
                        }
                    }
                    else
                    {
                        inShootRange = false;
                    }    
                }
                else
                {
                    CanSeePlayer = false;
                    inShootRange = false;
                }
            }
            else
            {
                CanSeePlayer = false;
                inShootRange = false;
            }
        }
        else if (CanSeePlayer)
        {
            CanSeePlayer = false;
            inShootRange = false;
        }
        //checking if the player character is right next to us

        if (innerRangeCheck.Length > 0)
        {
            Transform innerRangeTarget = innerRangeCheck[0].transform;
            Vector2 directionOfInnerRangeTarger = (innerRangeTarget.position - transform.position).normalized;
            float distanceToInnerTarget = Vector2.Distance(transform.position, innerRangeTarget.position);
            if(!Physics2D.Raycast(transform.position,directionOfInnerRangeTarger, distanceToInnerTarget, obstructionLayer))
            { CanSeePlayer = true;}    
        }
              

    }
    protected void OnDrawGizmos() //used for testing, draws the line of sight 
    {
        Gizmos.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, radius);
        UnityEditor.Handles.DrawWireDisc(transform.transform.position, Vector3.forward, innerRadius);

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
    protected Vector2 DirectionFromAngle(float eulerY, float angleInDegrees) //converts a direction into a 2d Vector
    {
        angleInDegrees += eulerY;

        return new Vector2(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    protected IEnumerator UpdateLastKnownPlayerPosition()
    {
        while (true)
        {
            //Update the last known position every second
            if (CanSeePlayer)
            {
                lastKnownPlayerPosition = playerRef.transform.position; //Update position
            }

            yield return new WaitForSeconds(0.3f);//wait for 1 second before updating again
        }
    }

}
public enum PanicMode //universal states for the state machine
{
    Calm, //used when the npc is unaware of danger
    Panic //used if npc is aware of danger 
}