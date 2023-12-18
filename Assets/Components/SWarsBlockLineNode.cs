using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWars;
public class SWarsBlockLineNode : MonoBehaviour
{
    public int index = 0;
    public SWars.Map map;

    public void SetBlockLineDetails(SWars.Map source, int blockID)
    {
        map     = source;
        index   = blockID;

        Vector3 startPos = new Vector3( map.blockLines[index].xStart,
                                        map.blockLines[index].yStart,
                                        map.blockLines[index].zStart);

        Vector3 endPos = new Vector3(   map.blockLines[index].xEnd,
                                        map.blockLines[index].yEnd,
                                        map.blockLines[index].zEnd);

        transform.GetChild(0).localPosition = startPos;
        transform.GetChild(1).localPosition = endPos;
    }

    public void UpdateBlockData()
    {
        Vector3 startPos = transform.GetChild(0).localPosition;
        Vector3 endPos = transform.GetChild(1).localPosition;

        SWars.NPCBlockLine blockLine = map.blockLines[index];

        blockLine.xStart = (short)startPos.x;
        blockLine.yStart = (short)startPos.y;
        blockLine.zStart = (short)startPos.z;

        blockLine.xEnd = (short)endPos.x;
        blockLine.yEnd = (short)endPos.y;
        blockLine.zEnd = (short)endPos.z;

        map.blockLines[index] = blockLine;
    }

    void Update()
    {
        UpdateBlockData();
        Vector3 startPos = transform.GetChild(0).position;
        Vector3 endPos = transform.GetChild(1).position;

        int primIndex = map.blockLines[index].primIndex;

        bool isQuad = SWars.Functions.BlockLineIsQuad(map.blockLines[index]);

        Debug.DrawLine(startPos, endPos, isQuad ? Color.cyan : Color.red);

        if (isQuad)
        {
            primIndex = SWars.Functions.BlockLineQuadIndex(map.blockLines[index]);

            int meshID = map.GetMeshForQuad(primIndex);
            if(meshID < 0)
            {
                return;
            }
            MeshDetails m = map.meshes[meshID];

            if (primIndex < map.quads.Count)
            {
                SWars.Quad t = map.quads[primIndex];

                List<Vector3> verts = new List<Vector3>();

                SWars.Unity.AddQuadVertices(ref verts, ref map.vertices, ref t);
        
                if (meshID >= 0)
                {
                    Vector3 meshPos = new Vector3(m.xPosition, m.yPosition, m.zPosition);
                    verts[0] += meshPos;
                    verts[1] += meshPos;
                    verts[2] += meshPos;
                    verts[3] += meshPos;
                }

                verts[0] = Vector3.Scale(verts[0], transform.lossyScale);
                verts[1] = Vector3.Scale(verts[1], transform.lossyScale);
                verts[2] = Vector3.Scale(verts[2], transform.lossyScale);
                verts[3] = Vector3.Scale(verts[3], transform.lossyScale);

                Debug.DrawLine(verts[0], verts[1], Color.cyan);
                Debug.DrawLine(verts[0], verts[2], Color.cyan);

                Debug.DrawLine(verts[3], verts[1], Color.cyan);
                Debug.DrawLine(verts[3], verts[2], Color.cyan);
            }
        }
        else
        {
            if (primIndex < map.tris.Count)
            {
                primIndex = SWars.Functions.BlockLineTriIndex(map.blockLines[index]);
                SWars.Tri t = map.tris[primIndex];

                List<Vector3> verts = new List<Vector3>();

                SWars.Unity.AddTriVertices(ref verts, ref map.vertices, ref t);

                int meshID = map.GetMeshForTri(primIndex);

                if (meshID >= 0)
                {
                    MeshDetails m = map.meshes[meshID];

                    Vector3 meshPos = new Vector3(m.xPosition, m.yPosition, m.zPosition);
                    verts[0] += meshPos;
                    verts[1] += meshPos;
                    verts[2] += meshPos;
                }

                verts[0] = Vector3.Scale(verts[0], transform.lossyScale);
                verts[1] = Vector3.Scale(verts[1], transform.lossyScale);
                verts[2] = Vector3.Scale(verts[2], transform.lossyScale);

                Debug.DrawLine(verts[0], verts[1], Color.red);
                Debug.DrawLine(verts[1], verts[2], Color.red);
                Debug.DrawLine(verts[2], verts[0], Color.red);
            }
        }
    }
}
