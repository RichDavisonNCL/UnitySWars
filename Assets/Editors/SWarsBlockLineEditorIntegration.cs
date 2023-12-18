using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsBlockLineNode))]
[CanEditMultipleObjects]
public class SWarsBlockLineEditorIntegration : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();

        for (int i = 0; i < targets.Length; ++i)
        {
            SWarsBlockLineNode block = (SWarsBlockLineNode)targets[i];

            SWars.Map map = block.map;

            SWars.NPCBlockLine blockLine = map.blockLines[block.index];

            blockLine.xStart = (short)EditorGUILayout.IntField("X Start:", blockLine.xStart);
            blockLine.yStart = (short)EditorGUILayout.IntField("Y Start:", blockLine.yStart);
            blockLine.zStart = (short)EditorGUILayout.IntField("Z Start:", blockLine.zStart);

            blockLine.xEnd = (short)EditorGUILayout.IntField("X End:", blockLine.xEnd);
            blockLine.yEnd = (short)EditorGUILayout.IntField("Y End:", blockLine.yEnd);
            blockLine.zEnd = (short)EditorGUILayout.IntField("Z End:", blockLine.zEnd);

            blockLine.primIndex = (short)EditorGUILayout.IntField("Unknown", blockLine.primIndex);

            map.blockLines[block.index] = blockLine;
        } 
    }

}
