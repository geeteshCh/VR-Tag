using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuUI))]
public class EditorScript : Editor
{

     public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); // Draws the default inspector
    
            MenuUI script = (MenuUI)target;
            
            if (GUILayout.Button("Room Join"))
            {
                script.OnUserNameEntered("editor");
                script.OnRoomJoinOrCreate();
            }
            
            if (GUILayout.Button("Refresh Session List"))
            {
                script.RefreshSessionList();
            }
    
            // Add more buttons for other functions as needed
        }

}
