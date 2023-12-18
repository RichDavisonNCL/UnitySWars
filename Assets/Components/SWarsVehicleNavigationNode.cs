using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWars;
public class SWarsVehicleNavigationNode : MonoBehaviour
{
    public int navIndex = 0;
    public SWars.Map map;

    public int connectionID;

    public SWarsVehicleNavigationNode[] connections = new SWarsVehicleNavigationNode[4];

    public void SetNavDetails(SWars.Map source, int index)
    {
        map = source;
        navIndex    = index;

        VehicleNavPoint nav = source.vehicleNavPoints[index];

        transform.localPosition = new Vector3(nav.x, nav.y, nav.z);

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
