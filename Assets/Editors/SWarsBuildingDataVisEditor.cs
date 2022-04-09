using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsBuildingDataVis))]
[CanEditMultipleObjects]
public class SWarsBuildingDataVisEditor : Editor
{
    public override void OnInspectorGUI()
    {
        bool saving = false;
        bool randomising = false;

        DrawDefaultInspector();
        if (GUILayout.Button("Randomise Data"))
        {
            randomising = true;
        }
        if (GUILayout.Button("Save Entire Map"))
        {
            saving = true;
        }

        SWars.Map l = null;
        for (int i = 0; i < targets.Length; ++i)
        {
            SWarsBuildingDataVis v = (SWarsBuildingDataVis)targets[i];
            Transform t = v.transform;
            l = v.sourceMap;

            v.meshDetail.xPosition = (ushort)t.localPosition.x;
            v.meshDetail.yPosition = (ushort)t.localPosition.y;
            v.meshDetail.zPosition = (ushort)t.localPosition.z;

            if (randomising)
            {
                v.Randomise(i);
                v.WriteDetails();
            }        
            if (saving)
            {
                v.WriteDetails();
            }
        }
        if (saving)
        {
            l.SaveToOriginalFile();
        }

        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }
}
