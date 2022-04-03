using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextureLoader
{
    public static void CreateSprites(string filename, string paletteName, ref List<Texture2D> outTextures)
    {

        string datFile      = "Assets/GAME/DATA/" + filename + ".DAT";
        string tabFile      = "Assets/GAME/DATA/" + filename + ".TAB";

        List<SWars.TABFileEntry> allEntries = new List<SWars.TABFileEntry>();

        using (BinaryReader reader = new BinaryReader(File.Open(tabFile, FileMode.Open)))
        {
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                allEntries.Add(SwarsFunctions.ByteToType<SWars.TABFileEntry>(reader));
            }
        }

        Color[] paletteColours = PaletteFileToColours(paletteName);

        int fileCount = 0;

        using (BinaryReader reader = new BinaryReader(File.Open(datFile, FileMode.Open)))
        {
            foreach (SWars.TABFileEntry e in allEntries)
            {
                reader.BaseStream.Seek(e.offset, SeekOrigin.Begin);
                int xPos = 0;
                int yPos = e.height - 1;

                int dataSize = e.width * e.height;

                byte[] data = new byte[dataSize];

                bool valid = true;

                while(valid && yPos >= 0)
                {
                    byte b  = reader.ReadByte();
                    sbyte c = unchecked((sbyte)b);

                    if (c == 0)
                    {
                        xPos = 0;
                        yPos--;
                    }
                    else if(c < 0)
                    {
                        xPos -= c;
                    }
                    else
                    {
                        for(int i = 0; i < c; ++i)
                        {                            
                            int at = (e.width * yPos) + xPos;

                            if(at >= data.Length)
                            {
                                Debug.LogWarning("Sprite lookup data larger than sprite size!?");
                                valid = false;
                                break;
                            }
                            if(at >= reader.BaseStream.Length)
                            {
                                Debug.LogWarning("Sprite lookup data larger than dat file size!?");
                                valid = false;
                                break;
                            }
                            data[at] = reader.ReadByte();
                            xPos++;
                        }
                    }
                }
                Texture2D tex = CreateTextureFromDatasets(filename + fileCount, e.width, e.height, data, paletteColours);
              
                outTextures.Add(tex);
                fileCount++;
            }
        }
    }

    public static Texture2D CreateTexture(string filename, string paletteName, int width, int height)
    {
        filename    = "Assets/GAME/DATA/" + filename;
        //paletteName = "Assets/GAME/DATA/" + paletteName;

        byte[] texData = null;
       // byte[] palette = null;

        using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
        {
            texData = reader.ReadBytes(width * height);
        }
        //using (BinaryReader reader = new BinaryReader(File.Open(paletteName, FileMode.Open)))
        //{
        //    palette = reader.ReadBytes(256 * 3);
        //}

        //Color[] paletteColours = new Color[256];

        //for(int i = 0; i < 256; ++i)
        //{
        //    float r = palette[(i * 3)+0] / 255.0f;
        //    float g = palette[(i * 3)+1] / 255.0f;
        //    float b = palette[(i * 3)+2] / 255.0f;
        //    paletteColours[i] = new Color(r,g,b);
        //}

        Color[] paletteColours = PaletteFileToColours(paletteName);

        return CreateTextureFromDatasets(filename, width, height, texData, paletteColours);
    }

    public static Color[] PaletteFileToColours(string filename)
    {
        string paletteFile = "Assets/GAME/DATA/" + filename;
        byte[] palette = null;
        using (BinaryReader reader = new BinaryReader(File.Open(paletteFile, FileMode.Open)))
        {
            palette = reader.ReadBytes(256 * 3);
        }
        Color[] paletteColours = new Color[256];

        for (int i = 0; i < 256; ++i)
        {
            float r = palette[(i * 3) + 0] / 255.0f;
            float g = palette[(i * 3) + 1] / 255.0f;
            float b = palette[(i * 3) + 2] / 255.0f;
            float a = i == 0 ? 0.0f : 1.0f;
            paletteColours[i] = new Color(r, g, b, a);
        }
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
                //int pixIndex = ((height - 1 - y) * width) + x;

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
