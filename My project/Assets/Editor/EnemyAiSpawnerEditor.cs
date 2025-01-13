using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyAi))]
public class EnemyAiEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Get the reference to the EnemyAi component
        EnemyAi spawner = (EnemyAi)target;

        // Display the default inspector elements
        DrawDefaultInspector();


        // Button to start the spawning process
        if (GUILayout.Button("Start Spawning"))
        {
            spawner.StartSpawning();
        }

        // Display the list of spawned objects (optional, depending on how you want to show this)
        EditorGUILayout.Space();
    }
}
