using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWars;
public class SWarsVehicleNavigationVis : MonoBehaviour
{
    int navIndex = 0;
    public MapLoader sourceMap;

    public int connectionID;

    public SWarsVehicleNavigationVis[] connections = new SWarsVehicleNavigationVis[4];

    [SerializeField]
    public VehicleNavPoint navData;

    public void SetNavDetails(VehicleNavPoint details, MapLoader source, int index)
    {
        navData     = details;
        sourceMap   = source;
        navIndex    = index;

        transform.localPosition = new Vector3(details.x, details.y, details.z);

        connectionID = 512 - index;
    }

    void Update()
    {
        if(connections[0])
        {
            Debug.DrawLine(transform.position, connections[0].transform.position, Color.yellow);
        }
        if (connections[1])
        {
            Debug.DrawLine(transform.position, connections[1].transform.position, Color.yellow);
        }
        if (connections[2])
        {
            Debug.DrawLine(transform.position, connections[2].transform.position, Color.yellow);
        }
        if (connections[3])
        {
            Debug.DrawLine(transform.position, connections[3].transform.position, Color.yellow);
        }
    }
}
