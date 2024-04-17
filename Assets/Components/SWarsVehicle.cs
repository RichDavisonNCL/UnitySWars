using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsVehicle : MonoBehaviour
{
    public int vehicleIndex;
    public SWars.VehicleMeshFile vehicleFile;

    public List<int> faceLookup;

    public void SetFileDetails(SWars.VehicleMeshFile inVehicleFile, int inIndex)
    {
        vehicleFile = inVehicleFile;
        vehicleIndex = inIndex;
    }

    public void SetFaceLookup(List<int> inFaces)
    {
        faceLookup = inFaces;
    }
}
