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

    public List<GameObject> patrolPoints = new List<GameObject>(); // Used for patrolling
    public States currentState = States.Patrol; // Handles states
    private int currentPointIndex = 0; // Helps iterate through the patrol points
    



  
    void Start()
    {

        navMeshAgent = GetComponent<NavMeshAgent>();


        // Disable all automatic rotation updates
        navMeshAgent.updateRotation = false;
        navMeshAgent.angularSpeed = 0f;
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
   
    
}

public enum States
{
    Idle,
    Patrol,
    Chasing,
    Attacking
}

