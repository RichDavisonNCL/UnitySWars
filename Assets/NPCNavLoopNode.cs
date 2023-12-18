using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNavLoopNode : MonoBehaviour
{
    public int index = 0;
    public SWars.Map map;

    [SerializeField]
    SWars.NPCNavPoint point;

    public void SetBlockLineDetails(SWars.Map source, int blockID)
    {
        map = source;
        index = blockID;

        point = map.navPoints[index];
    }

    void Update()
    {
        //SWars.NPCNavPoint point = map.navPoints[index];

        //point.
    }
}
