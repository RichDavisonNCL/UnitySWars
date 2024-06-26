using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsMapInstanceEditor))]
[CanEditMultipleObjects]
public class SWarsMapVisEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool saving = false;
        DrawDefaultInspector();

        if (GUILayout.Button("Save Map"))
        {
            saving = true;
        }

        if (GUILayout.Button("Add New Vehicle Nav Point"))
        {
        }

        if (GUILayout.Button("Add New Block Line"))
        {
        }

        for (int i = 0; i < targets.Length; ++i)
        {
            SWarsMapInstanceEditor v = (SWarsMapInstanceEditor)targets[i];

            if (saving)
            {
                v.SaveMapFile();
            }
        }
    }

}
