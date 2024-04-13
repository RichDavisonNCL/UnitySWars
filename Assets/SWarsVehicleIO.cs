using System.Collections;
using System.Collections.Generic;
using SWars;
using UnityEditor;
using UnityEngine;

public class SWarsVehicleIO : MonoBehaviour
{
    [SerializeField]
    List<Mesh> loadedVehicles;

    [SerializeField]
    SWarsTextureIO textureIO;

    public void LoadVehicles()
    {
        loadedVehicles.Clear();

        foreach (Transform child in transform)
        {
            DestroyImmediate(child.gameObject);
        }

        VehicleFile.LoadVehicles(SWars.FilePath.Get() + "GAME/QDATA/PRIMVEH.OBJ", ref loadedVehicles);

        transform.parent = transform;
        transform.localScale = Vector3.one;

        for (int i = 0; i < loadedVehicles.Count; ++i)
        {
            GameObject o = new GameObject();
            o.name = "Vehicle " + i;
            o.transform.parent = transform;

            o.transform.localScale      = new Vector3(1, 1, 1);
            o.transform.localPosition   = new Vector3(i * 512, 0, 0);

            MeshRenderer r = o.AddComponent<MeshRenderer>();
            MeshFilter f = o.AddComponent<MeshFilter>();
            f.mesh = loadedVehicles[i];
            r.materials = textureIO.gameMaterial;

            Undo.RegisterCreatedObjectUndo(o, "Create Vehicle");
        }
    }

    void AddTri(VehicleMeshFile f, int a, int b, int c)
    {
        Tri t = new Tri();
       // t.

        f.tris.Add(t);
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

            for(int v = 0; v < m.vertices.Length; ++v)
            {
                meshFile.vertices.Add(SWars.Unity.UnityVec3ToVertex(m.vertices[v]));
            }
            //Now to go through each tri pair, determine if they are 2 tris or 1 quad...

            int t = 0;
            for (; t < m.triangles.Length; t+=6)
            {
                int sharedVerts = 0;
                for(int a = 0; a < 3; ++a)
                {
                    for (int b = 0; b < 3; ++b)
                    {
                        if (m.triangles[t + a] == m.triangles[t + b])
                        {
                            sharedVerts++;
                        }
                    }
                }
                if(sharedVerts == 0)
                {
                    AddTri(meshFile, m.triangles[t + 0], m.triangles[t + 1], m.triangles[t + 2]);
                    AddTri(meshFile, m.triangles[t + 3], m.triangles[t + 4], m.triangles[t + 5]);

                }
            }
            if(t < m.triangles.Length-3)
            {
                //Trailing tri
            }
           // m.vertices


            meshFile.meshes.Add(md);
        };

        SWars.Vehicles.SaveVehicleFile(filename, meshFile);
    }


}
