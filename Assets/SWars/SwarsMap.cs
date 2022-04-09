using System.Collections.Generic;
using System.IO;
using SWars;
using static SWars.Functions;
using static SWarsVerify;

namespace SWars
{
    [System.Serializable]
    public class Map
    {
        public MapHeader header;

        public List<SWars.TerrainInfo> terrainData = new List<SWars.TerrainInfo>();

        public List<QuadTextureInfo> quadTexInfo = new List<QuadTextureInfo>();
        public List<TriTextureInfo> triTexInfo = new List<TriTextureInfo>();

        public List<Vertex> vertices = new List<Vertex>();
        public List<Tri> tris = new List<Tri>();
        public List<Quad> quads = new List<Quad>();
        public List<MeshDetails> meshes = new List<MeshDetails>();

        public List<LightInfo> lightInfo = new List<LightInfo>();
        public List<LightDetail> lightDetail = new List<LightDetail>();

        //Loaded as part of the texture information
        public List<short> bytes01 = new List<short>(); //Seems to be all 0
        public List<short> bytes02 = new List<short>(); //Seems to be all 0

        //Loaded as part of the vertex / light etc info
        public List<short> bytes04 = new List<short>();
        public List<short> bytes05 = new List<short>();
        public List<short> bytes06 = new List<short>();

        public List<short> bytes07 = new List<short>();
        public List<short> bytes08 = new List<short>();

        public List<DataBlockI> blockIData = new List<DataBlockI>();
        public List<short> bytes09 = new List<short>();
        public List<short> bytes10 = new List<short>();
        public List<short> bytes11 = new List<short>();
        public List<short> bytes12 = new List<short>();
        //Loaded by the LoadNavigation function
        public List<DataBlockK> blockKData = new List<DataBlockK>();
        public List<short> bytes13 = new List<short>();
        public List<VehicleNavPoint> vehicleNavPoints = new List<VehicleNavPoint>();
        public List<short> bytes14 = new List<short>();
        public List<DataBlockM> blockMData = new List<DataBlockM>();
        public List<short> bytes15 = new List<short>();
        public List<NPCNavPoint> navPoints = new List<NPCNavPoint>();
        public List<short> bytes16 = new List<short>();
        public List<NPCBlockLine> blockLines = new List<NPCBlockLine>();
        public List<short> bytes17 = new List<short>();
        //Loaded by LoadUnknownData function
        public List<DataBlockP> blockPData = new List<DataBlockP>();
        public List<DataBlockQ> blockQData = new List<DataBlockQ>();
        public List<DataBlockR> blockRData = new List<DataBlockR>();
        //Loaded by LoadSpriteData function
        public MapSubHeaderPreamble subHeaderPreamble;
        public List<SubHeaderA> subHeaderA = new List<SubHeaderA>();
        public SubHeaderB subHeaderB;

        public List<SubBlockA> subBlockAData = new List<SubBlockA>();
        public List<SubBlockB> subBlockBData = new List<SubBlockB>();

        public List<EntitySubBlock> entitySubBlockData = new List<EntitySubBlock>();
        public List<DataBlockD> dataBlockD = new List<DataBlockD>();

        public EntityHeader entityHeader;
        public SubHeaderD subHeaderD;

        string file = null;

        public float[,] vHeights;

        public bool LoadMap(string filename)
        {
            if (!File.Exists(filename))
            {
                return false;
            }

            file = filename;
            using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                header = Functions.ByteToType<SWars.MapHeader>(reader);
                LoadTerrainData(reader);
                LoadTextureInfo(reader);
                LoadMapDetails(reader);
                LoadNavigation(reader);
                LoadUnknownData(reader);
                LoadSpriteData(reader);
            }
            return true;
        }

        void LoadTerrainData(BinaryReader reader)
        {
            vHeights = new float[128, 128];
            for (int x = 0; x < 128; ++x)
            {
                for (int y = 0; y < 128; ++y)
                {
                    SWars.TerrainInfo t = Functions.ByteToType<SWars.TerrainInfo>(reader);

                    vHeights[y, x] = t.vertexHeight / 32.0f; //Transposed!

                    terrainData.Add(t);
                }
            }
        }

