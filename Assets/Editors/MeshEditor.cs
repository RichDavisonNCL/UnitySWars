using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using SWars;

public class MeshEditor : Editor
{
    bool showTris = false;
    bool showQuads = false;

    int openTri  = -1;
    int openQuad = -1;

    Vector3 bestCentre = Vector3.zero;

    struct DebugLine
    {
        public Vector3  begin;
        public Vector3  end;
        public Color    color;
    }

    List<DebugLine> debugLines = new List<DebugLine>();

    virtual protected List<SWars.Vertex> GetVertices(Object target)
    {
        return null;
    }

    virtual protected void GetTriInfo(Object target, ref List<SWars.Tri> tris, ref List<SWars.TriTextureInfo> triTexInfo, ref int triIndexBegin, ref int triIndexCount)
    {

    }

    virtual protected void GetQuadInfo(Object target, ref List<SWars.Quad> quads, ref List<SWars.QuadTextureInfo> quadTexInfo, ref int quadIndexBegin, ref int quadIndexCount)
    {

    }

    virtual protected List<int> GetFaceLookup(Object target)
    {
        return null;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        debugLines.Clear();

        for (int i = 0; i < targets.Length; ++i)
        {
            List<SWars.Tri> tris = null;
            List<SWars.Quad> quads = null;
    
            List<SWars.TriTextureInfo> triTex = null;
            List<SWars.QuadTextureInfo> quadTex = null;

            List<SWars.Vertex> vertices = GetVertices(targets[i]);

            int triIndexBegin   = 0;
            int quadIndexBegin  = 0;

            int triIndexCount   = 0;
            int quadIndexCount  = 0;

            GetTriInfo(targets[i], ref tris, ref triTex, ref triIndexBegin, ref triIndexCount);
            GetQuadInfo(targets[i], ref quads, ref quadTex, ref quadIndexBegin, ref quadIndexCount);

            MonoBehaviour v = (MonoBehaviour)targets[i];

            showTris = EditorGUILayout.BeginFoldoutHeaderGroup(showTris, "Tris");
            if (showTris)
            {
                for (int f = 0; f < triIndexCount; ++f)
                {
                    SWars.Tri tri = tris[triIndexBegin + f];
                    SWars.TriTextureInfo triUV = triTex[tri.faceIndex];

                    bool isOpen = false;
                    if(f == openTri)
                    {
                        isOpen = true;
                    }

                    isOpen = EditorGUILayout.Foldout(isOpen, "Tri " + f + "(" + (triIndexBegin + f) + ")");

                    if (isOpen)
                    {
                        EditorGUILayout.IntField("vert0Index: ", tri.vert0Index);
                        EditorGUILayout.IntField("vert1Index: ", tri.vert1Index);
                        EditorGUILayout.IntField("vert2Index: ", tri.vert2Index);
                        EditorGUILayout.IntField("faceIndex: ", tri.faceIndex);
                        EditorGUILayout.IntField("flags: ", tri.flags);
                        EditorGUILayout.IntField("paletteIndex: ", tri.paletteIndex);
                        EditorGUILayout.IntField("unknown3: ", tri.unknown3);
                        EditorGUILayout.IntField("unknown4: ", tri.unknown4);
                        EditorGUILayout.IntField("unknown5: ", tri.unknown5);
                        EditorGUILayout.IntField("unknown6: ", tri.unknown6);
                        EditorGUILayout.IntField("vert0LightInfo: ", tri.vert0LightInfo);
                        EditorGUILayout.IntField("vert1LightInfo: ", tri.vert1LightInfo);
                        EditorGUILayout.IntField("vert2LightInfo: ", tri.vert2LightInfo);
                        EditorGUILayout.IntField("unknown10: ", tri.unknown10);
                        EditorGUILayout.IntField("unknown11: ", tri.unknown11);
                        EditorGUILayout.IntField("unknown12: ", tri.unknown12);

                        Mesh m = v.GetComponent<MeshFilter>().sharedMesh;

                        Vector3 v0 = v.transform.TransformPoint(SWars.Unity.SwarsVertexToVec3(vertices[tri.vert0Index]));
                        Vector3 v1 = v.transform.TransformPoint(SWars.Unity.SwarsVertexToVec3(vertices[tri.vert1Index]));
                        Vector3 v2 = v.transform.TransformPoint(SWars.Unity.SwarsVertexToVec3(vertices[tri.vert2Index]));

                        DebugLine a = new DebugLine();
                        a.begin = v0;
                        a.end = v1;
                        a.color = Color.green;
                        debugLines.Add(a);

                        DebugLine b = new DebugLine();
                        b.begin = v1;
                        b.end = v2;
                        b.color = Color.green;
                        debugLines.Add(b);

                        DebugLine c = new DebugLine();
                        c.begin = v2;
                        c.end = v0;
                        c.color = Color.green;
                        debugLines.Add(c);
                    }

                    if(isOpen) {
                        openTri = f;
                    }
                    if(!isOpen && openTri == f)
                    {
                        openTri = -1;
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            showQuads = EditorGUILayout.BeginFoldoutHeaderGroup(showQuads, "Quads");
            if (showQuads)
            {
                for (int f = 0; f < quadIndexCount; ++f)
                {
                    SWars.Quad quad = quads[quadIndexBegin + f];
                    SWars.QuadTextureInfo quadUV = quadTex[quad.faceIndex];

                    bool isOpen = false;
                    if (f == openQuad)
                    {
                        isOpen = true;
                    }

                    isOpen = EditorGUILayout.Foldout(isOpen, "Quad " + f + "(" + (quadIndexBegin + f) + ")");

                    if (isOpen)
                    {
                        EditorGUILayout.IntField("vert0Index: ",    quad.vert0Index);
                        EditorGUILayout.IntField("vert1Index: ",    quad.vert1Index);
                        EditorGUILayout.IntField("vert2Index: ",    quad.vert2Index);
                        EditorGUILayout.IntField("faceIndex: ",     quad.faceIndex);
                        EditorGUILayout.IntField("flags: "      ,   quad.flags);
                        EditorGUILayout.IntField("paletteIndex: ",  quad.paletteIndex);
                        EditorGUILayout.IntField("buildingIndex: ", quad.buildingIndex);
                        EditorGUILayout.IntField("unknown4: ",      quad.unknown4);
                        EditorGUILayout.IntField("unknown5: ",      quad.unknown5);
                        EditorGUILayout.IntField("unknown6: ",      quad.unknown6);
                        EditorGUILayout.IntField("unknown7: ",      quad.unknown7);
                        EditorGUILayout.IntField("vert0LightInfo: ",quad.vert0LightInfo);
                        EditorGUILayout.IntField("vert1LightInfo: ",quad.vert1LightInfo);
                        EditorGUILayout.IntField("vert2LightInfo: ",quad.vert2LightInfo);
                        EditorGUILayout.IntField("vert3LightInfo: ",quad.vert3LightInfo);
                        EditorGUILayout.IntField("unknownStructure1Index: ",     quad.unknownStructure1Index);
                        EditorGUILayout.IntField("unknownStructure4Index: ",     quad.unknownStructure4Index);
                        EditorGUILayout.IntField("allFFFF1: ",     quad.allFFFF1);
                        EditorGUILayout.IntField("allFFFF2: ", quad.allFFFF2);

                        Mesh m = v.GetComponent<MeshFilter>().sharedMesh;

                        Vector3 v0 = v.transform.TransformPoint(SWars.Unity.SwarsVertexToVec3(vertices[quad.vert0Index]));
                        Vector3 v1 = v.transform.TransformPoint(SWars.Unity.SwarsVertexToVec3(vertices[quad.vert1Index]));
                        Vector3 v2 = v.transform.TransformPoint(SWars.Unity.SwarsVertexToVec3(vertices[quad.vert2Index]));
                        Vector3 v3 = v.transform.TransformPoint(SWars.Unity.SwarsVertexToVec3(vertices[quad.vert3Index]));

                        DebugLine a = new DebugLine();
                        a.begin = v0;
                        a.end = v1;
                        a.color = Color.green;
                        debugLines.Add(a);

                        DebugLine b = new DebugLine();
                        b.begin = v0;
                        b.end = v2;
                        b.color = Color.green;
                        debugLines.Add(b);

                        DebugLine c = new DebugLine();
                        c.begin = v2;
                        c.end = v3;
                        c.color = Color.green;
                        debugLines.Add(c);

                        DebugLine d = new DebugLine();
                        d.begin = v3;
                        d.end = v1;
                        d.color = Color.green;
                        debugLines.Add(d);
                    }

                    if (isOpen)
                    {
                        openQuad = f;
                    }
                    if (!isOpen && openQuad == f)
                    {
                        openQuad = -1;
                    }
                }
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }

    public void OnSceneGUI()
    {            
        MonoBehaviour behaviour = target as MonoBehaviour;
        GameObject targetObject = behaviour.gameObject;

        List<SWars.Tri> tris = null;
        List<SWars.Quad> quads = null;

        List<SWars.TriTextureInfo> triTex = null;
        List<SWars.QuadTextureInfo> quadTex = null;

        List<int> faceLookup = GetFaceLookup(target);

        int triIndexBegin = 0;
        int quadIndexBegin = 0;

        int triIndexCount = 0;
        int quadIndexCount = 0;

        GetTriInfo(target, ref tris, ref triTex, ref triIndexBegin, ref triIndexCount);
        GetQuadInfo(target, ref quads, ref quadTex, ref quadIndexBegin, ref quadIndexCount);

        if (openTri >= 0 || openQuad >= 0)
        {
            int flag = 0;
            if(openTri >= 0)
            {
                SWars.Tri tri = tris[triIndexBegin + openTri];
                flag = tri.flags;
            }
            else if(openQuad >= 0)
            {
                SWars.Quad quad = quads[quadIndexBegin + openQuad];
                flag = quad.flags;
            }

            var color = new Color(1, 0.8f, 0.4f, 1);
            Handles.color = color;

            GUIStyle style = new GUIStyle("label");

            style.normal.textColor = color;
            style.fontSize = 25;

            Handles.Label(bestCentre, "" + flag, style);
        }

        foreach (DebugLine l in debugLines)
        {
            Debug.DrawLine(l.begin, l.end, l.color);
        }

        Event e = Event.current;
        if ((e.type == EventType.MouseDrag || e.type == EventType.MouseDown)  && e.button == 0)
        {
            Ray r = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            Mesh m = targetObject.GetComponent<MeshFilter>().sharedMesh;
            if(!m)
            {
                return;
            }

            float maxDist = float.MaxValue;
            int bestTri = -1;

            int[] t = m.triangles;
            Vector3[] verts = m.vertices;

            for(int j = 0; j < t.Length; j+=3)
            {
                Vector3 v0 = targetObject.transform.TransformPoint(m.vertices[ t[j+0]] );
                Vector3 v1 = targetObject.transform.TransformPoint(m.vertices[ t[j+1]] );
                Vector3 v2 = targetObject.transform.TransformPoint(m.vertices[ t[j+2]] );

                object hitPoint = MathUtils.IntersectRayTriangle(r, v0, v1, v2, true);

                if(hitPoint != null)
                {
                    RaycastHit hit = (RaycastHit)hitPoint;

                    if(hit.distance < maxDist)
                    {
                        maxDist = hit.distance;
                        bestTri = j / 3;

                        bestCentre = (v0 + v1 + v2) / 3.0f;
                    }
                }
            }      
            if(bestTri != -1) //We now know the best tri, if any
            {
                int index = faceLookup[bestTri];

                if(index < 0)
                {
                    index += 1;

                    openTri = -1;
                    showTris = false;

                    openQuad = -index;
                    showQuads = true;
                }
                else
                {
                    openTri = index;
                    showTris = true;

                    openQuad = -1;
                    showQuads = false;
                }
                Repaint();

                Event.current.Use();
            }
        }
    }
}

