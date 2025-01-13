using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatrolPointSpawner))]
public class PatrolPointSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the reference to the ObjectSpawner component
        PatrolPointSpawner spawner = (PatrolPointSpawner)target;

        // Display the default inspector elements
        DrawDefaultInspector();

        // Button to start the spawning process
        if (GUILayout.Button("Start Spawning"))
        {
            spawner.StartSpawning();
        }

        // Display the list of spawned objects
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Spawned Objects", EditorStyles.boldLabel);

        // If there are spawned objects, display them in a list
        if (spawner.spawnedObjects.Count > 0)
        {
            for (int i = 0; i < spawner.spawnedObjects.Count; i++)
            {
                // Display each spawned object in the list with a label
                EditorGUILayout.ObjectField($"Object {i + 1}", spawner.spawnedObjects[i], typeof(GameObject), true);
            }
        }
        else
        {
            EditorGUILayout.LabelField("No objects spawned yet.");
        }
    }
}