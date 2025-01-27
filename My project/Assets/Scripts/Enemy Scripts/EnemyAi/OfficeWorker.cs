using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.AI;

public class OfficeWorker : Npc
{
    private OfficeWorkerStates currentState = OfficeWorkerStates.Idle;
    

    [SerializeField]
    public float checkInterval = 0.4f;

    private Coroutine searchCoroutine = null;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        if (navMeshAgent == null )
        {
            Debug.Log("Navmesh is null for officeworker");
        }

        navMeshAgent.enabled = true;

        //disable all navmesh updates, they mess with pathfinding AI
        navMeshAgent.updateRotation = false;
        navMeshAgent.angularSpeed = 0f;
        navMeshAgent.updateUpAxis = false;

    }
    private void Update()
    {
        if(panicModeState == PanicMode.Panic)//changes speed if in panic mode
        {
            navMeshAgent.speed = panicModeSpeed;
        }

        if (CanSeePlayer == true && panicModeState == PanicMode.Panic && inShootRange == true)//makes the npc run away if player is close
        {
            ChangeState(OfficeWorkerStates.fleeing);

        }
        else if (panicModeState == PanicMode.Panic && CanSeePlayer == true) //npc runs away randomly if they see the player
        {
            currentState = OfficeWorkerStates.hiding;
        }
        


        switch (currentState)
        {

            case OfficeWorkerStates.Idle:
                navMeshAgent.isStopped = true;
                break;

            case OfficeWorkerStates.hiding:
                navMeshAgent.isStopped = false;
                StartSearchingRandomly();
                break;

            case OfficeWorkerStates.fleeing:
                navMeshAgent.isStopped = false;
                FleeFromPlayer();
                break;
                

        }


        StartCoroutine(FOVCheck());
        HandleRotation();
    }
    private void StartSearchingRandomly() //same as in EnemyAi
    {
        if (searchCoroutine == null) // Check if we have a Coroutine running
        {
            searchCoroutine = StartCoroutine(SearchCoroutine());
        }
        
    }

    private IEnumerator SearchCoroutine() // Random search points for the worker
    {
        int angleIncreaser = 0;
        while (true)
        {
            Vector3 randomDirection = GetRandomSearchDirection(angleIncreaser);
            Vector3 searchPosition = lastKnownPlayerPosition + randomDirection * Random.Range(5f, 20f);

            NavMeshHit hit;

            if (NavMesh.SamplePosition(searchPosition, out hit, 1f, NavMesh.AllAreas)) // Check if point is valid on the navmesh
            {
                // If it's valid, set the position
                searchPosition = hit.position;
                navMeshAgent.SetDestination(searchPosition);
                lastKnownPlayerPosition = searchPosition; // Update the last known position
            }
            else
            {
                angleIncreaser++;
                continue; // If invalid, try again with a new direction
            }

            // Wait until the NPC has reached the destination
            while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > 0.5f)
            {
                yield return null;
            }

            yield return new WaitForSeconds(0); // Brief wait before starting another search
        }
    }

    private Vector3 GetRandomSearchDirection(int extraAngle) //Creates a random point in a direction that the player was last seen
    {
        //Calculates a vector based on the Player Position relative to the enemy
        Vector3 directionToPlayer = lastKnownPlayerPosition - transform.position;
        directionToPlayer.z = 0f; // 2D space

        //randomize the angle so the enenmy is not going in one direction
        float angleVariation = Random.Range(-100f + 20f * extraAngle, 100f + 20f * extraAngle);
        Quaternion rotation = Quaternion.Euler(0f, 0f, angleVariation);


        Vector3 randomDirection = rotation * directionToPlayer;

        return randomDirection.normalized;
    }

    private void FleeFromPlayer() //simple code that makes the character go in the opposite direction of the player 
    {
        Debug.Log("FleeFromPlayer is executing");
        Vector3 directionAwayFromPlayer = transform.position - playerRef.transform.position;
        directionAwayFromPlayer.z = 0f;

        directionAwayFromPlayer.Normalize();

        Vector3 fleePosition = transform.position + directionAwayFromPlayer * 5f; 
        navMeshAgent.SetDestination(fleePosition);
    }


    public void ChangeState(OfficeWorkerStates newState) //function for changing states
    {
        currentState = newState;
    }
    
    
}
public enum OfficeWorkerStates
{
    Idle,
    hiding,
    fleeing,
}
