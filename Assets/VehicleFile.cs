using System.Collections.Generic;
using UnityEngine;

public class VehicleFile
{
    public static void LoadVehicles(string filename, ref List<Mesh> allMeshes)
    {
        SWars.VehicleMeshFile vehicleData = new SWars.VehicleMeshFile();
        SWars.Vehicles.LoadVehicleFile(filename, ref vehicleData);

        for (int i = 0; i < vehicleData.header.numMeshes; ++i)
        {
            List<int>[] meshIndices = new List<int>[5];

            for (int j = 0; j < 5; ++j)
            {
                meshIndices[j] = new List<int>();
            }

            List<Vector3>   meshVertices    = new List<Vector3>();
            List<Vector2>   meshTexCoords   = new List<Vector2>();

            SWars.MeshDetails source = vehicleData.meshes[i];

            for(int v = 0; v < source.triIndexNum; ++v)
            {
                SWars.Tri tri        = vehicleData.tris[source.triIndexBegin + v];
                SWars.TriTextureInfo triUV  = vehicleData.triTex[tri.faceIndex];

                SWars.Unity.AddTriIndices(ref meshIndices[triUV.texNum], meshVertices.Count);
                SWars.Unity.AddTriTexCoords(ref meshTexCoords, triUV);
                SWars.Unity.AddTriVertices(ref meshVertices, ref vehicleData.vertices, ref tri);
            }
            for (int v = 0; v < source.quadIndexNum; ++v)
            {
                SWars.Quad quad          = vehicleData.quads[source.quadIndexBegin + v];
                SWars.QuadTextureInfo quadUV    = vehicleData.quadTex[quad.faceIndex];

                SWars.Unity.AddQuadIndices(ref meshIndices[quadUV.texNum], meshVertices.Count);
                SWars.Unity.AddQuadTexCoords(ref meshTexCoords, quadUV);
                SWars.Unity.AddQuadVertices(ref meshVertices, ref vehicleData.vertices, ref quad);
            }

            Mesh vehicle = new Mesh();
            vehicle.SetVertices(meshVertices);
            vehicle.SetUVs(0, meshTexCoords);
            vehicle.subMeshCount = 5;
            for (int j = 0; j < 5; ++j)
            {
                vehicle.SetIndices(meshIndices[j], MeshTopology.Triangles, j);
            }
            vehicle.name = filename;

            allMeshes.Add(vehicle);
        }
    }

    public static void AddMeshToVehicleFile(Mesh inputMesh, ref SWars.VehicleMeshFile file)
    {


    }

}
