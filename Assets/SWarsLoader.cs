using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsLoader : MonoBehaviour
{
    [SerializeField]
    List<Texture2D> mapTextures;

    [SerializeField]
    List<Texture2D> spriteTextures;

    [SerializeField]
    List<Texture2D> uiTextures;

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

    static public Dictionary<int, Texture2D> tempSpriteLookup; //I got these through inspection, must have a proper definition somewhere!

    void Start()
    { 
        LoadTextures();       
        LoadSprites();
        CreateSpriteLookup();
        LoadVehicles();
        LoadMaps(); 

    }

    void CreateSpriteLookup()
    {
        tempSpriteLookup = new Dictionary<int, Texture2D>();
        tempSpriteLookup.Add(990, spriteTextures[0]);  //"UnknownEntity"
        tempSpriteLookup.Add(992, spriteTextures[0]);  //"UnknownEntity"
        tempSpriteLookup.Add(993, spriteTextures[0]);  //"UnknownEntity"
        tempSpriteLookup.Add(999, spriteTextures[0]);  //"StreetlightC" This is a compound sprite
        tempSpriteLookup.Add(1000, spriteTextures[675]); //"Dustbin"
        tempSpriteLookup.Add(1001, spriteTextures[676]); //"TreeB"
        tempSpriteLookup.Add(1004, spriteTextures[686]); //"StreetlightF"	
        tempSpriteLookup.Add(1005, spriteTextures[687]); //"FlowerA"
        tempSpriteLookup.Add(1006, spriteTextures[688]); //"FlowerB"
        tempSpriteLookup.Add(1007, spriteTextures[693]); //"InvLight"
        tempSpriteLookup.Add(1008, spriteTextures[695]); //"StreetlightE"
        tempSpriteLookup.Add(1009, spriteTextures[0]); //"TheBurningMan"
        tempSpriteLookup.Add(1025, spriteTextures[0]); //"UnknownEntity"
        tempSpriteLookup.Add(1026, spriteTextures[707]); //"GrayBarrel"
        tempSpriteLookup.Add(1027, spriteTextures[690]); //"YellowBarrel"
        tempSpriteLookup.Add(1029, spriteTextures[692]); //"LargeMineA"	
        tempSpriteLookup.Add(1032, spriteTextures[695]); //"StreetlightE"
        tempSpriteLookup.Add(1034, spriteTextures[709]); //"TreeD"
                                                       //
        tempSpriteLookup.Add(1035, spriteTextures[698]); //"TreeA"
        tempSpriteLookup.Add(1036, spriteTextures[699]); //"LargeMineB"
        tempSpriteLookup.Add(1037, spriteTextures[700]); //"StreetlightB"
        tempSpriteLookup.Add(1038, spriteTextures[671]); // "StreetlightA"
        tempSpriteLookup.Add(1047, spriteTextures[711]); //"TreeF"
        tempSpriteLookup.Add(1049, spriteTextures[704]); //"TreeE"
        tempSpriteLookup.Add(1050, spriteTextures[0]); //"StreetlightC"
        tempSpriteLookup.Add(1052, spriteTextures[713]); //"Cashpoint"
                                                       //
        tempSpriteLookup.Add(1054, spriteTextures[0]); //"UnknownEntity"
        tempSpriteLookup.Add(1062, spriteTextures[704]); //"TreeG"
        tempSpriteLookup.Add(1064, spriteTextures[727]); //"TreeC"
        tempSpriteLookup.Add(1085, spriteTextures[732]); //"BushA"
        tempSpriteLookup.Add(65535, spriteTextures[0]);//"Emitter"
    }

    void LoadSprites()
    {
        TextureLoader.CreateSprites("MSPR-0", "PAL0.DAT", ref spriteTextures);

        TextureLoader.CreateSprites("POP0-1", "PAL0.DAT", ref uiTextures);
        TextureLoader.CreateSprites("POP1-1", "PAL0.DAT", ref uiTextures);
        TextureLoader.CreateSprites("POP2-1", "PAL0.DAT", ref uiTextures);

        TextureLoader.CreateSprites("POINTERS", "PAL0.DAT", ref uiTextures);
        TextureLoader.CreateSprites("MSPR0-0", "MSPR-0.PAL", ref uiTextures);

        TextureLoader.CreateSprites("MOUSE-0", "S-PROJ.PAL", ref uiTextures);
        TextureLoader.CreateSprites("ICONS0-0", "S-PROJ.PAL", ref uiTextures);
        /*
         *	SwarsTexture::TexturesFromSwarsTAB(fonts,"FONT0-0", "fonttest","Fonts-A");
	SwarsTexture::TexturesFromSwarsTAB(fonts,"FONT0-1", "fonttest","Fonts-B");
	SwarsTexture::TexturesFromSwarsTAB(fonts,"FONT0-2", "fonttest","Fonts-C");
	SwarsTexture::TexturesFromSwarsTAB(fonts,"FONT0-3", "fonttest","Fonts-D");
	SwarsTexture::TexturesFromSwarsTAB(fonts,"FONT0-4", "fonttest","Fonts-E");
	SwarsTexture::TexturesFromSwarsTAB(fonts,"FONT0-5", "fonttest","Fonts-F"); 
         */
        VisualiseTextureSet(ref spriteTextures, "SpriteTextures");
    }


    void LoadTextures()
    {
        mapTextures.Add(TextureLoader.CreateTexture("TEX00.DAT", "PAL0.DAT", 256, 256));
        mapTextures.Add(TextureLoader.CreateTexture("TEX01.DAT", "PAL0.DAT", 256, 256));
        mapTextures.Add(TextureLoader.CreateTexture("TEX02.DAT", "PAL0.DAT", 256, 256));
        mapTextures.Add(TextureLoader.CreateTexture("TEX03.DAT", "PAL0.DAT", 256, 256));
        mapTextures.Add(TextureLoader.CreateTexture("TEX04.DAT", "PAL0.DAT", 256, 256));

        VisualiseTextureSet(ref mapTextures , "MapTextures");
    }

    void VisualiseTextureSet(ref List<Texture2D> textures, string objectName)
    {
        GameObject textureObj = new GameObject(objectName);
        textureObj.transform.parent = transform;
        textureObj.transform.localScale = Vector3.one;

        float offset = 0.0f;

        for (int i = 0; i < textures.Count; ++i)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Quad);
            o.transform.parent = textureObj.transform;

            o.transform.localPosition = new Vector3(offset, 0, 0);
            o.transform.localScale = new Vector3(textures[i].width, textures[i].height, 0.0f);
            Material m = new Material(textureMaterial);
            m.mainTexture = textures[i];
            o.GetComponent<MeshRenderer>().material = m;
            o.name = textures[i].name;

            offset += textures[i].width;
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
            mapMats[i].mainTexture = mapTextures[i];
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
            mapMats[i].mainTexture = mapTextures[i];
        }

       GameObject map = mapA.LoadMap(name, mapMats);

        return map;
    }
}
