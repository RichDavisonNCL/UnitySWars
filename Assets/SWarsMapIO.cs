using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsMapIO : MonoBehaviour
{
    [SerializeField]
    SWarsTextureIO textureIO;

    [SerializeField]
    SWarsSpritesIO spritesIO;

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

    public void LoadMaps()
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
                loadedMaps.Add(LoadMap("GAME/MAPS/" + mapID + ".MAD"));
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

            loadedMaps.Add(LoadMap("GAME/MAPS/" + mapID + ".MAD"));
        }

        for (int i = 0; i < loadedMaps.Count; ++i)
        {
            if (loadedMaps[i])
            {
                loadedMaps[i].transform.parent = transform;
                loadedMaps[i].transform.localScale = Vector3.one;
                loadedMaps[i].transform.localPosition = Vector3.up * i * 8192;
            }
        }
    }
    GameObject LoadMap(string name)
    {
        GameObject o    = new GameObject();
        o.transform.parent = transform;
        o.transform.localScale = Vector3.one;
        o.transform.localPosition = Vector3.zero;

        SWarsMapEditor vis = o.AddComponent<SWarsMapEditor>();
        vis.LoadMap(name, textureIO, spritesIO);

        return o;
    }
}
