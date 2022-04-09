using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsMapVis))]
public class SWarsMapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();

        if (GUILayout.Button("Save Map"))
        {
            SWarsMapVis m = (SWarsMapVis)target;
            m.loadedMap.SaveToOriginalFile();
        }
    }
}
