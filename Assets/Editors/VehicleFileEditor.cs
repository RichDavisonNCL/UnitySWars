using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;
[CustomEditor(typeof(VehicleFile))]
public class VehicleFileEditor : Editor
{
    [SerializeField]
    string outputFileName;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        VehicleFile vehicleIO = (VehicleFile)target;

        if (GUILayout.Button("Load Vehicle File"))
        {
            vehicleIO.LoadVehicles();
        }
        if (GUILayout.Button("Save Vehicle File"))
        {
            vehicleIO.SaveVehicles(outputFileName);
        }
    }
}
