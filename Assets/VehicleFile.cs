using System.Collections.Generic;
using SWars;
using UnityEditor;
using UnityEngine;

public class VehicleFile : MonoBehaviour
{
    [SerializeField]
    List<Mesh> loadedVehicles;

    [SerializeField]
    SWarsTextureIO textureIO;

    [SerializeField]
    string filepath = "GAME/QDATA/PRIMVEH.OBJ";

    [SerializeField]
    SWars.VehicleMeshFile vehicleData;

    public void LoadVehicles()
    {
        loadedVehicles.Clear();

        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        LoadVehicles(SWars.FilePath.Get() + filepath);
    }

    public void LoadVehicles(string filename)
    {
        vehicleData = new SWars.VehicleMeshFile();
        SWars.Vehicles.LoadVehicleFile(filename, ref vehicleData);

        TestQuads(vehicleData.quads, vehicleData.vertices);

        for (int i = 0; i < vehicleData.header.numMeshes; ++i)
        {
            List<int> faceLookup = new List<int>();

            Mesh vehicle = SWars.Unity.CreateMesh(vehicleData.meshes[i], vehicleData.vertices, vehicleData.tris, vehicleData.triTex, vehicleData.quads, vehicleData.quadTex, faceLookup);

            vehicle.name = filename;

            loadedVehicles.Add(vehicle);

            GameObject o = new GameObject();
            o.name = "Vehicle " + i;

            o.transform.parent          = transform;
            o.transform.localScale      = new Vector3(1, 1, 1);
            o.transform.localPosition   = new Vector3(i * 512, 0, 0);

            MeshRenderer r = o.AddComponent<MeshRenderer>();
            MeshFilter f = o.AddComponent<MeshFilter>();
            f.mesh = loadedVehicles[i];
            r.materials = textureIO.gameMaterial;

            SWarsVehicle vc = o.AddComponent<SWarsVehicle>();
            vc.SetFileDetails(vehicleData, i);
            vc.SetFaceLookup(faceLookup);

            Undo.RegisterCreatedObjectUndo(o, "Create Vehicle");
        }
    }

    public void SaveVehicles(string filename)
    {
        //Holds all the vertex/tri/quad lists
        VehicleMeshFile meshFile = new VehicleMeshFile();

        //Go through each vehicle in turn, adding vertices and details
        for (int i = 0; i < loadedVehicles.Count; ++i)
        {
            Mesh m = loadedVehicles[i];
            MeshDetails md = new MeshDetails();

            //Add vertices from the mesh as-is

            for (int v = 0; v < m.vertices.Length; ++v)
            {
                meshFile.vertices.Add(SWars.Unity.UnityVec3ToVertex(m.vertices[v]));
            }
            //Now to go through each tri pair, determine if they are 2 tris or 1 quad...

            int t = 0;
            for (; t < m.triangles.Length; t += 6)
            {
                int sharedVerts = 0;
                for (int a = 0; a < 3; ++a)
                {
                    for (int b = 0; b < 3; ++b)
                    {
                        if (m.triangles[t + a] == m.triangles[t + b])
                        {
                            sharedVerts++;
                        }
                    }
                }
                if (sharedVerts == 0)
                {
                    AddTri(meshFile, m.triangles[t + 0], m.triangles[t + 1], m.triangles[t + 2]);
                    AddTri(meshFile, m.triangles[t + 3], m.triangles[t + 4], m.triangles[t + 5]);

                }
            }
            if (t < m.triangles.Length - 3)
            {
                //Trailing tri
            }

            meshFile.meshes.Add(md);
        };

        SWars.Vehicles.SaveVehicleFile(filename, meshFile);
    }

    public static void AddMeshToVehicleFile(Mesh inputMesh, ref SWars.VehicleMeshFile file)
    {
    }

