using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextureLoader
{    
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
    }
}
