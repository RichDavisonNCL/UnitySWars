using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsMapVis))]
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

        for (int i = 0; i < targets.Length; ++i)
        {
            SWarsMapVis v = (SWarsMapVis)targets[i];

            if (saving)
            {
                v.SaveMapFile();
            }
        }
    }

}