    static void TestQuads(List<SWars.Quad> quads, List<SWars.Vertex> vertices)
    {
        float maxPlaneDistance = 0.0f;
        float maxVertexDistance = 0.0f;
        float maxAngle = 0.0f;
        float minAngle = float.MaxValue;

        int maxUnknown1 = 0;
        int maxUnknown2 = 0;
        int maxUnknown4 = 0;
        int maxUnknown5 = 0;
        int maxUnknown6 = 0;
        int maxUnknown7 = 0;

        foreach (SWars.Quad quad in quads)
        {
            SWars.Vertex v0 = vertices[quad.vert0Index];
            SWars.Vertex v1 = vertices[quad.vert1Index];
            SWars.Vertex v2 = vertices[quad.vert2Index];
            SWars.Vertex v3 = vertices[quad.vert3Index];

            Vector3 v0u = SWars.Unity.SwarsVertexToVec3(v0);
            Vector3 v1u = SWars.Unity.SwarsVertexToVec3(v1);
            Vector3 v2u = SWars.Unity.SwarsVertexToVec3(v2);
            Vector3 v3u = SWars.Unity.SwarsVertexToVec3(v3);

            Plane p = new Plane();
            p.Set3Points(v0u, v2u, v1u);

            maxPlaneDistance = Mathf.Max(maxPlaneDistance, Mathf.Abs(p.GetDistanceToPoint(v3u)));

            maxVertexDistance = Mathf.Max(maxVertexDistance, Vector3.Distance(v0u, v1u));
            maxVertexDistance = Mathf.Max(maxVertexDistance, Vector3.Distance(v0u, v2u));
            maxVertexDistance = Mathf.Max(maxVertexDistance, Vector3.Distance(v2u, v1u));
            maxVertexDistance = Mathf.Max(maxVertexDistance, Vector3.Distance(v2u, v3u));

            float d0 = Mathf.Acos(Vector3.Dot(v1u - v0u, v2u - v0u)) * Mathf.Rad2Deg;
            float d1 = Mathf.Acos(Vector3.Dot(v2u - v3u, v1u - v3u)) * Mathf.Rad2Deg;
            float d2 = Mathf.Acos(Vector3.Dot(v0u - v1u, v3u - v1u)) * Mathf.Rad2Deg;
            float d3 = Mathf.Acos(Vector3.Dot(v3u - v2u, v0u - v2u)) * Mathf.Rad2Deg;

            maxAngle = Mathf.Max(d0, maxAngle);
            maxAngle = Mathf.Max(d1, maxAngle);
            maxAngle = Mathf.Max(d2, maxAngle);
            maxAngle = Mathf.Max(d3, maxAngle);

            minAngle = Mathf.Min(d0, minAngle);
            minAngle = Mathf.Min(d1, minAngle);
            minAngle = Mathf.Min(d2, minAngle);
            minAngle = Mathf.Min(d3, minAngle);

            if (d0 > 180.0f || d1 > 180.0f || d2 > 180.0f || d3 > 180.0f)
            {
                Debug.Log("Quad is concave!");
            }

            maxUnknown1 = Mathf.Max(quad.flags, maxUnknown1);
            maxUnknown2 = Mathf.Max(quad.paletteIndex, maxUnknown2);
            maxUnknown4 = Mathf.Max(quad.unknown4, maxUnknown4);
            maxUnknown5 = Mathf.Max(quad.unknown5, maxUnknown5);
            maxUnknown6 = Mathf.Max(quad.unknown6, maxUnknown6);
            maxUnknown7 = Mathf.Max(quad.unknown7, maxUnknown7);
        }
        //Debug.Log("Max plane distance = " + maxPlaneDistance);
        //Debug.Log("Max Vertex distance = " + maxVertexDistance);

        Debug.Log("Max quad angle = " + maxAngle);
        Debug.Log("Min quad angle = " + minAngle);

        Debug.Log("MaxUnknown1 " + maxUnknown1);
        Debug.Log("MaxUnknown2 " + maxUnknown2);
        Debug.Log("MaxUnknown4 " + maxUnknown4);
        Debug.Log("MaxUnknown5 " + maxUnknown5);
        Debug.Log("MaxUnknown6 " + maxUnknown6);
        Debug.Log("MaxUnknown7 " + maxUnknown7);
    }

    void AddTri(VehicleMeshFile f, int a, int b, int c)
    {
        Tri t = new Tri();
        // t.

        f.tris.Add(t);
    }
}
