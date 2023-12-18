using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsVehicleNavigationIO : MonoBehaviour
{
    SWars.Map map;
    public void BuildConnections(SWars.Map sourceMap)
    {
        for(int i = 0; i < transform.childCount; ++i)
        {
            GameObject o = transform.GetChild(i).gameObject;
            SWarsVehicleNavigationNode node = o.GetComponent<SWarsVehicleNavigationNode>();
            if(node == null)
            {
                continue;
            }
            SWars.VehicleNavPoint nav = sourceMap.vehicleNavPoints[i];

            int[] nodes =
            {
                512 - (nav.nodeConnection1 & 511),
                512 - (nav.nodeConnection2 & 511),
                512 - (nav.nodeConnection3 & 511),
                512 - (nav.nodeConnection4 & 511)
            };

            for(int j = 0; j < 4; ++j)
            {
                node.connections[j] = null;

                if (nodes[j] >= 0 && nodes[j] < 512)
                {
                    if(nodes[j] >= transform.childCount)
                    {
                        Debug.Log("invalid navigation connection " + nodes[j]);
                        continue;
                    }

                     node.connections[j] = transform.GetChild(nodes[j]).GetComponent<SWarsVehicleNavigationNode>();
                }
            }
        }
    }
}
