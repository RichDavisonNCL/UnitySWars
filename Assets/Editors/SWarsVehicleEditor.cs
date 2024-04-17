using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SWars;
using UnityEditor.UI;
using System;

[CustomEditor(typeof(SWarsVehicle))]
[CanEditMultipleObjects]
[ExecuteInEditMode]
public class SWarsVehicleEditor : MeshEditor
{
    override protected List<SWars.Vertex> GetVertices(UnityEngine.Object target)
    {
        SWarsVehicle v = (SWarsVehicle)target;
        SWars.VehicleMeshFile vehicleFile = v.vehicleFile;

        return vehicleFile.vertices;
    }

    override protected List<int> GetFaceLookup(UnityEngine.Object target)
    {
        SWarsVehicle v = (SWarsVehicle)target;
        return v.faceLookup;
    }

    override protected void GetTriInfo(UnityEngine.Object target, ref List<SWars.Tri> tris, ref List<SWars.TriTextureInfo> triTexInfo, ref int triIndexBegin, ref int triIndexCount)
    {
        SWarsVehicle v = (SWarsVehicle)target;
        SWars.VehicleMeshFile vehicleFile = v.vehicleFile;
        SWars.MeshDetails source = vehicleFile.meshes[v.vehicleIndex];

        triIndexBegin = source.triIndexBegin;
        triIndexCount = source.triIndexCount;

        tris = vehicleFile.tris;
        triTexInfo = vehicleFile.triTex;
    }

    override protected void GetQuadInfo(UnityEngine.Object target, ref List<SWars.Quad> quads, ref List<SWars.QuadTextureInfo> quadTexInfo, ref int quadIndexBegin, ref int quadIndexCount)
    {
        SWarsVehicle v = (SWarsVehicle)target;
        SWars.VehicleMeshFile vehicleFile = v.vehicleFile;
        SWars.MeshDetails source = vehicleFile.meshes[v.vehicleIndex];

        quadIndexBegin = source.quadIndexBegin;
        quadIndexCount = source.quadIndexCount;

        quads = vehicleFile.quads;
        quadTexInfo = vehicleFile.quadTex;
    }
}

