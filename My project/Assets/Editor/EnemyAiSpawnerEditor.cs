using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyAi))]
public class EnemyAiEditor : Editor
{
    public override void OnInspectorGUI()
    {
           //script that makes defining PatrolPoints easier
        EnemyAi spawner = (EnemyAi)target;

        //display deafult inspector elements
        DrawDefaultInspector();


        //button for spawning
        if (GUILayout.Button("Start Spawning"))
        {
            spawner.StartSpawning();
        }

        //displays a list of spawned objects
        EditorGUILayout.Space();
    }
}
