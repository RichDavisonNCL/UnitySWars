using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SWarsUIMap : MonoBehaviour
{
    [SerializeField]
    bool drawCityCubes = false;
    // Start is called before the first frame update
    void Start()
    {
        CreateMapMesh("Assets/GAME/DATA/MAPOUT00.DAT");
        CreateMapMesh("Assets/GAME/DATA/MAPOUT01.DAT");
        CreateMapMesh("Assets/GAME/DATA/MAPOUT02.DAT");
        CreateMapMesh("Assets/GAME/DATA/MAPOUT03.DAT");
        CreateMapMesh("Assets/GAME/DATA/MAPOUT04.DAT");

        CreateMapMesh("Assets/GAME/DATA/MAPOUT05.DAT");
        CreateMapMesh("Assets/GAME/DATA/MAPINSID.DAT");

        Mesh m = CreateCitiesMesh("Assets/GAME/DATA/CITIES.DAT");

        if (drawCityCubes)
        {
            int i = 0;
            foreach(Vector3 v in m.vertices)
            {
                GameObject city = GameObject.CreatePrimitive(PrimitiveType.Cube);
                city.name = "City " + i++;
                city.transform.parent = transform;
                city.transform.localScale = Vector3.one;
                city.transform.localPosition = v;
            }
        }
    }


    void CreateMapMesh(string inputFile)
    {
        List<SWars.UIMapCoordinate> loadedCoords = new List<SWars.UIMapCoordinate>();

        using (BinaryReader reader = new BinaryReader(File.Open(inputFile, FileMode.Open)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                loadedCoords.Add(SwarsFunctions.ByteToType<SWars.UIMapCoordinate>(reader));
            }
        }
        Mesh m = new Mesh();

        List<Vector3> meshCoords = new List<Vector3>();
        List<int> meshIndices = new List<int>();

        int previousIndex = 0;
        int latestIndex = 0;
        bool reset = true;
        foreach (SWars.UIMapCoordinate c in loadedCoords)
        {
            if (c.x == 0 && c.y == 0) //End the loop!
            {
                reset = true;
            }
            else
            {
                meshCoords.Add(new Vector3(c.x, -c.y, 0));

                previousIndex = latestIndex;
                latestIndex = meshCoords.Count - 1;

                if (reset)
                {
                    reset = false;
                }
                else
                {
                    meshIndices.Add(previousIndex);
                    meshIndices.Add(latestIndex);
                }
            }
        }

        m.SetVertices(meshCoords);
        m.SetIndices(meshIndices, MeshTopology.Lines, 0);
        m.name = inputFile;

        CreateMeshObject(m);
    }

    Mesh CreateCitiesMesh(string inputFile)
    {
        List<SWars.UICItyData> loadedCities = new List<SWars.UICItyData>();

        using (BinaryReader reader = new BinaryReader(File.Open(inputFile, FileMode.Open)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                loadedCities.Add(SwarsFunctions.ByteToType<SWars.UICItyData>(reader));
            }
        }
        Mesh m = new Mesh();

        List<Vector3> meshCoords = new List<Vector3>();
        List<int> meshIndices = new List<int>();

        int index = 0;
        foreach (SWars.UICItyData c in loadedCities)
        {
            meshCoords.Add(new Vector3(c.x , -c.y, 0));
            meshIndices.Add(index);
            ++index;
        }
        m.SetVertices(meshCoords);
        m.SetIndices(meshIndices, MeshTopology.Points, 0);
        m.name = inputFile;
        CreateMeshObject(m);

        return m;
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
        //r.materials = matSet;
    }
}
