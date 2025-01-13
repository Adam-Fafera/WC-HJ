using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{

    [SerializeField]
    public GameObject patrolPointPrefab;


    public List<GameObject> patrolPoints = new List<GameObject>();
    public States currentState = States.Patrol;
    private NavMeshAgent navMeshAgent;
    private int currentPointIndex = 0;

    private void Start()
    {
        // Get the NavMeshAgent component
        navMeshAgent = GetComponent<NavMeshAgent>();
      
        // Ensure the points array is not empty
        if (patrolPoints.Count > 0)
        {
            // Start cycling immediately
            MoveToNextPoint();
        }
        else
        {
            Debug.LogError("No patrolPoints defined for the character to cycle to.");
        }
    }

    private void Update()
    {

        this.transform.rotation = Quaternion.Euler(0f, this.transform.rotation.y, this.transform.rotation.z);
        // Handle state-based actions
        switch (currentState)
        {
            case States.Idle:
                // The character is idle, so no movement.
                navMeshAgent.isStopped = true;
                break;

            case States.Patrol:
                // If the agent is not moving, move to the next point
                if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
                {
                    // Move to the next point in the array
                    MoveToNextPoint();
                }
                break;

                // Add other cases for other states if necessary
        }
    }

    private void MoveToNextPoint()
    {
        // Set the agent to go to the next point
        
            navMeshAgent.isStopped = false;
            //Debug.Log(currentPointIndex);
            navMeshAgent.SetDestination(patrolPoints[currentPointIndex].transform.position);
            
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
       
    }

   
    public void ChangeState(States newState)
    {
        currentState = newState;
    }



    public void StartSpawning()
    {
        // Calculate the spawn position: 1 unit further to the right for each entity spawned
        float currentOffset = (patrolPoints.Count + 1); // The offset will equal the number of objects already spawned + 1

        // Spawn the object at the calculated position (based on the currentOffset)
        Vector3 spawnPosition = transform.position + Vector3.right * currentOffset;

        // Instantiate the object at the spawn position
        GameObject newObject = Instantiate(patrolPointPrefab, spawnPosition, Quaternion.identity);

        // Check if the parent container ("pPoints") already exists, if not, create it
        Transform parentTransform = transform.parent.Find("pPoints");

        if (parentTransform == null)
        {
            // Create a new GameObject called "pPoints" under the parent of the current GameObject
            GameObject pPoints = new GameObject("pPoints");
            pPoints.transform.SetParent(transform.parent);  // Set its parent to be the same as the parent of the current object
            parentTransform = pPoints.transform;
        }

        // Set the new object's parent to "pPoints"
        newObject.transform.SetParent(parentTransform);

        // Add the newly spawned object to the patrolPoints list
        patrolPoints.Add(newObject);

        // Make the object active
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