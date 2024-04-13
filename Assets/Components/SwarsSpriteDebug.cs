using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarsSpriteDebug : MonoBehaviour
{
    public List<Texture2D> spriteSet = new List<Texture2D>();
    public List<SWars.ELEFileEntry> elements = new List<SWars.ELEFileEntry>();

    public SWars.STAFileEntry entry;
    public SWars.FRAFileEntry frame;
}
