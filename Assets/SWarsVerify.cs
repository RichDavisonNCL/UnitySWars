using UnityEngine;
public class SWarsVerify 
{
    public static bool Validate(SWars.MapHeader header)
    {
        if(header.always1 != 1 || header.always0 != 0)
        {
            return false;
        }
        return true;
    }

    public static bool Validate(SWars.MapSubHeaderPreamble preamble)
    {
        if(preamble.preamble1 != 1 || 
           preamble.preamble2 != 0 ||
           preamble.preamble3 != 1 ||
           preamble.preamble4 != 0 ||
           preamble.preamble5 != 1 ||
           preamble.preamble6 != 0 ||
           preamble.preamble7 != 1 ||
           preamble.preamble8 != 0)
        {
            return false;
        }
        return true;
    }

    public static bool Validate(SWars.SubHeaderA header)
    {
        if(header.unknown4 != 0 ||
           header.unknown6 != 0)
        {
            return false;
        }
        return true;
    }

    public static bool Validate(SWars.SubHeaderB header)
    {
        if (header.unknown1 != 4504||
            header.unknown2 != 403 ||
            header.unknown9 != 65535 ||
            header.unknown11 != 1 ||
            header.unknown12 != 5 ||
            header.unknown13 != 8 ||
            header.unknown14 != 4 ||
            header.unknown16 != 11 ||
            header.unknown17 != 65535 ||
            header.unknown18 != 1024 ||
            header.unknown19 != 4)
        {
            return false;
        }
        return true;
    }

    //Map 51 has 0 sprites, and unknown4 is something else!
    //Map 12 has 0 sprites, and unknown4 is something else!
    public static bool Validate(SWars.EntityHeader header)
    {
        if (header.numSprites != 0 && header.unknown4 != 0)
        {
            return false;
        }
        return true;
    }

    //There's ALWAYS exactly 8 of these in the data block...
    public static bool IsFinalEntry(SWars.SubBlockB block)
    {
        if(block.unknown1 == 0 && block.unknown2 == 32768)
        {
            return true;
        }
        return false;
    }

    public static bool IsFinalEntry(SWars.SubBlockA block)
    {
        if (block.unknown12 == 0 &&
            block.unknown14 == 0 &&
            block.unknown18 == 0 &&
            block.unknown20 == 0 &&
            block.unknown22 == 0 &&
            block.unknown24 == 0
            )
        {
            return true;
        }
        return false;
    }
    public static bool Validate(SWars.SubBlockA block, int blockNum)
    {
        if(blockNum == 0) //The first entry seems to follow a more strict pattern?
        {
            //This set seems to always pass?
            if(block.unknown1 != 65535)
            {
                return false;
            }
            if (block.unknown8 != 65535 || block.unknown9 != 65535)
            {
                return false;
            }

            if (block.unknown2 != 1024)
            {
                return false;
            }
            if (block.unknown3 != 3)
            {
                return false;
            }
            if (block.unknown4 != 7)
            {
                return false;
            }
            if (block.unknown5 != 0)
            {
                return false;
            }
            if (block.unknown6 != 17)
            {
                return false;
            }
            if (block.unknown7 != 9)
            {
                return false;
            }
            if (block.unknown10 != 0)
            {
                return false;
            }
            if (block.unknown11 != 5)
            {
                return false;
            }
            if (block.unknown12 != 1)
            {
                return false;
            }
            if (block.unknown13 != 0)
            {
                return false;
            }
            if (block.unknown14 != 1)
            {
                return false;
            }
            if (block.unknown15 != 65535)
            {
                return false;
            }
            if (block.unknown16 != 2)
            {
                return false;
            }
            if (block.unknown17 != 65535)
            {
                return false;
            }
            if (block.unknown18 != 0)
            {
                return false;
            }
            return true;
        }
        else
        {
            //Unknown 12 is NEVER 0 except for the last entry?
            if(block.unknown12 != 0)
            {
                return false;
            }
            if (block.unknown14 != 0)
            {
                return false;
            }
            if (block.unknown18 != 0)
            {
                return false;
            }
            if (block.unknown20 != 0)
            {
                return false;
            }
            if (block.unknown22 != 0)
            {
                return false;
            }
            if (block.unknown24 != 0)
            {
                return false;
            }
            if (block.unknown1 != 65535) //~100 times or so per map
            {
                //if (/*block.unknown8 != 65535 || */block.unknown9 != 65535)
                //{
                //    return false; 
                //}
                return true;
            }

            ////Unknown9 is MOSTLY 65535, but not always!
            //if (block.unknown1 != 65535/* || block.unknown9 != 65535*/)
            //{
            //    return false;
            //}
            return true;
        }
        //if (block.unknown4 != 0  ||
        //    block.unknown6 != 0  ||
        //    block.unknown10 != 0 ||
        //    block.unknown12 != 0 || 
        //    block.unknown16 != 0 ||
        //    block.unknown18 != 0 ||
        //    block.unknown22 != 0 ||
        //    block.unknown24 != 0)
        //{
        //    return false;
        //}
        //return true;
    }

