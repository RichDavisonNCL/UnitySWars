using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SWars
{
    [System.Serializable]
    public struct MapHeader
    {
        public ushort always1;
        public ushort always0;
        public ushort numQuadTex;
        public ushort numTriTex;  
        public ushort numVerts;
        public ushort numTris; 
        public ushort numMeshes;
        public ushort numLightInfo;
        public ushort numLights;
        public ushort numBlockI;
        public ushort numQuads; 
        public ushort numBlockK;
        public ushort numNavPoints; 
        public ushort numBlockM;
        public ushort numNavPointsNPC;
        public ushort numBlockPointsNPC;
        public ushort numBlockP;
        public ushort numBlockQ;   
        public ushort numBlockR; 
    };
    [System.Serializable]
    public struct MapSubHeaderPreamble
    {
        public ushort preamble1;           //0100    always 1
        public ushort preamble2;           //0000    always 0
        public ushort preamble3;           //0100    always 1
        public ushort preamble4;           //0000    always 0
        public ushort preamble5;           //0100    always 1	//preamble?
        public ushort preamble6;           //0000    always 0
        public ushort preamble7;           //0100    always 1
        public ushort preamble8;           //0000    always 0
    };
    [System.Serializable]
    //Buildings in minimap disappear without this - header for DataBlockC?
    public struct SubHeaderA
    {
        public ushort unknown1;        //usually 65535
        public ushort unknown2;        //usually 65535
        public ushort start;           //num a
        public ushort unknown4;        //always 0 
        public ushort end;             //num a	
        public ushort unknown6;        //always 0
    };
    [System.Serializable]
    public struct SubHeaderB
    {
        public ushort unknown1;        //Seems to be always 4504?
        public ushort unknown2;        
        public ushort unknown3;       
        public ushort unknown4;        
        public ushort unknown5;        
        public ushort unknown6;

        public ushort unknown7;      
        public ushort unknown8;
        public ushort unknown9;
        public ushort unknown10;
        public ushort unknown11;
        public ushort unknown12;

        public ushort unknown13;
        public ushort unknown14;
        public ushort unknown15;
        public ushort unknown16;
        public ushort unknown17;
        public ushort unknown18;

        public ushort unknown19;
        public ushort unknown20;
        public ushort unknown21;
        public ushort unknown22;
        public ushort unknown23;
        public ushort unknown24;
    };
    [System.Serializable]
    public struct EntityHeader
    {
        public ushort numSprites;      //
        public ushort unknown2;        //usually 0
        public ushort unknown3;        //num a
        public ushort unknown4;        //0
    };
    [System.Serializable]
    //If this is zeroed out, the data afterwards is still read in!
    public struct SubHeaderD
    {
        public ushort unknown1;        //usually 65535
        public ushort unknown2;        //usually 65535
        public ushort unknown3;        //num a
        public ushort unknown4;        //0
        public ushort unknown5;        //num a	
        public ushort unknown6;        //0
        public ushort unknown7;        //num a	
        public ushort unknown8;        //0
    };

    [System.Serializable]
    public struct SubBlockA
    {
        public ushort unknown1;        //usually 65535
        public ushort unknown2;        //usually 65535
        public ushort unknown3;        //num a
        public ushort unknown4;        //0
        public ushort unknown5;        //num a	
        public ushort unknown6;        //0
        public ushort unknown7;        //num
        public ushort unknown8;        //num
        public ushort unknown9;        //num b
        public ushort unknown10;       //0
        public ushort unknown11;       //num b
        public ushort unknown12;       //0
         
        public ushort unknown13;       //usually 65535
        public ushort unknown14;       //usually 65535
        public ushort unknown15;       //num a
        public ushort unknown16;       //0
        public ushort unknown17;       //num a	
        public ushort unknown18;       //0
        public ushort unknown19;       //num
        public ushort unknown20;       //num
        public ushort unknown21;       //num b
        public ushort unknown22;       //0
        public ushort unknown23;       //num b
        public ushort unknown24;       //0
    };
    [System.Serializable]
    public struct SubBlockB
    {
        public ushort unknown1;
        public ushort unknown2;
    }
    [System.Serializable]
    public struct EntitySubBlock
    {
        public ushort increment1;  //increments from 65251
        public ushort unknown2;    //usually 1280, twice 6144(could that be the 'smoke' tag?) setting everything to 6144 crashes:}
        public ushort zero1;   //always 0
        public ushort four1;   //always 4
        public ushort zero2;   //always 0
        public ushort zero3;   //always 0
        public ushort zero4;   //always 0
        public ushort always64;    //always 64
        public ushort increment2;  //increments from 65250
         
        public char subXZero;
        public char subX;
         
        public ushort x;
        public ushort flags;       //mostly 0, some sort of tags
        public ushort unknownTag;  //appears to be either 0 or 1, if set to 1, sprite is up in the air?
         
        public char subYZero;
        public char subY;
        public ushort y;
         
        public ushort unknown16;   //usually a number around 4000, 0 for smoke emitters
        public ushort spritenum;
        public ushort unknown18;   //65535 except for emitters	(then 5 in testmap)	//appears to do nothing interesting
        public ushort emitterType; //0 except for emitters (then 30 in testmap)
        public ushort emitterInitialDistance1; //always 0
        public ushort emitterInitialDistance2; //always 0
        public ushort emitterVal3; //either 0 or unique number
        public ushort emitterAngleVariance;    //0 except for emitters
        public ushort emitterSpeed;    //0 except for emitters //how fast particles are
        public ushort emitterFrequency;    //0 except for emitters lower = faster
        public ushort zero7;   //always 0
        public ushort zero8;   //always 0
        public ushort unknown28;   //either 0 or unique value
        public ushort unknown29;   //either 0 or unique value
        public ushort increment3;  //incrementing value, 0 for last sprite
    };

    /*
     * If this is zeroed out, the game works, but the buildings disappear
     * The pathing still works, though. They also disppear from the minimap!
     * Could this be minimap data that is also used for vertex culling?
     */
    [System.Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct DataBlockD
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 84)]
        public ushort[] data;
        //These are all related to the meshes array, they have a 'building ID' seen in these 
        //1 is sequential from 1
        //76 sequential from 3??
        //77 sequential from 1?
        //78 indicates a 'connected' building piece that should be destroyed with it - chain?


        //19 is always 8024?
        //31 looks like numbers in the same range as 1?
        //33 flags? nearly always 1, but always low. 
        //49 nearly always v high numbers, sometimes not. 32k bit is flag?

        //78 nearly always 2331?
    };

    [System.Serializable]
    public struct TerrainInfo
    {
        public ushort quadIndex;               //quadVector2map index. This value may be bigger than the number of quad_uvfaces, see note (A).
        public ushort lightScale;              //unknown		//something to do with quads that ARENT lit?
        public ushort flags;                   //unknown	//flags controlled whether the sprite lights were 'on' or 'off' (or is it fullbrights?) //256 = fullbright
        public short  vertexHeight;            //vertex height (this value must be multiplied by 8) (signed 16 bit)
        public ushort spriteNum;               //unknown
        public ushort hasBlockLine;            //quads with this > 0 contain part of a SwarsNPCeBlockLine
        public ushort lightLevel;              //base light level (0 = black, 65535 = XTREME SUNBURN)
        public ushort blocksMovement;              //unknown		//SwarsUnknown6
        public ushort underBuilding;               //unknown		//SwarsUnknown6
    };



    [System.Serializable]
    public struct QuadTextureInfo
    {
        public char v1x;
        public char v1y;
        public char v2x;
        public char v2y;
        public char v3x;
        public char v3y;
        public char v4x;
        public char v4y;
        public ushort texNum;
        public ushort blank1;  //these arent always blank, they show up on the moon, the spacestation, and
        public ushort blank2;  //some random, seemingly unconnected buildings, ie on map 041. but dont seem 
        public ushort blank3;  //to do a lot, and arent 'advert' indicators, sadly
        public ushort blank4;
    };
    [System.Serializable]
    public struct TriTextureInfo
    {
        public char v1x;
        public char v1y;
        public char v2x;
        public char v2y;
        public char v3x;
        public char v3y;
        public ushort texNum;
        public ushort blank1;
        public ushort blank2;
        public ushort blank3;
        public ushort blank4;
    };

    [System.Serializable]
    public struct VehicleHeader
    {
        public ushort unknown1;
        public ushort unknown2;
        public ushort numVerts;
        public ushort numTris;
        public ushort numQuads;
        public ushort numMeshes;
        public ushort numQuadUV;
        public ushort numTriUV;
        public ushort unknown3;
    }
    [System.Serializable]
    public struct Vertex
    {
        public ushort unknown1;
        public short x;
        public short y;
        public short z;
        public ushort unknown2;
    };
    [System.Serializable]
    public struct Tri
    {
        public ushort vert0Index;
        public ushort vert1Index;
        public ushort vert2Index;
        public ushort faceIndex;
        public ushort unknown1; //Usually 1280
        public ushort unknown2;
        public ushort unknown3;
        public ushort unknown4;
        public ushort unknown5;
        public ushort unknown6;
        public ushort vert0LightInfo;
        public ushort vert1LightInfo;
        public ushort vert2LightInfo;
        public ushort unknown10;
        public ushort unknown11;
        public ushort unknown12;
    };
    [System.Serializable]
    public struct Quad
    {
        public ushort vert0Index;
        public ushort vert1Index;
        public ushort vert2Index;
        public ushort vert3Index;
        public ushort faceIndex;
        public ushort unknown1;
        public ushort unknown2;
        public ushort buildingIndex;
        public ushort unknown4;
        public ushort unknown5;
        public ushort unknown6;
        public ushort unknown7;
        public ushort vert0LightInfo;
        public ushort vert1LightInfo;
        public ushort vert2LightInfo;
        public ushort vert3LightInfo;
        public ushort unknownStructure1Index;
        public ushort unknownStructure4Index;
        public ushort allFFFF1;
        public ushort allFFFF2;
    };
    [System.Serializable]
    public struct MeshDetails
    {
        public ushort triIndexBegin;
        public ushort triIndexNum;
        public ushort unknown1;
        public ushort quadIndexBegin;
        public ushort quadIndexNum;
        public ushort buildingIndex;
        public ushort unknown3;
        public ushort yPosition;
        public ushort unknown5;
        public ushort unknown6;	    //shows up very rarely, doesnt seem to mean a lot!
        public ushort xPosition;
        public ushort zPosition;
        public ushort firstVertIndex;
        public ushort lastVertIndex;
        public ushort noClip;	    //not entirely sure on this. bit 8 makes buildings float!
        public ushort unknown10;	//seems to be in the same place as unknown6
        public ushort unknown11;
        public ushort unknown12;    //same maps again, slightly different bits
    };
    [System.Serializable]
    public struct LightInfo  //block [G] //no idea exactly what it is
    { 
        public ushort unknown1;
        public ushort lightDetailID;
        public ushort unknown3; //A lot of 0s. Not quite sequential for others, but almost. Unique? 
    };
    [System.Serializable]
    //DEFINATELY LIGHT SOURCES
    public struct LightDetail  //block [H]
    { 
        public ushort intensity;       //Doc says this is illumination value. Perhaps illumination radius?
        public ushort unknowna;
        public ushort unknownb;
        public ushort unknownc;        //only lights with this 0 seem to be rendered in game?
        public ushort x;
        public ushort y;
        public ushort z;
        public ushort unknown1;    //A few maps have some of these, a couple have nearly all lights tagged with this
        public ushort unknown2;    //i can only find one map in the whole game with a light with this set > 0?
        public ushort unknown3;    //same as unknown2.//1 or 0
        public ushort unknown4;    //seems to be a fairly even split between 0 and > 0 on most maps. some have barely any 0. possibly unused? // 1 or 0 //Destructable lights always seem to have this > 0
        public ushort unknown5;    //very few have this > 0
        public ushort unknown6;    //cant find any > 0
        public ushort unknown7;    //cant find any > 0
        public ushort unknown8;    //very few above > 0. same as 5?
        public ushort unknown9;    //most maps have no > 0. a couple of unused maps have lots > 0. some sort of feature removed early on, or maybe for the ps.
    };
    [System.Serializable]
    //really unknown
    //set all these to 0, didnt seem to make any difference?
    public struct DataBlockI
    { 
        public ushort unknown1;
        public ushort unknown2;
        public ushort unknown3;
        public ushort unknown4;
        public ushort unknown5;
        public ushort unknown6;
        public ushort unknown7;
        public ushort unknown8;
    };
    [System.Serializable]
    /*
    These have something to do with animated landscape tiles - 
    with all these set to 0, they stop animating.
    */
    public struct DataBlockK
    { 
        public ushort unknown1;
        public ushort unknown2;
        public ushort unknown3;
        public ushort unknown4;
        public ushort unknown5;
        public ushort unknown6;
        public ushort unknown7;
        public ushort unknown8;
        public ushort unknown9;
        public ushort unknown10;
        public ushort unknown11;
        public ushort unknown12;
        public ushort unknown13;
        public ushort unknown14;
        public ushort unknown15;
        public ushort unknown16;
        public ushort unknown17;
        public ushort unknown18;
        public ushort unknown19;
        public ushort unknown20;
        public ushort unknown21;
        public ushort unknown22;
        public ushort unknown23;
        public ushort unknown24;
        public ushort unknown25;
        public ushort unknown26;
        public ushort unknown27;
    };

    [System.Serializable]
    /*
    Still no idea what these do
    */
    public struct DataBlockM
    {
        public ushort unknown1;
        public ushort unknown2;
        public ushort unknown3;
        public ushort unknown4;
        public ushort unknown5;
        public ushort unknown6;
        public ushort unknown7;
        public ushort unknown8;
        public ushort unknown9;
        public ushort unknown10;
        public ushort unknown11;
        public ushort unknown12;
        public ushort unknown13;
        public ushort unknown14;
        public ushort unknown15;
        public ushort unknown16;
        public ushort unknown17;
        public ushort unknown18;
    };
    [System.Serializable]
    /*
    Not a clue :)
    */
    public struct DataBlockP
    {
        public ushort unknown1;
        public ushort unknown2;
    };
    [System.Serializable]
    /*
    Something to do with bank doors?
    they dont open if i set these to 0
    */
    public struct DataBlockQ
    {
        public ushort unknown1;
    };
    [System.Serializable]
    //this seems to tie in to the map quad underbuilding value
    //otherwise - Doc says QuadTree?
    public struct DataBlockR
    {
        public ushort unknown1;
        public ushort unknown2;
        public ushort unknown3;
        public ushort unknown4;
        public ushort unknown5;
        public ushort unknown6;
        public ushort unknown7;
        public ushort unknown8;
    };
    [System.Serializable]
    public enum VehicleNavPointType : ushort
    {
        Normal      = 0,
        Unknown1    = 1,
        Barrier1    = 1 << 1,
        Barrier2    = 1 << 2,
        Crossing    = 1 << 3,
        Junction1   = 1 << 4,
        Junction2   = 1 << 5,
        Unknown2    = 1 << 6,
        Exit        = 1 << 7,
        Entrance    = 1 << 8,
        Water1      = 1 << 9,
        Water2      = 1 << 10,
        Parking     = 1 << 11,
        EntryDest   = 1 << 12,
        Unknown3    = 1 << 13,
        Unknown4    = 1 << 14,
        Barrier3    = 1 << 15//,
    }
    [System.Serializable]
    public struct VehicleNavPoint
    {   
        public short x;
        public short y;
        public short z;
        public ushort nodeConnection1;  //this is 0 for 'left hand' 'edge' points, and a couple of others, seemingly around 'car parks'
        public ushort nodeConnection2;  //0 except for some junctions? 'Give way' locations?
        public ushort nodeConnection3;  //car parking 'road' points (odd points in unknown6 show up in this aswell). Doesnt appear in all maps. Unused feature?
        public ushort nodeConnection4;  //the odd points in unknown6 also have this > 0?. Shows up VERY infrequently, so unused?
        public ushort junctionNodes;    //This appears to have something to do with junctions / turn points 0,64,
        public ushort unknown6;         //something to do with barrier entry points, but also a few others? ALWAYS a barrier entry point, so ignore others? How does it know which barrier to drop?
        public VehicleNavPointType typeFlags;        //another junction thing, high at 'edge' points, different heights for left/right. Ped crossings are red, as are all 'water' points.
        public ushort unknown8;         //Nav points which vehicles stop at while barriers lower AND close have values > 0. some others randomly as well
        public ushort unknown9;         //only a few have this > 0. Appear random. Some maps have none at all.
        public ushort unknown10;        //edge points, parking spots, and junctions have values > 0. < only on the test map. Some have barely any points. seems random
        public ushort blank1;           //always 0
        public ushort blank2;           //
        public ushort blank3;           //
        public ushort blank4;           //
        public ushort blank5;           //
    };
    [System.Serializable]
    public struct NPCNavPoint
    { 
        public short lineNumber;        //NPCBlockLine number
        public short numPoints;
        public short loopNumber;
    };
    [System.Serializable]
    public struct NPCBlockLine
    { 
        public short xStart;
        public short yStart;
        public short zStart;
        public short xEnd;
        public short yEnd;
        public short zEnd;
        public short unknown1;
    };
}