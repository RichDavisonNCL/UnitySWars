using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsSprite : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    int spriteID;

    int frame = -1;

    SWars.FRAFileEntry frameData;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if(frame < 0)
        //{
        //    if(SWarsLoader.staEntries != null)
        //    {
        //        frame = SWarsLoader.staEntries[spriteID].index;

        //        frameData = SWarsLoader.fraEntries[frame];

        //        SWars.ELEFileEntry element = SWarsLoader.eleEntries[frameData.firstElement];

        //        while(element.next != 0)
        //        {
        //            element = SWarsLoader.eleEntries[element.next];

        //            Texture2D lol;
        //            lol.
        //        }
        //    }
        //}
        
    }
}
