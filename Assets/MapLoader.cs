using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

using SWars;
using static SwarsFunctions;
using static SWarsVerify;

public class MapLoader// : MonoBehaviour
{
    MapHeader header;

    List<SWars.TerrainData> terrainData = new List<SWars.TerrainData>();

    List<QuadTextureInfo> quadTexInfo   = new List<QuadTextureInfo>();
    List<TriTextureInfo>  triTexInfo    = new List<TriTextureInfo>();


    List<Vertex>  vertices    = new List<Vertex>();
    List<Tri>     tris        = new List<Tri>();
    List<Quad>    quads       = new List<Quad>();
    List<MeshDetails>    meshes      = new List<MeshDetails>();

    List<LightInfo>   lightInfo      = new List<LightInfo>();
    List<LightDetail> lightDetail    = new List<LightDetail>();

    //Loaded as part of the texture information
    List<short> bytes01 = new List<short>(); //Seems to be all 0
    List<short> bytes02 = new List<short>(); //Seems to be all 0

    //Loaded as part of the vertex / light etc info
    List<short> bytes04 = new List<short>();
    List<short> bytes05 = new List<short>();
    List<short> bytes06 = new List<short>();

    List<short> bytes07 = new List<short>();
    List<short> bytes08 = new List<short>();

    List<DataBlockI> blockIData = new List<DataBlockI>();
    List<short> bytes09 = new List<short>();
    List<short> bytes10 = new List<short>();
    List<short> bytes11 = new List<short>();
    List<short> bytes12 = new List<short>();
    //Loaded by the LoadNavigation function
    List<DataBlockK> blockKData    = new List<DataBlockK>();
    List<short> bytes13                 = new List<short>();
    List<VehicleNavPoint> vehicleNavPoints = new List<VehicleNavPoint>();
    List<short> bytes14                 = new List<short>();
    List<DataBlockM> blockMData    = new List<DataBlockM>();
    List<short> bytes15                 = new List<short>();
    List<NPCNavPoint> navPoints   = new List<NPCNavPoint>();
    List<short> bytes16                 = new List<short>();
    List<NPCBlockLine> blockLines = new List<NPCBlockLine>();
    List<short> bytes17                 = new List<short>();
    //Loaded by LoadUnknownData function
    List<DataBlockP> blockPData = new List<DataBlockP>();
    List<DataBlockQ> blockQData = new List<DataBlockQ>();
    List<DataBlockR> blockRData = new List<DataBlockR>();
    //Loaded by LoadSpriteData function
    MapSubHeaderPreamble subHeaderPreamble;
    List<SubHeaderA> subHeaderA = new List<SubHeaderA>();
    SubHeaderB subHeaderB ;

    List<SubBlockA> subBlockAData = new List<SubBlockA>();
    List<SubBlockB> subBlockBData = new List<SubBlockB>();

    List<SpriteSubBlock> spriteSubBlockData = new List<SpriteSubBlock>();
    List<DataBlockC> dataBlockC = new List<DataBlockC>();

    string file = null;

    public GameObject LoadMap(string filename, Material[] materialSet)
    {
        if(!File.Exists(filename))
        {
            return null;
        }
        file = filename;
        using (BinaryReader reader = new BinaryReader(File.Open(filename, FileMode.Open)))
        {
            header = SwarsFunctions.ByteToType<SWars.MapHeader>(reader);
            if(!Validate(header))
            {
                Debug.LogError("Header failed to validate!");
            }

            LoadTerrainData(reader);
            LoadTextureInfo(reader);
            LoadMapDetails(reader);
            LoadNavigation(reader);
            LoadUnknownData(reader);
            LoadSpriteData(reader);

            GameObject o = CreateMapMesh(filename, materialSet);
            GameObject buildingRoot = new GameObject();
            buildingRoot.name = "Buildings";
            buildingRoot.transform.parent = o.transform;
            buildingRoot.transform.localScale = Vector3.one;
            //Now to handle any buildings!
            CreateBuildingMeshes(buildingRoot, materialSet);

            GameObject spritesRoot = new GameObject();
            spritesRoot.name = "Sprites";
            spritesRoot.transform.parent = o.transform;
            spritesRoot.transform.localScale = Vector3.one;
            CreateSprites(spritesRoot);

            return o;
        }
    }

