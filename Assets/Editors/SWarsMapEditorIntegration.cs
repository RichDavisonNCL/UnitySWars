using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsMapInstanceEditor))]
public class SWarsMapEditorIntegration : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        if (GUILayout.Button("Save Map"))
        {
            SWarsMapInstanceEditor m = (SWarsMapInstanceEditor)target;
            m.SaveMapFile();
        }


        if (GUILayout.Button("Add New Block Line"))
        {
            SWarsMapInstanceEditor m = (SWarsMapInstanceEditor)target;
            m.AddNewBlockLine();
        }
    }
}