        void LoadTextureInfo(BinaryReader reader)
        {
            VerifyTag(reader, 0x00);
            ReadData<QuadTextureInfo>(reader, ref quadTexInfo, header.numQuadTex);
            ReadUnknownData(reader, ref bytes01, 14400 / 2);

            VerifyTag(reader, 0x01);
            ReadData<TriTextureInfo>(reader, ref triTexInfo, header.numTriTex);
            ReadUnknownData(reader, ref bytes02, 12800 / 2);
        }

        void LoadMapDetails(BinaryReader reader)
        {
            VerifyTag(reader, 0x02);
            ReadData<Vertex>(reader, ref vertices, header.numVerts);
            ReadUnknownData(reader, ref bytes04, 20000 / 2);

            VerifyTag(reader, 0x03);
            ReadData<Tri>(reader, ref tris, header.numTris);
            ReadUnknownData(reader, ref bytes05, 64000 / 2);

            VerifyTag(reader, 0x04);
            ReadData<MeshDetails>(reader, ref meshes, header.numMeshes);
            ReadUnknownData(reader, ref bytes06, 4320 / 2);

            VerifyTag(reader, 0x05);
            ReadData<LightInfo>(reader, ref lightInfo, header.numLightInfo);
            ReadUnknownData(reader, ref bytes07, 24000 / 2);

            VerifyTag(reader, 0x06);
            ReadData<LightDetail>(reader, ref lightDetail, header.numLights);

            ReadUnknownData(reader, ref bytes08, 320 / 2);

            VerifyTag(reader, 0x07);
            ReadData<DataBlockI>(reader, ref blockIData, header.numBlockI);
            ReadUnknownData(reader, ref bytes09, 56000 / 2);

            VerifyTag(reader, 0x08);
            ReadData<Quad>(reader, ref quads, header.numQuads);
            ReadUnknownData(reader, ref bytes10, 80000 / 2);
        }

        void LoadNavigation(BinaryReader reader)
        {
            VerifyTag(reader, 0x09);
            ReadData<DataBlockK>(reader, ref blockKData, header.numBlockK);
            ReadUnknownData(reader, ref bytes13, 2700 / 2);

            VerifyTag(reader, 0x0A);
            ReadData<VehicleNavPoint>(reader, ref vehicleNavPoints, header.numNavPoints);
            ReadUnknownData(reader, ref bytes14, 1800 / 2);

            VerifyTag(reader, 0x0B);
            ReadData<DataBlockM>(reader, ref blockMData, header.numBlockM);
            ReadUnknownData(reader, ref bytes15, 1800 / 2); //1800 is correct

            VerifyTag(reader, 0x0C);
            ReadData<NPCNavPoint>(reader, ref navPoints, header.numNavPointsNPC);
            ReadUnknownData(reader, ref bytes16, 6000 / 2);

            VerifyTag(reader, 0x0D);
            ReadData<NPCBlockLine>(reader, ref blockLines, header.numBlockPointsNPC);
            ReadUnknownData(reader, ref bytes17, 8400 / 2);
        }

        void LoadUnknownData(BinaryReader reader)
        {
            VerifyTag(reader, 0x0E);
            ReadData<DataBlockP>(reader, ref blockPData, header.numBlockP);

            VerifyTag(reader, 0x0F);
            ReadData<DataBlockQ>(reader, ref blockQData, header.numBlockQ);

            VerifyTag(reader, 0x10);
            ReadData<DataBlockR>(reader, ref blockRData, header.numBlockR);
        }