    public static bool Validate(SWars.EntitySubBlock block)
    {
        if (block.zero1 != 0 ||
            block.four1 != 4 ||
            block.zero2 != 0 ||
            block.zero3 != 0 ||
            block.zero4 != 0 ||
            block.always64 != 64 ||
            //block.subXZero != 0 ||
            //block.subYZero != 0 ||
            block.zero7 != 0 ||
            block.zero8 != 0 )
        {
            return false;
        }
        return true;
    }        
    ////69 nonzero rarely happens
    //    //17 nonzero rarely happens
    //    //39 nonzero rarely happens
    //    static int[] zeroIndices = {
    //        /*9*//*,10*//*,*//*11*//*,13*//*,15*//*,17*//*,18,*//*20*//*,21*//*,*//*22,*//*23,*//*24,*//*25,*//*26,*//*27*//*,30*//*,32*//*,36*//*,37*//*,38*//*,39*//*,40*//*,41,42*//*,43*//*,44*//*,45*//*,46*//*,*//*47,*//*48*//*,50*//*,51*//*,52*//*,54*//*,55*//*,*//*56*//*,57*//*,*/59/*,60*//*,61*//*,62*//*,63*//*,64*//*,65*//*,66*//*,68*//*,69*//*,70*//*,71*//*,72*//*,73*//*,83*/
    //    };
    public static bool Validate(SWars.DataBlockD block)
    {
        //Map 11 uses EVERY entry at least once! Curiously it is the space map?
        //map 11 first to have 67
        //map 11 first to have 0
        //map 11 first to have 148
        //map 11 first to have 2094
        //map 11 first to have 1842
        //map 11 first to have 2046
        //OK I THINK THESE ARE GOING TO BE DIFFERENT A LOT
        //if(block.data[19] != 8024 && 
        //    block.data[19] != 6000 && 
        //    block.data[19] != 67 && 
        //    block.data[19] != 0 && 
        //    block.data[19] != 148 && 
        //    block.data[19] != 2094 && 
        //    block.data[19] != 1842 &&
        //    block.data[19] != 2045 &&
        //    block.data[19] != 2046// &&
        //    )
        //{
        //    return false;
        //}
        //for(int i = 0; i < zeroIndices.Length; ++i)
        //{
        //    if(block.data[zeroIndices[i]] != 0)
        //    {
        //        Debug.Log("Non zero blockD item found in index " + zeroIndices[i]);
        //        return false;
        //    }
        //}
        return true;
    }


    /*
     * 
     * Map 12 fails this - also has no sprites?
     */
    public static bool Validate(SWars.SubHeaderD header)
    {
        if (header.unknown1 != 2)
        {
            return false;
        }
        if (header.unknown2 != 0)
        {
            return false;
        }
        if (header.unknown4 != 0 ||
            header.unknown6 != 0 ||
            header.unknown8 != 0)
        {
            return false;
        }
        return true;
    }

    public static bool Validate(SWars.LightInfo info, int lightDetailCount)
    {
        //if(info.unknown1 != 4)
        //{
        //    return false;
        //}
        if (info.lightDetailID > lightDetailCount)
        {
            return false;
        }
        return true;
    }

    public static bool Validate(SWars.LightDetail detail)
    {
        if(detail.intensity != detail.unknowna)
        {
            return false;
        }

        if( detail.unknown1 != 0 ||
            detail.unknown2 != 0 ||
            detail.unknown3 != 0 ||
            detail.unknown5 != 0 ||
            detail.unknown6 != 0 ||
            detail.unknown7 != 0 ||
            detail.unknown8 != 0 ||
            detail.unknown9 != 0    )
        {
            return false;
        }
        if(detail.unknown4 != 0 && detail.unknown4 != 1)
        {
            return false;
        }
        return true;
    }

    public static bool Validate(SWars.VehicleNavPoint navPoint)
    {
        if( navPoint.blank1  != 0 ||
            navPoint.blank2 != 0 ||
            navPoint.blank3 != 0 ||
            navPoint.blank4 != 0 ||
            navPoint.blank5 != 0)
        {
            return false;
        }
        return true;
    }

}
