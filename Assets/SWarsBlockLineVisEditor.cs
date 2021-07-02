using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsBlockLineVis))]
[CanEditMultipleObjects]
public class SWarsBlockLineVisEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();
    }

}
