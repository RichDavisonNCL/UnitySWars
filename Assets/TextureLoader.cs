using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextureLoader// : MonoBehaviour
{
    //[SerializeField]
    //Material materialPrototype = null;

    //[SerializeField]
    //List<Texture2D> loadedTextures = new List<Texture2D>();

    //[SerializeField]
    //List<Material> loadedMaterials = new List<Material>();
    // Start is called before the first frame update
    //void Start()
    //{
    //    CreateTexture("TEX00.DAT", "PAL0.DAT", 256, 256);
    //    CreateTexture("TEX01.DAT", "PAL0.DAT", 256, 256);
    //    CreateTexture("TEX02.DAT", "PAL0.DAT", 256, 256);
    //    CreateTexture("TEX03.DAT", "PAL0.DAT", 256, 256);
    //    CreateTexture("TEX04.DAT", "PAL0.DAT", 256, 256);

    //    if (loadedTextures.Count > 0)
    //    {
    //        GameObject textureObj = new GameObject("Textures");
    //        textureObj.transform.parent = this.transform;

    //        for (int i = 0; i < loadedTextures.Count; ++i)
    //        {
    //            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Quad);
    //            o.transform.parent = textureObj.transform;

    //            o.transform.localPosition = new Vector3(i * 2, 0, 0);

    //            Material m = new Material(materialPrototype);
    //            m.mainTexture = loadedTextures[i];

    //            loadedMaterials.Add(m);

    //            o.GetComponent<MeshRenderer>().material = m;
    //        }
    //    }
    //}

    
    public static Texture2D CreateTexture(string filename, string paletteName, int width, int height)
    {
        filename    = "Assets/GAME/DATA/" + filename;
        paletteName = "Assets/GAME/DATA/" + paletteName;

        byte[] texData = null;
        byte[] palette = null;

        using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
        {
            texData = reader.ReadBytes(width * height);
        }
        using (BinaryReader reader = new BinaryReader(File.Open(paletteName, FileMode.Open)))
        {
            palette = reader.ReadBytes(256 * 3);
        }

        Color[] paletteColours = new Color[256];

        for(int i = 0; i < 256; ++i)
        {
            float r = palette[(i * 3)+0] / 255.0f;
            float g = palette[(i * 3)+1] / 255.0f;
            float b = palette[(i * 3)+2] / 255.0f;
            paletteColours[i] = new Color(r,g,b);
        }

        Texture2D newTex = new Texture2D(width, height);

        for(int y = 0; y < height; ++y)
        {
            for(int x = 0; x < width; ++x)
            {
                int pixIndex = (y * width) + x;
                newTex.SetPixel(x, y, paletteColours[texData[pixIndex]]);
            }
        }
        newTex.filterMode = FilterMode.Point;
        newTex.Apply();
        newTex.name = filename;
        return newTex;
       // loadedTextures.Add(newTex);
    }
}
