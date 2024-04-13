using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;
[CustomEditor(typeof(SWarsVehicleIO))]
public class SWarsVehicleIOEditor : Editor
{
    [SerializeField]
    string outputFileName;
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SWarsVehicleIO vehicleIO = (SWarsVehicleIO)target;

        if (GUILayout.Button("Load Vehicle File"))
        {
            vehicleIO.LoadVehicles();
        }
        if (GUILayout.Button("Save Vehicle File"))
        {
            vehicleIO.SaveVehicles(outputFileName);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
