using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(SWarsBuilding))]
[CanEditMultipleObjects]
public class SWarsBuildingEditor : MeshEditor
{
    override protected List<SWars.Vertex> GetVertices(UnityEngine.Object target)
    {
        SWarsBuilding b = (SWarsBuilding)target;
        return b.loadedMap.vertices;
    }

    override protected void GetTriInfo(UnityEngine.Object target, ref List<SWars.Tri> tris, ref List<SWars.TriTextureInfo> triTexInfo, ref int triIndexBegin, ref int triIndexCount)
    {
        SWarsBuilding b = (SWarsBuilding)target;
  
        triIndexBegin = b.loadedMap.meshes[b.buildingIndex].triIndexBegin;
        triIndexCount = b.loadedMap.meshes[b.buildingIndex].triIndexCount;

        tris = b.loadedMap.tris;
        triTexInfo = b.loadedMap.triTexInfo;
    }

    override protected void GetQuadInfo(UnityEngine.Object target, ref List<SWars.Quad> quads, ref List<SWars.QuadTextureInfo> quadTexInfo, ref int quadIndexBegin, ref int quadIndexCount)
    {
        SWarsBuilding b = (SWarsBuilding)target;

        quadIndexBegin = b.loadedMap.meshes[b.buildingIndex].quadIndexBegin;
        quadIndexCount = b.loadedMap.meshes[b.buildingIndex].quadIndexCount;

        quads = b.loadedMap.quads;
        quadTexInfo = b.loadedMap.quadTexInfo;
    }

    override protected List<int> GetFaceLookup(UnityEngine.Object target)
    {
        SWarsBuilding b = (SWarsBuilding)target;

        return b.faceLookup;
    }
}
