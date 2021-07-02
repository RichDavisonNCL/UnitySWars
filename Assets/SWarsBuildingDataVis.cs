using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWars;
using System;

public class SWarsBuildingDataVis : MonoBehaviour
{
    int meshIndex = 0;
    public MapLoader sourceMap;

    [SerializeField]
    public MeshDetails meshDetail;

    [SerializeField]
    public DataBlockD blockD;
    int blockIndex = 0;

    [SerializeField]
    int randomiseFirstIndex = 0;
    [SerializeField]
    int randomiseLastIndex = 80;
    [SerializeField]
    int randomiseRange = 4096;
    [SerializeField]
    int randomSeed = -1;

    public void SetMeshDetails(MeshDetails details, MapLoader source, int index)
    {
        meshDetail  = details;
        sourceMap   = source;
        meshIndex = index;
    }

    public void SetBlockDDetails(DataBlockD details, MapLoader source, int index)
    {
        blockD      = details;
        sourceMap   = source;
        blockIndex  = index;
    }

    public void Randomise(int timeOffset = 0)
    {
        if (randomSeed < 0)
        {
            UnityEngine.Random.InitState(timeOffset + (int)System.DateTime.Now.Ticks);
        }
        else
        {
            UnityEngine.Random.InitState(randomSeed);
        }

        for(int i = 0; i < blockD.data.Length; ++i)
        {
            ushort r = (ushort)(UnityEngine.Random.value * randomiseRange);
            if (i < randomiseFirstIndex)
            {
                continue;
            }
            if(i > randomiseLastIndex)
            {
                continue;
            }
            blockD.data[i] = r;
        }
    }

    public void WriteDetails()
    {
        sourceMap.WriteMeshDetails(meshDetail, meshIndex);
        sourceMap.WriteBlockDDetails(blockD, blockIndex);
    }
}
