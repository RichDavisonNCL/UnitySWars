using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsLoader : MonoBehaviour
{
    [SerializeField]
    List<Texture2D> loadedTextures;

    [SerializeField]
    Material mapMaterial;

    [SerializeField]
    Material vehicleMaterial;

    [SerializeField]
    Material textureMaterial;

    [SerializeField]
    List<Mesh> loadedVehicles;

    [SerializeField]
    List<GameObject> loadedMaps;

    [SerializeField]
    int mapToLoad = 1;

    [SerializeField]
    bool loadRange = false;

    [SerializeField]
    int firstRange = 1;

    [SerializeField]
    int lastRange = 79;

    void Start()
    {
        LoadTextures();
        LoadVehicles();
        LoadMaps();
    }


    void LoadTextures()
    {
        loadedTextures.Add(TextureLoader.CreateTexture("TEX00.DAT", "PAL0.DAT", 256, 256));
        loadedTextures.Add(TextureLoader.CreateTexture("TEX01.DAT", "PAL0.DAT", 256, 256));
        loadedTextures.Add(TextureLoader.CreateTexture("TEX02.DAT", "PAL0.DAT", 256, 256));
        loadedTextures.Add(TextureLoader.CreateTexture("TEX03.DAT", "PAL0.DAT", 256, 256));
        loadedTextures.Add(TextureLoader.CreateTexture("TEX04.DAT", "PAL0.DAT", 256, 256));

        GameObject textureObj = new GameObject("Textures");
        textureObj.transform.parent = transform;
        textureObj.transform.localScale = Vector3.one;
        for (int i = 0; i < loadedTextures.Count; ++i)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Quad);
            o.transform.parent = textureObj.transform;

            o.transform.localPosition = new Vector3(i * 2, 0, 0);

            Material m = new Material(textureMaterial);
            m.mainTexture = loadedTextures[i];
            //loadedMaterials.Add(m);
            o.GetComponent<MeshRenderer>().material = m;
            o.name = loadedTextures[i].name;
        }
    }

    void LoadVehicles()
    {
        VehicleLoader.LoadVehicles("Assets/GAME/QDATA/PRIMVEH.OBJ", ref loadedVehicles);

        GameObject vehicleObj       = new GameObject("Vehicles");
        vehicleObj.transform.parent = transform;
        vehicleObj.transform.localScale = Vector3.one;
        Material[] mapMats = new Material[5];
        for (int i = 0; i < 5; ++i)
        {
            mapMats[i] = new Material(mapMaterial);
            mapMats[i].mainTexture = loadedTextures[i];
        }

        for (int i = 0; i < loadedVehicles.Count; ++i)
        {
            GameObject o = new GameObject();
            o.name = "Vehicle " + i;
            o.transform.parent          = vehicleObj.transform;
            
            o.transform.localScale      = new Vector3(1, 1, 1);
            o.transform.localPosition   = new Vector3(i * 512, 0, 0);

            MeshRenderer r  = o.AddComponent<MeshRenderer>();
            MeshFilter f    = o.AddComponent<MeshFilter>();
            f.mesh          = loadedVehicles[i];
            r.materials     = mapMats;
        }
    }

    void LoadMaps()
    {
        if(loadRange)
        {
            for(int i = firstRange; i < lastRange; ++i)
            {
                string mapID = "MAP0";
                if (i < 9)
                {
                    mapID += "0";
                }
                mapID += i;
                Debug.Log("Trying to load map " + i);
                loadedMaps.Add(LoadMap("Assets/GAME/MAPS/" + mapID + ".MAD"));
            }
        }
        else
        {
            string mapID = "MAP0";
            if(mapToLoad < 9)
            {
                mapID += "0";
            }
            mapID += mapToLoad;

            loadedMaps.Add(LoadMap("Assets/GAME/MAPS/" + mapID + ".MAD"));
        }

        GameObject mapObj = new GameObject("Maps");
        mapObj.transform.parent = transform;
        mapObj.transform.localScale = Vector3.one;

        for (int i = 0; i < loadedMaps.Count; ++i)
        {
            if (loadedMaps[i])
            {
                loadedMaps[i].transform.parent = mapObj.transform;
                loadedMaps[i].transform.localScale = Vector3.one;
                loadedMaps[i].transform.localPosition = Vector3.up * i * 8192;
            }
        }
    }
    GameObject LoadMap(string name)
    {
        MapLoader mapA = new MapLoader();

        Material[] mapMats = new Material[5];

        for (int i = 0; i < 5; ++i)
        {
            mapMats[i] = new Material(mapMaterial);
            mapMats[i].mainTexture = loadedTextures[i];
        }

       GameObject map = mapA.LoadMap(name, mapMats);

        return map;
    }
}
