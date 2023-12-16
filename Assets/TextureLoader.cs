using System.Collections.Generic;
using UnityEngine;

public class TextureLoader
{
    public static void CreateSprites(string filename, string paletteName, ref List<Texture2D> outTextures)
    {
        string datFile = SWars.FilePath.Get() + "GAME/DATA/" + filename + ".DAT";
        string tabFile = SWars.FilePath.Get() + "GAME/DATA/" + filename + ".TAB";
        string palFile = SWars.FilePath.Get() + "GAME/DATA/" + paletteName;

        List<SWars.TABFileEntry> allEntries = new List<SWars.TABFileEntry>();
        SWars.Textures.ReadTABEntries(tabFile, ref allEntries);

        Color[] paletteColours = PaletteFileToColours(palFile);

        int fileCount = 0;

        List<Texture2D> processedTextures = new List<Texture2D>();

        SWars.Textures.ReadDATFile(datFile, ref allEntries,
            (i, width, height, data) =>
            {
                Texture2D tex = CreateTextureFromDatasets(filename + fileCount, width, height, data, paletteColours);
                processedTextures.Add(tex);
                fileCount++;
            }
           );

        outTextures = processedTextures;
    }

    public static Texture2D CreateTexture(string filename, string paletteName, int width, int height)
    {
        string dataFile  = SWars.FilePath.Get() + "GAME/DATA/" + filename;
        string palFile   = SWars.FilePath.Get() + "GAME/DATA/" + paletteName;

        Color[] paletteColours = PaletteFileToColours(palFile);
        Texture2D newTex = null;
        SWars.Textures.ReadDATFile(dataFile, width, height,
            (i, w, h, data) =>
            {
                newTex = CreateTextureFromDatasets(filename, width, height, data, paletteColours);
            }
        );
        return newTex;
    }

    public static Color[] PaletteFileToColours(string filename)
    {        
        Color[] paletteColours = new Color[256];
        int colourIndex = 0;

        SWars.Textures.ReadPaletteFile(filename, 
            (r, g, b, a) => paletteColours[colourIndex++] = new Color(r,g,b,a));

        return paletteColours;
    }

    public static Texture2D CreateTextureFromDatasets(string name, int width, int height, byte[] texData, Color[] paletteData)
    {
        Texture2D newTex = new Texture2D(width, height);

        for (int y = 0; y < height; ++y) 
        {
            for (int x = 0; x < width; ++x)
            {
                int pixIndex = (y * width) + x;
                newTex.SetPixel(x, y, paletteData[texData[pixIndex]]);
            }
        }
        newTex.filterMode = FilterMode.Point;
        newTex.alphaIsTransparency = true;
        newTex.Apply();
        newTex.name = name;
        return newTex;
    }
}
