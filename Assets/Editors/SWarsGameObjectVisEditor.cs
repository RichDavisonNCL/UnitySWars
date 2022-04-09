using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsGameObjectVis))]
[CanEditMultipleObjects]
public class SWarsGameObjectVisEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        bool saving = false;
        bool saveMission =false;
        if (GUILayout.Button("Write To Mission Structure"))
        {
            saving = true;
        }

        if (GUILayout.Button("Write and Save Mission"))
        {
            saveMission = true;
        }

        if (saving || saveMission)
        {
            for (int i = 0; i < targets.Length; ++i)
            {
                SWarsGameObjectVis v = (SWarsGameObjectVis)targets[i];

                v.sourceMission.WriteObjectData(v.data, v.dataIndex);
            }
        }
        if(saveMission)
        {
            if(targets.Length > 0)
            {
                SWarsGameObjectVis v = (SWarsGameObjectVis)targets[0];
                v.sourceMission.SaveData();
            }
        }
    }
}
