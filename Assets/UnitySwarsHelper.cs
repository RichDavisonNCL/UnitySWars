using System;
using UnityEngine;
using System.Collections.Generic;

namespace SWars
{
    class Unity
    {
        static public Vector3 SwarsVertexToVec3(ref List<Vertex> allVerts, int i)
        {
            return new Vector3(allVerts[i].x, allVerts[i].y, allVerts[i].z);
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
    }
}