    void LoadTerrainData(BinaryReader reader)
    {
        for (int x = 0; x < 128; ++x)
        {
            for (int y = 0; y < 128; ++y)
            {
                terrainData.Add(SwarsFunctions.ByteToType<SWars.TerrainData>(reader));
            }
        }
    }

    void CreateSprites(GameObject gameObject)
    {
        for (int i = 0; i < spriteSubBlockData.Count; ++i)
        {
            SpriteSubBlock block = spriteSubBlockData[i];
            if (!Validate(block))
            {
                Debug.LogError("SpriteSubBlock failed to validate!");
            }

            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
            o.name = SwarsFunctions.SpriteNumToName(block.spritenum) + " " + i;
            o.transform.parent = gameObject.transform;
            o.transform.localScale = Vector3.one * 25.0f;
            float xPos = block.x * 256;
            xPos += block.subX;// * 8;

            float zPos = block.y * 256;
            zPos += block.subY;// * 8;

            o.transform.position = new Vector3(xPos, 0, zPos);
        }
    }
    GameObject CreateMapMesh(string filename, Material[] matSet)
    {
        List<int>[] meshIndices = new List<int>[5];

        for (int i = 0; i < 5; ++i)
        {
            meshIndices[i] = new List<int>();
        }

        List<Vector3> meshVertices = new List<Vector3>();
        List<Vector2> meshTexCoords = new List<Vector2>();

        for (int y = 0; y < 127; ++y)
        {
            for (int x = 0; x < 127; ++x)
            {
                SWars.TerrainData dataA = terrainData[(y * 128) + x];
                SWars.TerrainData dataB = terrainData[(y * 128) + (x + 1)];
                SWars.TerrainData dataC = terrainData[((y + 1) * 128) + (x + 1)];
                SWars.TerrainData dataD = terrainData[((y + 1) * 128) + (x + 0)];

                SWars.QuadTextureInfo quadUV = GetQuadTexture(dataA.quadIndex & 0x3FFF, ref quadTexInfo);

                meshIndices[quadUV.texNum].Add(meshVertices.Count + 0);
                meshIndices[quadUV.texNum].Add(meshVertices.Count + 2);
                meshIndices[quadUV.texNum].Add(meshVertices.Count + 1);
                meshIndices[quadUV.texNum].Add(meshVertices.Count + 3);
                meshIndices[quadUV.texNum].Add(meshVertices.Count + 2);
                meshIndices[quadUV.texNum].Add(meshVertices.Count + 0);

                int flipOrder = dataA.quadIndex & 65535;

                if (flipOrder > 0)
                {
                    meshTexCoords.Add(new Vector2(quadUV.v2x, quadUV.v2y) / 255.0f);
                    meshTexCoords.Add(new Vector2(quadUV.v1x, quadUV.v1y) / 255.0f);
                    meshTexCoords.Add(new Vector2(quadUV.v4x, quadUV.v4y) / 255.0f);
                    meshTexCoords.Add(new Vector2(quadUV.v3x, quadUV.v3y) / 255.0f);

                    meshVertices.Add(new Vector3(x + 1, dataC.vertexHeight / 32.0f, y + 1) * 256);
                    meshVertices.Add(new Vector3(x + 0, dataD.vertexHeight / 32.0f, y + 1) * 256);
                    meshVertices.Add(new Vector3(x + 0, dataA.vertexHeight / 32.0f, y) * 256);
                    meshVertices.Add(new Vector3(x + 1, dataB.vertexHeight / 32.0f, y) * 256);
                }
                else
                {
                    meshTexCoords.Add(new Vector2(quadUV.v1x, quadUV.v1y) / 255.0f);
                    meshTexCoords.Add(new Vector2(quadUV.v2x, quadUV.v2y) / 255.0f);
                    meshTexCoords.Add(new Vector2(quadUV.v3x, quadUV.v3y) / 255.0f);
                    meshTexCoords.Add(new Vector2(quadUV.v4x, quadUV.v4y) / 255.0f);

                    meshVertices.Add(new Vector3(x + 0, dataA.vertexHeight / 32.0f, y) * 256);
                    meshVertices.Add(new Vector3(x + 1, dataB.vertexHeight / 32.0f, y) * 256);
                    meshVertices.Add(new Vector3(x + 1, dataC.vertexHeight / 32.0f, y + 1) * 256);
                    meshVertices.Add(new Vector3(x + 0, dataD.vertexHeight / 32.0f, y + 1) * 256);
                }
            }
        }

        GameObject o = new GameObject();
        o.name = filename;

        o.transform.localScale = Vector3.one;

        Mesh map = new Mesh();
        map.SetVertices(meshVertices);
        map.SetUVs(0, meshTexCoords);
        map.subMeshCount = 5;
        for (int i = 0; i < 5; ++i)
        {
            map.SetIndices(meshIndices[i], MeshTopology.Triangles, i);
        }
        map.name = filename;

        MeshRenderer r = o.AddComponent<MeshRenderer>();
        MeshFilter f = o.AddComponent<MeshFilter>();
        f.mesh = map;
        r.materials = matSet;

        return o;
    }
    void CreateBuildingMeshes(GameObject rootObject, Material[] matSet)
    {
        List<GameObject> buildings = new List<GameObject>();
        //Each building is actually made up of a number of submeshes!
        int buildingCount = 0;
        for(int i = 0; i < header.numMeshes; ++i)
        {
            buildingCount = Mathf.Max(meshes[i].buildingIndex, buildingCount);
        }
        buildingCount++;
        for (int i = 0; i < buildingCount; ++i)
        {
            GameObject o = new GameObject();
            o.name = "Building " + i;
            o.transform.parent      = rootObject.transform;
            o.transform.localScale  = Vector3.one;
            buildings.Add(o);
        }
        for (int i = 0; i < header.numMeshes; ++i)
        {
            List<Vector3> meshVertices  = new List<Vector3>();
            List<Vector2> meshTexCoords = new List<Vector2>();
            List<int>[] meshIndices = new List<int>[5];

            for (int j = 0; j < 5; ++j)
            {
                meshIndices[j] = new List<int>();
            }

            for (int v = 0; v < meshes[i].triIndexNum; ++v)
            {
                SWars.Tri tri               = GetTri(meshes[i].triIndexBegin + v, ref tris);
                SWars.TriTextureInfo triUV  = GetTriTexture(tri.faceIndex, ref triTexInfo); 

                meshTexCoords.Add(new Vector2(triUV.v1x, triUV.v1y) / 255.0f);
                meshTexCoords.Add(new Vector2(triUV.v2x, triUV.v2y) / 255.0f);
                meshTexCoords.Add(new Vector2(triUV.v3x, triUV.v3y) / 255.0f);

                int layer = triUV.texNum;

                if (layer < 0 || layer > 4)
                {
                    Debug.Log("incorrect texnum " + layer);
                    layer = 0;
                }

                meshIndices[layer].Add(meshVertices.Count);
                meshIndices[layer].Add(meshVertices.Count + 2);
                meshIndices[layer].Add(meshVertices.Count + 1);

                meshVertices.Add(new Vector3(vertices[tri.vert0Index].x, vertices[tri.vert0Index].y, vertices[tri.vert0Index].z));
                meshVertices.Add(new Vector3(vertices[tri.vert1Index].x, vertices[tri.vert1Index].y, vertices[tri.vert1Index].z));
                meshVertices.Add(new Vector3(vertices[tri.vert2Index].x, vertices[tri.vert2Index].y, vertices[tri.vert2Index].z));
            }
            for (int v = 0; v < meshes[i].quadIndexNum; ++v)
            {
                SWars.Quad quad                 = GetQuad(meshes[i].quadIndexBegin + v, ref quads);
                SWars.QuadTextureInfo quadUV    = GetQuadTexture(quad.faceIndex, ref quadTexInfo);

                int layer = quadUV.texNum;

                if(layer < 0 || layer > 4)
                {
                   // Debug.Log("incorrect texnum " + layer);
                    layer = 0;
                }

                meshIndices[layer].Add(meshVertices.Count + 0);
                meshIndices[layer].Add(meshVertices.Count + 2);
                meshIndices[layer].Add(meshVertices.Count + 1);

                meshIndices[layer].Add(meshVertices.Count + 2);
                meshIndices[layer].Add(meshVertices.Count + 3);
                meshIndices[layer].Add(meshVertices.Count + 1);

                meshTexCoords.Add(new Vector2(quadUV.v1x, quadUV.v1y) / 255.0f);
                meshTexCoords.Add(new Vector2(quadUV.v2x, quadUV.v2y) / 255.0f);
                meshTexCoords.Add(new Vector2(quadUV.v3x, quadUV.v3y) / 255.0f);
                meshTexCoords.Add(new Vector2(quadUV.v4x, quadUV.v4y) / 255.0f);

                meshVertices.Add(new Vector3(vertices[quad.vert0Index].x, vertices[quad.vert0Index].y, vertices[quad.vert0Index].z));
                meshVertices.Add(new Vector3(vertices[quad.vert1Index].x, vertices[quad.vert1Index].y, vertices[quad.vert1Index].z));
                meshVertices.Add(new Vector3(vertices[quad.vert2Index].x, vertices[quad.vert2Index].y, vertices[quad.vert2Index].z));
                meshVertices.Add(new Vector3(vertices[quad.vert3Index].x, vertices[quad.vert3Index].y, vertices[quad.vert3Index].z));
            }
            Mesh buildingPart = new Mesh();
            buildingPart.SetVertices(meshVertices);
            buildingPart.SetUVs(0, meshTexCoords);
            buildingPart.subMeshCount = 5;
            for (int j = 0; j < 5; ++j)
            {
                buildingPart.SetIndices(meshIndices[j], MeshTopology.Triangles, j);
            }
            buildingPart.name = "Section " + i;

            if(meshes[i].buildingIndex >= buildings.Count)
            {
                Debug.Log("Invalid building index " + meshes[i].buildingIndex);
            }

            GameObject o = new GameObject();
            o.name = "Building part " + buildings[meshes[i].buildingIndex].transform.childCount;
            o.transform.parent = buildings[meshes[i].buildingIndex].transform;
            o.transform.localPosition = new Vector3(meshes[i].xPosition, meshes[i].yPosition, meshes[i].zPosition);
            o.transform.localScale = Vector3.one;
            MeshRenderer r  = o.AddComponent<MeshRenderer>();
            MeshFilter f    = o.AddComponent<MeshFilter>();
            f.mesh          = buildingPart;
            r.materials     = matSet;
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
        subHeaderPreamble = SwarsFunctions.ByteToType<SWars.MapSubHeaderPreamble>(reader);
        if (!Validate(subHeaderPreamble))
        {
            Debug.LogError("MapSubHeaderPreamble failed to validate!");
        }

        ReadData<SubHeaderA>(reader, ref subHeaderA, 19); //Unknown 4 and 6 are always 0?

        subHeaderB = SwarsFunctions.ByteToType<SWars.SubHeaderB>(reader);

        foreach (SubHeaderA a in subHeaderA)
        {
            if(!Validate(a))
            {
                Debug.Log("Invalid Subheader A?");
            }
        }
        if (!Validate(subHeaderB))
        {
            Debug.LogWarning("Invalid Subheader B?");
        }

        bool done = false;
        SubBlockA blockA = new SubBlockA();

        while (!done)//This can't be the 'proper' way to be doing this bit...
        {
            blockA = ByteToType<SubBlockA>(reader);
            subBlockAData.Add(blockA);

            if (!Validate(blockA, subBlockAData.Count))
            {
               // Debug.LogError("SubBlockA " + subBlockAData.Count + " failed to validate for file " + file);
            }
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

        SubHeaderC spriteHeader = ByteToType<SubHeaderC>(reader);
        if (!Validate(spriteHeader))
        {
            Debug.LogError("Sprite Header failed to validate for file " + file);
        }
        ReadData<SpriteSubBlock>(reader, ref spriteSubBlockData, spriteHeader.numSprites);

        SubHeaderD subHeaderD = ByteToType<SubHeaderD>(reader);
        if(!Validate(subHeaderD))
        {
            Debug.LogWarning("Unusual SubHeaderD!");
        }
        //unknown3 is pretty close to the size of the count
        //of the next bit * 4

        while(reader.BaseStream.Position != reader.BaseStream.Length)
        {
            DataBlockC block2 = ByteToType<DataBlockC>(reader);
            if (!Validate(block2))
            {
                Debug.LogError("DataBlockC failed to validate!");
            }
            dataBlockC.Add(block2);
        }
        Debug.Log(subHeaderB.unknown1 + " , " + subBlockAData.Count);
        //Debug.Log("SubHeaderD test: " + subHeaderD.unknown3 + ", vs loaded " + dataBlockC.Count + "(" + (subHeaderD.unknown3 / (float)dataBlockC.Count) + " )");
    }
}
