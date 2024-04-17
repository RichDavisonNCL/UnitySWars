using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SWarsSpritesIO : MonoBehaviour
{
    [SerializeField]
    SWarsTextureIO textureIO;

    [SerializeField]
    List<Texture2D> spriteTextures;

    [SerializeField]
    List<Texture2D> uiTextures;

    [SerializeField]
    List<Texture2D> spriteFrameTextures;

    public Dictionary<int, Texture2D> spriteLookup; //I got these through inspection, must have a proper definition somewhere!

    public List<SWars.STAFileEntry> staEntries = new List<SWars.STAFileEntry>();
    public List<SWars.FRAFileEntry> fraEntries = new List<SWars.FRAFileEntry>();
    public List<SWars.ELEFileEntry> eleEntries = new List<SWars.ELEFileEntry>();

    void CreateSpriteLookup()
    {
        spriteLookup = new Dictionary<int, Texture2D>();
        spriteLookup.Add(990, spriteTextures[0]);  //"UnknownEntity"
        spriteLookup.Add(992, spriteTextures[0]);  //"UnknownEntity"
        spriteLookup.Add(993, spriteTextures[0]);  //"UnknownEntity"
        spriteLookup.Add(999, spriteTextures[0]);  //"StreetlightC" This is a compound sprite
        spriteLookup.Add(1000, spriteTextures[675]); //"Dustbin"
        spriteLookup.Add(1001, spriteTextures[676]); //"TreeB"
        spriteLookup.Add(1004, spriteTextures[686]); //"StreetlightF"	
        spriteLookup.Add(1005, spriteTextures[687]); //"FlowerA"
        spriteLookup.Add(1006, spriteTextures[688]); //"FlowerB"
        spriteLookup.Add(1007, spriteTextures[693]); //"InvLight"
        spriteLookup.Add(1008, spriteTextures[695]); //"StreetlightE"
        spriteLookup.Add(1009, spriteTextures[0]); //"TheBurningMan"
        spriteLookup.Add(1025, spriteTextures[0]); //"UnknownEntity"
        spriteLookup.Add(1026, spriteTextures[707]); //"GrayBarrel"
        spriteLookup.Add(1027, spriteTextures[690]); //"YellowBarrel"
        spriteLookup.Add(1029, spriteTextures[692]); //"LargeMineA"	
        spriteLookup.Add(1032, spriteTextures[695]); //"StreetlightE"
        spriteLookup.Add(1034, spriteTextures[709]); //"TreeD"
                                                         //
        spriteLookup.Add(1035, spriteTextures[698]); //"TreeA"
        spriteLookup.Add(1036, spriteTextures[699]); //"LargeMineB"
        spriteLookup.Add(1037, spriteTextures[700]); //"StreetlightB"
        spriteLookup.Add(1038, spriteTextures[671]); // "StreetlightA"
        spriteLookup.Add(1047, spriteTextures[711]); //"TreeF"
        spriteLookup.Add(1049, spriteTextures[704]); //"TreeE"
        spriteLookup.Add(1050, spriteTextures[0]); //"StreetlightC"
        spriteLookup.Add(1052, spriteTextures[713]); //"Cashpoint"
                                                         //
        spriteLookup.Add(1054, spriteTextures[0]); //"UnknownEntity"
        spriteLookup.Add(1062, spriteTextures[704]); //"TreeG"
        spriteLookup.Add(1064, spriteTextures[727]); //"TreeC"
        spriteLookup.Add(1085, spriteTextures[732]); //"BushA"
        spriteLookup.Add(65535, spriteTextures[0]);//"Emitter"
    }

    void LoadAnimStates()
    {
        string staFile = SWars.FilePath.Get() + "GAME/DATA/MSTA-0.ANI";
        string fraFile = SWars.FilePath.Get() + "GAME/DATA/MFRA-0.ANI";
        string eleFile = SWars.FilePath.Get() + "GAME/DATA/MELE-0.ANI";

        staEntries = new List<SWars.STAFileEntry>();
        fraEntries = new List<SWars.FRAFileEntry>();
        eleEntries = new List<SWars.ELEFileEntry>();

        using (BinaryReader reader = new BinaryReader(File.Open(staFile, FileMode.Open)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                staEntries.Add(SWars.Functions.ByteToType<SWars.STAFileEntry>(reader));
            }
        }

        using (BinaryReader reader = new BinaryReader(File.Open(fraFile, FileMode.Open)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                fraEntries.Add(SWars.Functions.ByteToType<SWars.FRAFileEntry>(reader));
            }
        }

        using (BinaryReader reader = new BinaryReader(File.Open(eleFile, FileMode.Open)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                eleEntries.Add(SWars.Functions.ByteToType<SWars.ELEFileEntry>(reader));
            }
        }
    }
    void LoadSprites()
    {
        TextureLoader.CreateSprites("MSPR-0", "PAL0.DAT", ref spriteTextures);

        //TextureLoader.CreateSprites("MSPR-1", "PAL0.DAT", ref spriteTextures);
        //TextureLoader.CreateSprites("MSPR-2", "PAL0.DAT", ref spriteTextures);
        //TextureLoader.CreateSprites("MSPR-3", "PAL0.DAT", ref spriteTextures);
        //TextureLoader.CreateSprites("MSPR-4", "PAL0.DAT", ref spriteTextures);
        //TextureLoader.CreateSprites("MSPR-5", "PAL0.DAT", ref spriteTextures);

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
        GameObject set = VisualiseTextureSet(ref spriteTextures, "SpriteTextures");
        set.transform.localPosition = new Vector3(16384.0f,0,0);
        set.transform.localScale = Vector3.one * 8.0f;
    }

    GameObject VisualiseTextureSet(ref List<Texture2D> textures, string objectName)
    {
        GameObject textureObj = new GameObject(objectName);
        textureObj.transform.parent = transform;
        textureObj.transform.localScale = Vector3.one;
        textureObj.transform.localPosition = Vector3.zero;

        float offset = 0.0f;

        float y = 0;
        int countX = 0;
        float maxY = 0.0f;

        for (int i = 0; i < textures.Count; ++i)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Quad);
            o.transform.parent = textureObj.transform;

            o.transform.localPosition = new Vector3(offset + (textures[i].width / 2), y, 0);
            o.transform.localScale = new Vector3(textures[i].width, textures[i].height, 0.0f);
            Material m = new Material(textureIO.baseMaterial);
            m.mainTexture = textures[i];
            o.GetComponent<MeshRenderer>().material = m;
            o.name = textures[i].name;

            offset += textures[i].width;

            maxY = Mathf.Max(maxY, textures[i].height);

            ++countX;
            if(countX > 32)
            {
                countX = 0;
                y += maxY;
                maxY = 0.0f;
                offset = 0.0f;
            }
        }

        return textureObj;
    }

    void LoadAnimFrames()
    {
        int f = 0;

        foreach(SWars.STAFileEntry e in staEntries)
        {
            int index = e.index;
            if(index == 0)
            {
                continue;
            }

            SWars.FRAFileEntry frameData = fraEntries[index];

            Texture2D newTex = new Texture2D(frameData.width , frameData.height );
            newTex.alphaIsTransparency = true;
            newTex.name = "Sprite Texture " + f; //21 is the interesting one!
            newTex.filterMode = FilterMode.Point;

            Color[] texColours = newTex.GetPixels();

            for(int i = 0; i < texColours.Length; ++i)
            {
                texColours[i] = Color.clear;
            }
            newTex.SetPixels(texColours);


            SWars.ELEFileEntry element = eleEntries[frameData.firstElement];

            //GameObject debugObject      = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //SwarsSpriteDebug debugger   = debugObject.AddComponent<SwarsSpriteDebug>();

            //debugObject.name = "Sprite " + f;
            //debugger.entry = e;
            //debugger.frame = frameData;

            while(true)
            {
                if(element.sprite % 6 != 0)
                {
                    Debug.LogError("Invalid sprite ID not modulo 6!");
                }
                int spriteID = element.sprite / 6; //???? got the divide from freesynd source. don't know why?

                if (spriteID >= spriteTextures.Count)
                {
                    Debug.LogWarning("Sprite element has invalid sprite id " + spriteID);
                    break;
                }
                Texture2D eleTex = spriteTextures[spriteID];

                BlitTexture(eleTex, newTex, element);

                //debugger.spriteSet.Add(eleTex);
                //debugger.elements.Add(element);

                if(element.next == 0)
                {
                    break;
                }

                element = eleEntries[element.next];
            }
            spriteFrameTextures.Add(newTex);
            f++;
        }

        Debug.Log("Created " + f + " individual sprite frames");

        GameObject set = VisualiseTextureSet(ref spriteFrameTextures, "SpriteFrameTextures");
        set.transform.localPosition = new Vector3(0, 0, 0);
        set.transform.localScale = Vector3.one * 8.0f;
    }

    void BlitTexture(Texture2D src, Texture2D dst, SWars.ELEFileEntry entry)
    {
        float midX = dst.width * 0.5f;
        float midY = dst.height * 0.5f;

        int xStart = (int)midX + (entry.xOffset / 2);
        int yStart = (int)midY + (entry.yOffset / 2);

        yStart += src.height;
        yStart = dst.height - yStart;

        for (int y = 0; y < src.height; ++y)
        {
            int yWrite = yStart + y;

            if (yWrite < 0)
            {
                continue;
            }
            if (yWrite >= dst.height)
            {
                break;
            }

            for (int x = 0; x < src.width; ++x)
            {
                int xWrite = xStart + x;
                int xRead = x;
                if ((entry.xFlipped & 1) != 0)
                {
                    xRead = (src.width - 1) - x;
                }
                if (xWrite < 0)
                {
                    continue;
                }
                if (xWrite >= dst.width)
                {
                    break;
                }
                Color c = src.GetPixel(xRead, y);
                if (c.a < 0.01f)
                {
                    continue;
                }
                dst.SetPixel(xWrite, yWrite, c);
            }
        }
        dst.Apply();
    }

    public void LoadSpriteFiles()
    {
        LoadSprites();
        CreateSpriteLookup();

        LoadAnimStates();
        LoadAnimFrames();
    }
}
