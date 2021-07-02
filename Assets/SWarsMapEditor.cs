using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsMap))]
public class SWarsMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        if (GUILayout.Button("Save Map"))
        {
            SWarsMap m = (SWarsMap)target;
            m.loader.SaveToOriginalFile();
        }
    }
}
