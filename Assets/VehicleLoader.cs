using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class VehicleLoader// : MonoBehaviour
{
    public static void LoadVehicles(string filename, ref List<Mesh> allMeshes)
    {
        using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
        {
            SWars.VehicleHeader header = SwarsFunctions.ByteToType<SWars.VehicleHeader>(reader);

            List<SWars.Vertex>   vertices    = new List<SWars.Vertex>();
            List<SWars.Tri>      tris        = new List<SWars.Tri>();
            List<SWars.Quad>     quads       = new List<SWars.Quad>();
            List<SWars.MeshDetails>     meshes      = new List<SWars.MeshDetails>();

            List<SWars.QuadTextureInfo> quadTex     = new List<SWars.QuadTextureInfo>();
            List<SWars.TriTextureInfo>  triTex      = new List<SWars.TriTextureInfo>();

            for(int i = 0; i < header.numVerts; ++i)
            {
                vertices.Add(SwarsFunctions.ByteToType<SWars.Vertex>(reader));
            }
            for (int i = 0; i < header.numTris; ++i)
            {
                tris.Add(SwarsFunctions.ByteToType<SWars.Tri>(reader));
            }
            for (int i = 0; i < header.numQuads; ++i)
            {
                quads.Add(SwarsFunctions.ByteToType<SWars.Quad>(reader));
            }
            for (int i = 0; i < header.numMeshes; ++i)
            {
                meshes.Add(SwarsFunctions.ByteToType<SWars.MeshDetails>(reader));
            }
            for (int i = 0; i < header.numQuadUV; ++i)
            {
                quadTex.Add(SwarsFunctions.ByteToType<SWars.QuadTextureInfo>(reader));
            }
            for (int i = 0; i < header.numTriUV; ++i)
            {
                triTex.Add(SwarsFunctions.ByteToType<SWars.TriTextureInfo>(reader));
            }

            for (int i = 0; i < header.numMeshes; ++i)
            {
                List<int>[] meshIndices = new List<int>[5];

                for (int j = 0; j < 5; ++j)
                {
                    meshIndices[j] = new List<int>();
                }

                List<Vector3>   meshVertices    = new List<Vector3>();
                List<Vector2>   meshTexCoords   = new List<Vector2>();

                SWars.MeshDetails source = meshes[i];

                int numVerts = source.lastVertIndex - source.firstVertIndex;
                int numQuads = source.quadIndexNum;

                for(int v = 0; v < source.triIndexNum; ++v)
                {
                    SWars.Tri tri        = tris[source.triIndexBegin + v];
                    SWars.TriTextureInfo triUV  = triTex[tri.faceIndex];

                    meshTexCoords.Add(new Vector2(triUV.v1x, triUV.v1y) / 255.0f);
                    meshTexCoords.Add(new Vector2(triUV.v2x, triUV.v2y) / 255.0f);
                    meshTexCoords.Add(new Vector2(triUV.v3x, triUV.v3y) / 255.0f);

                    meshIndices[triUV.texNum].Add(meshVertices.Count);
                    meshIndices[triUV.texNum].Add(meshVertices.Count + 2);
                    meshIndices[triUV.texNum].Add(meshVertices.Count + 1);

                    meshVertices.Add(new Vector3(vertices[tri.vert0Index].x, vertices[tri.vert0Index].y, vertices[tri.vert0Index].z));
                    meshVertices.Add(new Vector3(vertices[tri.vert1Index].x, vertices[tri.vert1Index].y, vertices[tri.vert1Index].z));
                    meshVertices.Add(new Vector3(vertices[tri.vert2Index].x, vertices[tri.vert2Index].y, vertices[tri.vert2Index].z));
                }
                for (int v = 0; v < source.quadIndexNum; ++v)
                {
                    SWars.Quad quad          = quads[source.quadIndexBegin + v];
                    SWars.QuadTextureInfo quadUV    = quadTex[quad.faceIndex];

                    meshIndices[quadUV.texNum].Add(meshVertices.Count + 0);
                    meshIndices[quadUV.texNum].Add(meshVertices.Count + 2);
                    meshIndices[quadUV.texNum].Add(meshVertices.Count + 1);

                    meshIndices[quadUV.texNum].Add(meshVertices.Count + 2);
                    meshIndices[quadUV.texNum].Add(meshVertices.Count + 3);
                    meshIndices[quadUV.texNum].Add(meshVertices.Count + 1);

                    meshTexCoords.Add(new Vector2(quadUV.v1x, quadUV.v1y) / 255.0f);
                    meshTexCoords.Add(new Vector2(quadUV.v2x, quadUV.v2y) / 255.0f);
                    meshTexCoords.Add(new Vector2(quadUV.v3x, quadUV.v3y) / 255.0f);
                    meshTexCoords.Add(new Vector2(quadUV.v4x, quadUV.v4y) / 255.0f);

                    meshVertices.Add(new Vector3(vertices[quad.vert0Index].x, vertices[quad.vert0Index].y, vertices[quad.vert0Index].z));
                    meshVertices.Add(new Vector3(vertices[quad.vert1Index].x, vertices[quad.vert1Index].y, vertices[quad.vert1Index].z));
                    meshVertices.Add(new Vector3(vertices[quad.vert2Index].x, vertices[quad.vert2Index].y, vertices[quad.vert2Index].z));
                    meshVertices.Add(new Vector3(vertices[quad.vert3Index].x, vertices[quad.vert3Index].y, vertices[quad.vert3Index].z));
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
    }
}