        void LoadSpriteData(BinaryReader reader)
        {
            VerifyTag(reader, 0x11);
            subHeaderPreamble = Functions.ByteToType<SWars.MapSubHeaderPreamble>(reader);

            ReadData<SubHeaderA>(reader, ref subHeaderA, 19); //Unknown 4 and 6 are always 0?

            subHeaderB = Functions.ByteToType<SWars.SubHeaderB>(reader);

            bool done = false;
            SubBlockA blockA = new SubBlockA();

            while (!done)//This can't be the 'proper' way to be doing this bit...
            {
                blockA = ByteToType<SubBlockA>(reader);
                subBlockAData.Add(blockA);

                if (IsFinalEntry(blockA))
                {
                    break;
                }
            }
            done = false;

            SubBlockB blockB;
            int finalCount = 0;

            List<int> blockEnds = new List<int>();
            while (!done)//This can't be the 'proper' way to be doing this bit...
            {
                blockB = ByteToType<SubBlockB>(reader);

                if (IsFinalEntry(blockB))
                {
                    blockEnds.Add(subBlockBData.Count);
                    finalCount++;
                }
                subBlockBData.Add(blockB);

                if (finalCount == 8)
                {
                    break;
                }
            }

            entityHeader = ByteToType<EntityHeader>(reader);

            ReadData<EntitySubBlock>(reader, ref entitySubBlockData, entityHeader.numSprites);

            subHeaderD = ByteToType<SubHeaderD>(reader);

            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                DataBlockD block2 = ByteToType<DataBlockD>(reader);
                dataBlockD.Add(block2);
            }
        }

        public void WriteMeshDetails(MeshDetails details, int index)
        {
            meshes[index] = details;
        }

        public void WriteBlockDDetails(DataBlockD details, int index)
        {
            dataBlockD[index] = details;
        }

        public void SaveToOriginalFile()
        {
            SaveMap(file);
        }

        public void SaveMap(string filename)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(filename, FileMode.OpenOrCreate)))
            {
                WriteType<MapHeader>(writer, header);

                WriteData<TerrainInfo>(writer, ref terrainData);

                WriteTag(writer, 0x00);
                WriteData<QuadTextureInfo>(writer, ref quadTexInfo);
                WriteData<short>(writer, ref bytes01);

                WriteTag(writer, 0x01);
                WriteData<TriTextureInfo>(writer, ref triTexInfo);
                WriteData<short>(writer, ref bytes02);

                WriteTag(writer, 0x02);
                WriteData<Vertex>(writer, ref vertices);
                WriteData<short>(writer, ref bytes04);

                WriteTag(writer, 0x03);
                WriteData<Tri>(writer, ref tris);
                WriteData<short>(writer, ref bytes05);

                WriteTag(writer, 0x04);
                WriteData<MeshDetails>(writer, ref meshes);
                WriteData<short>(writer, ref bytes06);

                WriteTag(writer, 0x05);
                WriteData<LightInfo>(writer, ref lightInfo);
                WriteData<short>(writer, ref bytes07);

                WriteTag(writer, 0x06);
                WriteData<LightDetail>(writer, ref lightDetail);
                WriteData<short>(writer, ref bytes08);

                WriteTag(writer, 0x07);
                WriteData<DataBlockI>(writer, ref blockIData);
                WriteData<short>(writer, ref bytes09);

                WriteTag(writer, 0x08);
                WriteData<Quad>(writer, ref quads);
                WriteData<short>(writer, ref bytes10);

                WriteTag(writer, 0x09);
                WriteData<DataBlockK>(writer, ref blockKData);
                WriteData<short>(writer, ref bytes13);

                WriteTag(writer, 0x0A);
                WriteData<VehicleNavPoint>(writer, ref vehicleNavPoints);
                WriteData<short>(writer, ref bytes14);

                WriteTag(writer, 0x0B);
                WriteData<DataBlockM>(writer, ref blockMData);
                WriteData<short>(writer, ref bytes15);

                WriteTag(writer, 0x0C);
                WriteData<NPCNavPoint>(writer, ref navPoints);
                WriteData<short>(writer, ref bytes16);

                WriteTag(writer, 0x0D);
                WriteData<NPCBlockLine>(writer, ref blockLines);
                WriteData<short>(writer, ref bytes17);

                WriteTag(writer, 0x0E);
                WriteData<DataBlockP>(writer, ref blockPData);

                WriteTag(writer, 0x0F);
                WriteData<DataBlockQ>(writer, ref blockQData);

                WriteTag(writer, 0x10);

                WriteData<DataBlockR>(writer, ref blockRData);

                WriteTag(writer, 0x11);
                WriteType<MapSubHeaderPreamble>(writer, subHeaderPreamble);

                WriteData<SubHeaderA>(writer, ref subHeaderA);
                WriteType<SubHeaderB>(writer, subHeaderB);

                WriteData<SubBlockA>(writer, ref subBlockAData);
                WriteData<SubBlockB>(writer, ref subBlockBData);

                WriteType<EntityHeader>(writer, entityHeader);
                WriteData<EntitySubBlock>(writer, ref entitySubBlockData);

                WriteType<SubHeaderD>(writer, subHeaderD); //This can be zeroed out, doesn't affect next block?

                WriteData<DataBlockD>(writer, ref dataBlockD);
            }
        }
    }
}