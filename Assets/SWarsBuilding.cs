using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsBuilding : MonoBehaviour
{
    public List<int> faceLookup;
    public SWars.Map loadedMap;
    public int buildingIndex;

    public void SetState(int inIndex, SWars.Map inMap, List<int> inFaces)
    {
        faceLookup = inFaces;
        loadedMap = inMap;
        buildingIndex = inIndex;
    }
}
