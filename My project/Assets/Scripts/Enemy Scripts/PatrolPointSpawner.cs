using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPointSpawner : MonoBehaviour
{
    // Prefab to spawn (drag a prefab into this field in the Inspector)
    public GameObject objectPrefab;

    // List to keep track of spawned objects
    public List<GameObject> spawnedObjects = new List<GameObject>();

    // Function to start spawning objects at the parent's position
    public void StartSpawning()
    {
        // Calculate the spawn position: 1 unit further to the right for each entity spawned
        float currentOffset = (spawnedObjects.Count+1); // The offset will equal the number of objects already spawned + 1

        // Spawn the object at the calculated position (based on the currentOffset)
        Vector3 spawnPosition = transform.position + Vector3.right * currentOffset;

        // Instantiate the object at the spawn position
        GameObject newObject = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);

        // Parent the newly spawned object to the object this script is attached to
        newObject.transform.SetParent(this.transform);

        // Add the newly spawned object to the list
        spawnedObjects.Add(newObject);

        // Make the object active
        newObject.SetActive(true);
    }
}