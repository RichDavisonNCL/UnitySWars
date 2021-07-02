using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsVehicleNavigationSetup : MonoBehaviour
{
    public void BuildConnections()
    {
        Dictionary<int, SWarsVehicleNavigationVis> allNodes = new Dictionary<int, SWarsVehicleNavigationVis>();

        SWarsVehicleNavigationVis[] foundNodes = GetComponentsInChildren<SWarsVehicleNavigationVis>();

        foreach(SWarsVehicleNavigationVis node in foundNodes)
        {
            allNodes.Add(node.connectionID, node);
        }
        foreach (SWarsVehicleNavigationVis node in foundNodes)
        {
            int[] nodes =
            {
                node.navData.nodeConnection1,
                node.navData.nodeConnection2,
                node.navData.nodeConnection3,
                node.navData.nodeConnection4
            };
            for(int i = 0; i < 4; ++i)
            {
                if (nodes[i] > 0)
                {
                    int id = nodes[i] & 511;
                    SWarsVehicleNavigationVis cNode = null;
                    allNodes.TryGetValue(id, out cNode);
                    if (allNodes.TryGetValue(id, out cNode))
                    {
                        node.connections[i] = cNode;
                    }
                    else
                    {
                        Debug.LogWarning("Incorrect nav ID " + id);
                    }
                }
            }
        }
    }
}
