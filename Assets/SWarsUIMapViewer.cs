using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SWarsUIMapViewer : MonoBehaviour
{
    [SerializeField]
    bool drawCityCubes = false;
    // Start is called before the first frame update
    void Start()
    {
        CreateMapMesh("GAME/DATA/MAPOUT00.DAT");
        CreateMapMesh("GAME/DATA/MAPOUT01.DAT");
        CreateMapMesh("GAME/DATA/MAPOUT02.DAT");
        CreateMapMesh("GAME/DATA/MAPOUT03.DAT");
        CreateMapMesh("GAME/DATA/MAPOUT04.DAT");

        CreateMapMesh("GAME/DATA/MAPOUT05.DAT");
        CreateMapMesh("GAME/DATA/MAPINSID.DAT");

        CreateCitiesMesh("GAME/DATA/CITIES.DAT");
    }


    void CreateMapMesh(string inputFile)
    {
        Mesh m = new Mesh();

        List<Vector3> meshCoords = new List<Vector3>();
        List<int> meshIndices = new List<int>();

        SWars.UIFunctions.CreateMapMesh(inputFile,
            (x,y,z) => meshCoords.Add(new Vector3(x, y, z)),
            (i) => meshIndices.Add((int)i)
        );

        m.SetVertices(meshCoords);
        m.SetIndices(meshIndices, MeshTopology.Lines, 0);
        m.name = inputFile;

        CreateMeshObject(m);
    }

    void CreateCitiesMesh(string inputFile)
    {
        List<SWars.UICityData> loadedCities = new List<SWars.UICityData>();
        SWars.UIFunctions.LoadCityInfo(inputFile, ref loadedCities);

        Mesh m = new Mesh();

        List<Vector3> meshCoords = new List<Vector3>();
        List<int> meshIndices = new List<int>();

        int index = 0;
        foreach (SWars.UICityData c in loadedCities)
        {
            meshCoords.Add(new Vector3(c.x , -c.y, 0));
            meshIndices.Add(index);
            ++index;
        }
        m.SetVertices(meshCoords);
        m.SetIndices(meshIndices, MeshTopology.Points, 0);
        m.name = inputFile;
        CreateMeshObject(m);

        if (drawCityCubes)
        {
            int i = 0;
            foreach (Vector3 v in m.vertices)
            {
                GameObject city = GameObject.CreatePrimitive(PrimitiveType.Cube);
                city.name = "City " + i++;
                city.transform.parent = transform;
                city.transform.localScale = Vector3.one;
                city.transform.localPosition = v;
            }
        }
    }

    void CreateMeshObject(Mesh m)
    {
        GameObject o = new GameObject();
        o.name = m.name;
        o.transform.parent = gameObject.transform;
        o.transform.localPosition = new Vector3(0, 0, 0);
        o.transform.localScale = Vector3.one;
        MeshRenderer r  = o.AddComponent<MeshRenderer>();
        MeshFilter f    = o.AddComponent<MeshFilter>();
        f.mesh = m;
    }
}
