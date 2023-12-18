using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsMapEditor))]
public class SWarsMapEditorIntegration : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        if (GUILayout.Button("Save Map"))
        {
            SWarsMapEditor m = (SWarsMapEditor)target;
            m.SaveMapFile();
        }


        if (GUILayout.Button("Add New Block Line"))
        {
            SWarsMapEditor m = (SWarsMapEditor)target;
            m.AddNewBlockLine();
        }
    }
}
