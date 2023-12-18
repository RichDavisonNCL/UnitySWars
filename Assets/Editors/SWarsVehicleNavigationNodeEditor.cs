using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWarsVehicleNavigationNode))]
[CanEditMultipleObjects]
public class SWarsVehicleNavigationNodeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();
        serializedObject.ApplyModifiedProperties();

        for (int i = 0; i < targets.Length; ++i)
        {
            SWarsVehicleNavigationNode nav = (SWarsVehicleNavigationNode)targets[i];

            SWars.Map map = nav.map;

            SWars.VehicleNavPoint navPoint = map.vehicleNavPoints[nav.navIndex];

            navPoint.junctionNodes = (ushort)EditorGUILayout.IntField("junctionNodes: ", navPoint.junctionNodes);
            navPoint.unknown6 = (ushort)EditorGUILayout.IntField("unknown6: ", navPoint.unknown6);

            navPoint.unknown8 = (ushort)EditorGUILayout.IntField("unknown8: ", navPoint.unknown8);
            navPoint.unknown9 = (ushort)EditorGUILayout.IntField("unknown9: ", navPoint.unknown9);
            navPoint.unknown10 = (ushort)EditorGUILayout.IntField("unknown10: ", navPoint.unknown10);

            navPoint.typeFlags = (SWars.VehicleNavPointType)EditorGUILayout.EnumFlagsField(navPoint.typeFlags);

            navPoint.blank1 = (ushort)EditorGUILayout.IntField("blank1: ", navPoint.blank1);
            navPoint.blank2 = (ushort)EditorGUILayout.IntField("blank2: ", navPoint.blank2);
            navPoint.blank3 = (ushort)EditorGUILayout.IntField("blank3: ", navPoint.blank3);
            navPoint.blank4 = (ushort)EditorGUILayout.IntField("blank4: ", navPoint.blank4);
            navPoint.blank5 = (ushort)EditorGUILayout.IntField("blank5: ", navPoint.blank5);

            navPoint.x = (short)nav.transform.localPosition.x;
            navPoint.y = (short)nav.transform.localPosition.y;
            navPoint.z = (short)nav.transform.localPosition.z;

            map.vehicleNavPoints[nav.navIndex] = navPoint;
        }
    }
}
