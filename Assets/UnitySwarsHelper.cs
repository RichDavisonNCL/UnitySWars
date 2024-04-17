using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering;

namespace SWars
{
    class Unity
    {
        static public Vector3 SwarsVertexToVec3(ref List<Vertex> allVerts, int i)
        {
            return new Vector3(allVerts[i].x, allVerts[i].y, allVerts[i].z);
        }

        static public Vector3 SwarsVertexToVec3(Vertex v)
        {
            return new Vector3(v.x, v.y,v.z);
        }

        static public void AddQuadTexCoords(ref List<Vector2> coords, SWars.QuadTextureInfo texInfo)
        {
            coords.Add(new Vector2(texInfo.v1x, texInfo.v1y) / 255.0f);
            coords.Add(new Vector2(texInfo.v2x, texInfo.v2y) / 255.0f);
            coords.Add(new Vector2(texInfo.v3x, texInfo.v3y) / 255.0f);
            coords.Add(new Vector2(texInfo.v4x, texInfo.v4y) / 255.0f);
        }

        static public void AddTriTexCoords(ref List<Vector2> coords, SWars.TriTextureInfo texInfo)
        {
            coords.Add(new Vector2(texInfo.v1x, texInfo.v1y) / 255.0f);
            coords.Add(new Vector2(texInfo.v2x, texInfo.v2y) / 255.0f);
            coords.Add(new Vector2(texInfo.v3x, texInfo.v3y) / 255.0f);
        }

        static public void AddQuadVertices(ref List<Vector3> coords, ref List<Vertex> allVerts,ref SWars.Quad quad)
        {
            coords.Add(SwarsVertexToVec3(ref allVerts, quad.vert0Index));
            coords.Add(SwarsVertexToVec3(ref allVerts, quad.vert1Index));
            coords.Add(SwarsVertexToVec3(ref allVerts, quad.vert2Index));
            coords.Add(SwarsVertexToVec3(ref allVerts, quad.vert3Index));
        }

        static public void AddTriVertices(ref List<Vector3> coords, ref List<Vertex> allVerts, ref SWars.Tri tri)
        {
            coords.Add(SwarsVertexToVec3(ref allVerts, tri.vert0Index));
            coords.Add(SwarsVertexToVec3(ref allVerts, tri.vert1Index));
            coords.Add(SwarsVertexToVec3(ref allVerts, tri.vert2Index));
        }

        static public void AddTriColours(ref List<Color> colors, ref SWars.Tri tri, bool useFaceColours, Color[] paletteColours)
        {
            Color c = useFaceColours ? paletteColours[tri.paletteIndex & 255] : Color.white;
            colors.Add(c);
            colors.Add(c);
            colors.Add(c);
        }

        static public void AddQuadColours(ref List<Color> colors, ref SWars.Quad quad, bool useFaceColours, Color[] paletteColours)
        {
            Color c = useFaceColours ? paletteColours[quad.paletteIndex & 255] : Color.white;
            colors.Add(c);
            colors.Add(c);
            colors.Add(c);
            colors.Add(c);
        }

        static public void AddTriIndices(ref List<int> to, int lastIndex)
        {
            to.Add(lastIndex);
            to.Add(lastIndex + 2);
            to.Add(lastIndex + 1);
        }

        static public void AddQuadIndices(ref List<int> to, int lastIndex)
        {
            to.Add(lastIndex);
            to.Add(lastIndex + 2);
            to.Add(lastIndex + 1);

            to.Add(lastIndex + 2);
            to.Add(lastIndex + 3);
            to.Add(lastIndex + 1);
        }

        static public bool UnityToSWarsMesh(Mesh m, ref SWars.MeshDetails swarsMesh)
        {
            swarsMesh = new MeshDetails();


            return true;
        }

        static public Vertex UnityVec3ToVertex(Vector3 v)
        {
            Vertex vertex = new Vertex();

            vertex.x = (short)v.x;
            vertex.y = (short)v.y;
            vertex.z = (short)v.z;

            vertex.unknown1 = 0;
            vertex.unknown2 = 0;

            return vertex;
        }

