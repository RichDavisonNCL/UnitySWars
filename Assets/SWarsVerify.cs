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
    public static bool Validate(SWars.SubHeaderC header)
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
        return true;
    }

    public static bool Validate(SWars.SpriteSubBlock block)
    {
        if (block.zero1 != 0 ||
            block.four1 != 4 ||
            block.zero2 != 0 ||
            block.zero3 != 0 ||
            block.zero4 != 0 ||
            block.always64 != 64 ||
            block.subXZero != 0 ||
            block.subYZero != 0 ||
            block.zero7 != 0 ||
            block.zero8 != 0 )
        {
            return false;
        }
        return true;
    }

    public static bool Validate(SWars.DataBlockC block)
    {
        //if (block.unknown10 != 0// ||
        //    //block.unknown12 != 4 || //This can be 0!
        //   // block.unknown16 != 0 || //Map1 has a number here!
        //    //block.unknown18 != 0 //Mostly PoT but not always?
        //    )
        //{
        //    return false;
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



}
