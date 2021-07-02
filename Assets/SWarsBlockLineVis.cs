using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWars;
public class SWarsBlockLineVis : MonoBehaviour
{
    int blockIndex = 0;
    public MapLoader sourceMap;

    [SerializeField]
    public NPCBlockLine blockD;


    public void SetBlockLineDetails(NPCBlockLine details, MapLoader source, int index)
    {
        blockD      = details;
        sourceMap   = source;
        blockIndex  = index;
    }

    void Update()
    {
        Vector3 startPos    = new Vector3(blockD.xStart, blockD.yStart, blockD.zStart);
        Vector3 endPos      = new Vector3(blockD.xEnd  , blockD.yEnd  , blockD.zEnd);
        Debug.DrawLine(startPos / 32, endPos / 32, blockD.unknown1 >= 0 ? Color.green : Color.red);
    }
}