        static public Mesh CreateMesh(SWars.MeshDetails source, List<Vertex> vertices, List<Tri> tris, List<TriTextureInfo> triTex, List<Quad> quads, List<QuadTextureInfo> quadTex, List<int> faceLookup)
        {
            string palFile = SWars.FilePath.Get() + "GAME/DATA/" + "PAL0.DAT";

            Color[] paletteColours = TextureLoader.PaletteFileToColours(palFile);

            List<int>[] triIndices = new List<int>[5];
            List<int>[] quadIndices = new List<int>[5];

            List<int> newMeshIndices = new List<int>();

            for (int j = 0; j < 5; ++j)
            {
                triIndices[j] = new List<int>();
                quadIndices[j] = new List<int>();
            }

            List<Vector3> meshVertices = new List<Vector3>();
            List<Vector2> meshTexCoords = new List<Vector2>();

            List<Color> meshColors = new List<Color>();

            for (int v = 0; v < source.triIndexCount; ++v)
            {
                SWars.Tri tri = tris[source.triIndexBegin + v];
                SWars.TriTextureInfo triUV = triTex[tri.faceIndex];

                int layer = triUV.texNum;
                if (layer < 0 || layer > 4)
                {
                    layer = 0;
                }

                //if(tri.flags == 512)
                {
                    triIndices[layer].Add(v);
                }

                //if(((tri.flags) & 1024) == 0)
                //{
                //    Debug.Log("Tri without 1024 bit set! " + tri.flags);
                //}
            }
            for (int v = 0; v < source.quadIndexCount; ++v)
            {
                SWars.Quad quad = quads[source.quadIndexBegin + v];
                SWars.QuadTextureInfo quadUV = quadTex[quad.faceIndex];

                int layer = quadUV.texNum;
                if (layer < 0 || layer > 4)
                {
                    layer = 0;
                }

                //if(quad.flags == 512)
                {
                    quadIndices[layer].Add(v);
                }

                //if (((quad.flags) & 1024) == 0)
                //{
                //    Debug.Log("Tri without 1024 bit set! " + quad.flags);   

                //}
            }

            for (int t = 0; t < 5; ++t)
            {
                for (int v = 0; v < triIndices[t].Count; ++v)
                {
                    faceLookup.Add(triIndices[t][v]);
                    SWars.Tri tri = tris[source.triIndexBegin + triIndices[t][v]];
                    SWars.TriTextureInfo triUV = triTex[tri.faceIndex];

                    bool useColour = (tri.flags & 256) > 0;
                    //useColour = true;

                    SWars.Unity.AddTriIndices(ref newMeshIndices, meshVertices.Count);
                    SWars.Unity.AddTriTexCoords(ref meshTexCoords, triUV);
                    SWars.Unity.AddTriVertices(ref meshVertices, ref vertices, ref tri);
                    SWars.Unity.AddTriColours(ref meshColors, ref tri, !useColour, paletteColours);
                }
                for (int v = 0; v < quadIndices[t].Count; ++v)
                {
                    faceLookup.Add(-1 - quadIndices[t][v]);
                    faceLookup.Add(-1 - quadIndices[t][v]);
                    SWars.Quad quad = quads[source.quadIndexBegin + quadIndices[t][v]];
                    SWars.QuadTextureInfo quadUV = quadTex[quad.faceIndex];

                    bool useColour = (quad.flags & 256) > 0;
                    //useColour = true;

                    SWars.Unity.AddQuadIndices(ref newMeshIndices, meshVertices.Count);
                    SWars.Unity.AddQuadTexCoords(ref meshTexCoords, quadUV);
                    SWars.Unity.AddQuadVertices(ref meshVertices, ref vertices, ref quad);
                    SWars.Unity.AddQuadColours(ref meshColors, ref quad, !useColour, paletteColours);
                }
            }

            Mesh vehicle = new Mesh();
            vehicle.SetVertices(meshVertices);
            vehicle.SetColors(meshColors);
            vehicle.SetUVs(0, meshTexCoords);

            vehicle.triangles = newMeshIndices.ToArray();

            vehicle.subMeshCount = 5;
            int startingIndex = 0;
            for (int j = 0; j < 5; ++j)
            {
                int indexCount = triIndices[j].Count * 3;
                indexCount += quadIndices[j].Count * 2 * 3;

                SubMeshDescriptor d = new SubMeshDescriptor(startingIndex, indexCount, MeshTopology.Triangles);
                vehicle.SetSubMesh(j, d);

                startingIndex += indexCount;
            }

            return vehicle;
        }
    }
}
