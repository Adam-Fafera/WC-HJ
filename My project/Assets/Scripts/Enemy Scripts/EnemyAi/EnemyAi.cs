using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : Npc
{
    [SerializeField] public GameObject patrolPointPrefab;
    [SerializeField] public float rotationSpeed = 5f;

    public List<GameObject> patrolPoints = new List<GameObject>(); //used for patrolling
    public States currentState = States.Patrol; //handles states
    private PanicMode currentPanicState = PanicMode.Calm;
    private int currentPointIndex = 0; //used to iterate thhrough points
    
    private Coroutine searchCoroutine; //Coroutine used to haandle searching

    private AiShooting aiShooting;






    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();
        aiShooting = GetComponent<AiShooting>();

        if (aiShooting != null )
        {
            Debug.Log("Nie jest nullem");
        }

        if (navMeshAgent == null)
        {
            Debug.LogError("NavMeshAgent not found on " + gameObject.name);
        }
        navMeshAgent.enabled = true;

        //disable all navmesh updates, they mess with pathfinding AI
        navMeshAgent.updateRotation = false;
        navMeshAgent.angularSpeed = 0f;
        navMeshAgent.updateUpAxis = false;

        //check for available partol points
        if (patrolPoints.Count > 0)
        {
            MoveToNextPoint();
        }
        else
        {
            currentState = States.Idle; //If no patrol points are set automatically go into Idle
        }

        StartCoroutine(FOVCheck());
        StartCoroutine(UpdateLastKnownPlayerPosition());

    }

    private void Update()
    {
        if (panicModeState == PanicMode.Panic)
        {
            navMeshAgent.speed = panicModeSpeed;
            if (CanSeePlayer == true && inShootRange == true)
            {
                ChangeState(States.Attacking);
            }
            else if (CanSeePlayer == true)
            {
                ChangeState(States.Chasing);
            }
            if (CanSeePlayer == false && lastKnownPlayerPosition != Vector3.zero)
            {
                ChangeState(States.Searching);
            }
        }


        //handles state machine
        switch (currentState)
        {
            case States.Idle:
                navMeshAgent.isStopped = true;
                break;

            case States.Patrol:
                navMeshAgent.isStopped = false;
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
                {
                    MoveToNextPoint();
                }
                break;

            case States.Searching:
                navMeshAgent.isStopped = false;
                SearchForPlayer();
                break;

            case States.Attacking:
                navMeshAgent.isStopped = true;
                aiShooting.AimAndShoot();
                break;


            case States.Chasing:
                navMeshAgent.isStopped = false;
                ChasePlayer();
                break;
        
                
        }


        HandleRotation();//used for rotating the character manually 
    }
    

   
    private void MoveToNextPoint()
    {
        navMeshAgent.isStopped = false;

        //set destination to the next target
        navMeshAgent.SetDestination(patrolPoints[currentPointIndex].transform.position);

        //update the index
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
    }

    

    public void ChangeState(States newState) //function for changing states
    {
        currentState = newState;
    }

    public void StartSpawning() //Used for making a list of patrol points
    {
        float currentOffset = (patrolPoints.Count + 1); //makes the patrol points spawn to the right of the enemy
        Vector3 spawnPosition = transform.position + Vector3.right * currentOffset;
        GameObject newObject = Instantiate(patrolPointPrefab, spawnPosition, Quaternion.identity); //creates a new patrol point

        Transform parentTransform = transform.parent.Find("pPoints"); //searches for an object used to stope ppoints

        if (parentTransform == null)//if null creates it
        {
            GameObject pPoints = new GameObject("pPoints");
            pPoints.transform.SetParent(transform.parent);
            parentTransform = pPoints.transform;
        }

        newObject.transform.SetParent(parentTransform);
        patrolPoints.Add(newObject);
        newObject.SetActive(true);//adds the object and sets it active
    }
    public void SearchForPlayer()//used for searching for the player after losing them.
    {
        if (lastKnownPlayerPosition != Vector3.zero) //cheks if we have the lkpp
        {
            
            if (navMeshAgent.remainingDistance < 0.5f) //if we're at the lkpp find a new one
            {
                if (!navMeshAgent.pathPending) 
                {
                    StartSearchingRandomly();
                }
            }
            else
            {
                navMeshAgent.SetDestination(lastKnownPlayerPosition); //goes to the lkpp
            }
        }

    }
    private void StartSearchingRandomly()
    {

        if (searchCoroutine == null)//checks if we have a Couroutine
        {
            searchCoroutine = StartCoroutine(SearchCoroutine());
        }
    }

    private IEnumerator SearchCoroutine() //Creates a new Search point for the Enemy
    {
        int angleIncreaser = 0;
        while (true)
        {
            
            Vector3 randomDirection = GetRandomSearchDirection(angleIncreaser);
            Vector3 searchPosition = lastKnownPlayerPosition + randomDirection * Random.Range(5f, 20f); //Search radius, radius in which new points will be placed

            NavMeshHit hit;

            if (NavMesh.SamplePosition(searchPosition, out hit, 1f, NavMesh.AllAreas)) //we check if the new point is on the navmesh and is a valid point for travel
            {
                //if it is we set it
                searchPosition = hit.position;
                navMeshAgent.SetDestination(searchPosition);
                lastKnownPlayerPosition = searchPosition;//update the lkpp so the next one will be calculated from this point
            }
            else
            {
                angleIncreaser++;
                continue; //if it is invalid run the loop again
            }

            //waits until the enemy has reached the destination
            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > 0.5f)
            {
                yield return null;
            }

            
            yield return new WaitForSeconds(0); 
        }
    }

    private Vector3 GetRandomSearchDirection(int extraAngle) //Creates a random point in a direction that the player was last seen
    {
        //Calculates a vector based on the Player Position relative to the enemy
        Vector3 directionToPlayer = lastKnownPlayerPosition - transform.position;
        directionToPlayer.z = 0f; // 2D space

        //randomize the angle so the enenmy is not going in one direction
        float angleVariation = Random.Range(-100f+20f*extraAngle, 100f+20f * extraAngle);
        Quaternion rotation = Quaternion.Euler(0f, 0f, angleVariation);

        
        Vector3 randomDirection = rotation * directionToPlayer;

        return randomDirection.normalized;
    }

    private void ChasePlayer()
    {
        navMeshAgent.SetDestination(lastKnownPlayerPosition);
    }
}



public enum States
{
    Idle,
    Patrol,
    Chasing,
    Attacking,
    Searching
}

