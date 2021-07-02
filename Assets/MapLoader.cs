using System.Collections.Generic;
using System.IO;
using SWars;
using UnityEngine;
using static SwarsFunctions;
using static SWarsVerify;

public class MapLoader
{
    MapHeader header;

    List<SWars.TerrainInfo> terrainData = new List<SWars.TerrainInfo>();

    List<QuadTextureInfo> quadTexInfo   = new List<QuadTextureInfo>();
    List<TriTextureInfo>  triTexInfo    = new List<TriTextureInfo>();

    List<Vertex>        vertices    = new List<Vertex>();
    List<Tri>           tris        = new List<Tri>();
    List<Quad>          quads       = new List<Quad>();
    List<MeshDetails>   meshes      = new List<MeshDetails>();

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
    List<DataBlockK>    blockKData      = new List<DataBlockK>();
    List<short>         bytes13         = new List<short>();
    List<VehicleNavPoint> vehicleNavPoints = new List<VehicleNavPoint>();
    List<short>         bytes14         = new List<short>();
    List<DataBlockM>    blockMData      = new List<DataBlockM>();
    List<short>         bytes15         = new List<short>();
    List<NPCNavPoint>   navPoints       = new List<NPCNavPoint>();
    List<short>         bytes16         = new List<short>();
    List<NPCBlockLine>  blockLines      = new List<NPCBlockLine>();
    List<short>         bytes17         = new List<short>();
    //Loaded by LoadUnknownData function
    List<DataBlockP> blockPData = new List<DataBlockP>();
    List<DataBlockQ> blockQData = new List<DataBlockQ>();
    List<DataBlockR> blockRData = new List<DataBlockR>();
    //Loaded by LoadSpriteData function
    MapSubHeaderPreamble    subHeaderPreamble;
    List<SubHeaderA>        subHeaderA = new List<SubHeaderA>();
    SubHeaderB              subHeaderB ;

    List<SubBlockA>         subBlockAData = new List<SubBlockA>();
    List<SubBlockB>         subBlockBData = new List<SubBlockB>();

    List<EntitySubBlock>    entitySubBlockData = new List<EntitySubBlock>();
    List<DataBlockD>        dataBlockD = new List<DataBlockD>();

    EntityHeader    entityHeader;
    SubHeaderD      subHeaderD;

    string file = null;

    bool zeroHeader             = false;
    bool zeroTerrainInfo        = false;
    bool zeroQuadTexInfo        = false;
    bool zeroTriTexInfo         = false;
    bool zeroVertices           = false;
    bool zeroTris               = false;
    bool zeroQuads              = false;
    bool zeroMeshes             = false;
    bool zeroLightInfo          = false;
    bool zeroLightDetail        = false;
    bool zeroBytes01            = false;
    bool zeroBytes02            = false;
    bool zeroBytes04            = false;
    bool zeroBytes05            = false;
    bool zeroBytes06            = false;
    bool zeroBytes07            = false;
    bool zeroBytes08            = false;
    bool zeroBytes09            = false;
    bool zeroBytes10            = false;
    bool zeroBytes13            = false;
    bool zeroBytes14            = false;
    bool zeroBytes15            = false;
    bool zeroBytes16            = false; 
    bool zeroBytes17            = false;
    bool zeroBlockIData         = false;
    bool zeroBlockKData         = false;
    bool zeroVehicleNavPoints   = false;
    bool zeroBlockMData         = false;
    bool zeroNavPoints          = false;
    bool zeroBlockLines         = false;
    bool zeroBlockPData         = false;
    bool zeroBlockQData         = false;
    bool zeroBlockRData         = false;
    bool zeroSubHeaderPreamble  = false;
    bool zeroSubHeaderA         = false;
    bool zeroSubHeaderB         = false;
    bool zeroSubBlockAData      = false;
    bool zeroSubBlockBData      = false;
    bool zeroEntityHeader       = false;
    bool zeroSpriteSubBlockData = false;  
    bool zeroSubHeaderD         = false;
    bool zeroSubBlockDData      = false;

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
            SWarsMap mapComponent = o.AddComponent<SWarsMap>();
            mapComponent.loader = this;

            GameObject buildingRoot = new GameObject();
            buildingRoot.name = "Buildings";
            buildingRoot.transform.parent       = o.transform;
            buildingRoot.transform.localScale   = Vector3.one;
            //Now to handle any buildings!
            CreateBuildingMeshes(buildingRoot, materialSet);

            GameObject spritesRoot = new GameObject();
            spritesRoot.name = "Sprites";
            spritesRoot.transform.parent        = o.transform;
            spritesRoot.transform.localScale    = Vector3.one;
            CreateSprites(spritesRoot);

            GameObject blockLines = new GameObject();
            blockLines.name = "BlockLines";
            blockLines.transform.parent     = o.transform;
            blockLines.transform.localScale = Vector3.one;
            CreateBlockLines(blockLines);


            GameObject vehicleNav = new GameObject();
            vehicleNav.name = "Vehicle Nav";
            vehicleNav.transform.parent     = o.transform;
            vehicleNav.transform.localScale = Vector3.one;
            CreateVehicleNavPoints(vehicleNav);

            GameObject lightDetail = new GameObject();
            lightDetail.name                    = "Light Details";
            lightDetail.transform.parent        = o.transform;
            lightDetail.transform.localScale    = Vector3.one;
            CreateLightDetails(lightDetail);

            return o;
        }
    }

    void LoadTerrainData(BinaryReader reader)
    {
        for (int x = 0; x < 128; ++x)
        {
            for (int y = 0; y < 128; ++y)
            {
                terrainData.Add(SwarsFunctions.ByteToType<SWars.TerrainInfo>(reader));
            }
        }
    }

    void CreateLightDetails(GameObject parent)
    {
        for (int i = 0; i < lightDetail.Count; ++i)
        {
            GameObject light = new GameObject();
            light.name = "LightDetail " + i;
            light.transform.parent        = parent.transform;
            light.transform.localScale    = Vector3.one;

            SWarsLightDataVis vis = light.AddComponent<SWarsLightDataVis>();
            vis.SetLightDetails(lightDetail[i], this, i);
        }
    }

    void CreateBlockLines(GameObject parent)
    {
        for(int i = 0; i < blockLines.Count; ++i)
        {
            GameObject blockLine = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            blockLine.name = "Line " + i;
            blockLine.transform.parent     = parent.transform;
            blockLine.transform.localScale = Vector3.one * 25.0f;

            SWarsBlockLineVis vis = blockLine.AddComponent<SWarsBlockLineVis>();
            vis.SetBlockLineDetails(blockLines[i], this, i);
        }
    }

    void CreateVehicleNavPoints(GameObject parent)
    {
        SWarsVehicleNavigationSetup setup =  parent.AddComponent<SWarsVehicleNavigationSetup>();
        for (int i = 0; i < vehicleNavPoints.Count; ++i)
        {
            GameObject navPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
            navPoint.name = "NavPoint " + i;
            navPoint.transform.parent = parent.transform;
            navPoint.transform.localScale = Vector3.one * 25.0f;

            SWarsVehicleNavigationVis vis = navPoint.AddComponent<SWarsVehicleNavigationVis>();
            vis.SetNavDetails(vehicleNavPoints[i], this, i);
        }
        setup.BuildConnections();
    }

    void CreateSprites(GameObject gameObject)
    {
        for (int i = 0; i < entitySubBlockData.Count; ++i)
        {
            EntitySubBlock block = entitySubBlockData[i];
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
                SWars.TerrainInfo dataA = terrainData[(y * 128) + x];
                SWars.TerrainInfo dataB = terrainData[(y * 128) + (x + 1)];
                SWars.TerrainInfo dataC = terrainData[((y + 1) * 128) + (x + 1)];
                SWars.TerrainInfo dataD = terrainData[((y + 1) * 128) + (x + 0)];

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
                   // Debug.Log("incorrect texnum " + layer);
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
            float yPos = meshes[i].yPosition;
            if(yPos > 16384) //TODO
            {
                yPos = 0;
            }
            o.transform.localPosition = new Vector3(meshes[i].xPosition, yPos, meshes[i].zPosition);
            o.transform.localScale = Vector3.one;
            MeshRenderer r  = o.AddComponent<MeshRenderer>();
            MeshFilter f    = o.AddComponent<MeshFilter>();
            f.mesh          = buildingPart;
            r.materials     = matSet;

            SWarsBuildingDataVis dataVis = o.AddComponent<SWarsBuildingDataVis>();
            dataVis.SetMeshDetails(meshes[i], this, i);

            //dataVis.SetBlockDDetails(dataBlockD[i], this, i);

            int b = 0;
            foreach (DataBlockD d in dataBlockD)
            {
                //seems that 1 isn't the index?
                //3,76,77,31
                if (d.data[1] == meshes[i].buildingIndex)
                {
                    dataVis.SetBlockDDetails(d, this, b);
                    break;
                }
                b++;
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

        for (int i = 0; i < lightInfo.Count; ++i)
        {
            if (!Validate(lightInfo[i], lightDetail.Count))
            {
                Debug.LogWarning("Unusual Light Info " + i);
            }
        }

        for (int i = 0; i < lightDetail.Count; ++i)
        {
            if(!Validate(lightDetail[i]))
            {
                Debug.LogWarning("Unusual Light Detail " + i);
            }
        }
        ReadUnknownData(reader, ref bytes08, 320 / 2);

        VerifyTag(reader, 0x07);
        ReadData<DataBlockI>(reader, ref blockIData, header.numBlockI);
        ReadUnknownData(reader, ref bytes09, 56000 / 2);

        VerifyTag(reader, 0x08);
        ReadData<Quad>(reader, ref quads, header.numQuads);
        ReadUnknownData(reader, ref bytes10, 80000 / 2);
        
        int max0 = 0;
        int max1 = 0;
        int max2 = 0;

        //List<ushort> allTes3 = new List<ushort>();
        List<ushort> allTest = new List<ushort>();
        HashSet<ushort> allTest2 = new HashSet<ushort>();

        for(int i = 0; i < lightInfo.Count; ++i)
        {

            max0 = Mathf.Max(max0, lightInfo[i].unknown1);
            max1 = Mathf.Max(max1, lightInfo[i].lightDetailID);
            max2 = Mathf.Max(max2, lightInfo[i].unknown3);

            LightInfo info = lightInfo[i];
            info.unknown1 = (ushort)(Random.value * 4096);
            info.unknown1 = 65535;
            info.unknown1 = 4;

            int testX = info.unknown3 / 128;
            int testY = info.unknown3 % 128;

            //GameObject o            = new GameObject();
            //o.transform.position    = new Vector3(testX * 32, 0, testY * 32);

            if (info.unknown3 != 0)
            {
                allTest.Add(info.unknown3);
                if(!allTest2.Add(info.unknown3))
                {
                    Debug.Log("Unknown3 is NOT UNIQUE"); //Seems like it is! LOL NO
                }
            }

            lightInfo[i] = info;
        }
   
        Debug.Log("Light info ranges: " + max0 + " , " + max1 + " , " + max2 + ". Light Detail Count: " + lightDetail.Count);
    }

    void LoadNavigation(BinaryReader reader)
    {
        VerifyTag(reader, 0x09);
        ReadData<DataBlockK>(reader, ref blockKData, header.numBlockK);
        ReadUnknownData(reader, ref bytes13, 2700 / 2);

        VerifyTag(reader, 0x0A);
        ReadData<VehicleNavPoint>(reader, ref vehicleNavPoints, header.numNavPoints);
        ReadUnknownData(reader, ref bytes14, 1800 / 2);
        for(int i = 0; i < vehicleNavPoints.Count; ++i)
        {
            if(!Validate(vehicleNavPoints[i]))
            {
                Debug.LogWarning("Unusual vehicle nav point " + i);
            }
        }

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

        entityHeader = ByteToType<EntityHeader>(reader);
        if (!Validate(entityHeader))
        {
            Debug.LogError("Sprite Header failed to validate for file " + file);
        }
        ReadData<EntitySubBlock>(reader, ref entitySubBlockData, entityHeader.numSprites);

        subHeaderD = ByteToType<SubHeaderD>(reader);
        if(!Validate(subHeaderD))
        {
            Debug.LogWarning("Unusual SubHeaderD!");
        }
        //unknown3 is pretty close to the size of the count
        //of the next bit * 4

        while(reader.BaseStream.Position != reader.BaseStream.Length)
        {
            DataBlockD block2 = ByteToType<DataBlockD>(reader);
            if (!Validate(block2))
            {
                Debug.LogError("DataBlockC failed to validate!");
            }
            dataBlockD.Add(block2);
        }
        Debug.Log(subHeaderB.unknown1 + " , " + subBlockAData.Count);
        //Debug.Log("SubHeaderD test: " + subHeaderD.unknown3 + ", vs loaded " + dataBlockC.Count + "(" + (subHeaderD.unknown3 / (float)dataBlockC.Count) + " )");
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

    void SaveMap(string filename)
    {
        int divider     = filename.LastIndexOf('/');
        string realname = filename.Substring(divider);
        string path     = filename.Substring(0, divider);
        string newName  = path + "/Saved" + realname;

        if(!Directory.Exists(path + "/Saved/"))
        {
            Directory.CreateDirectory(path + "/Saved/");
        }

        using (BinaryWriter writer = new BinaryWriter(File.Open(newName, FileMode.OpenOrCreate)))
        {
            WriteType<MapHeader>(writer, header, zeroHeader);

            WriteData<TerrainInfo>(writer, ref terrainData, zeroTerrainInfo);

            WriteTag(writer, 0x00);
            WriteData<QuadTextureInfo>(writer, ref quadTexInfo, zeroQuadTexInfo);
            WriteData<short>(writer, ref bytes01, zeroBytes01);

            WriteTag(writer, 0x01);
            WriteData<TriTextureInfo>(writer, ref triTexInfo, zeroTriTexInfo);
            WriteData<short>(writer, ref bytes02, zeroBytes02);

            WriteTag(writer, 0x02);
            WriteData<Vertex>(writer, ref vertices, zeroVertices);
            WriteData<short>(writer, ref bytes04, zeroBytes04);

            WriteTag(writer, 0x03);
            WriteData<Tri>(writer, ref tris, zeroTris);
            WriteData<short>(writer, ref bytes05, zeroBytes05);

            for(int i = 0; i < meshes.Count; ++i)
            {
                MeshDetails m = meshes[i];
                m.noClip = 256;
                meshes[i] = m;
            }

            //foreach(MeshDetails m in meshes)
            //{
            //    m.noClip = 256;
            //}

            WriteTag(writer, 0x04);
            WriteData<MeshDetails>(writer, ref meshes, zeroMeshes);
            WriteData<short>(writer, ref bytes06, zeroBytes06);

            WriteTag(writer, 0x05);
            WriteData<LightInfo>(writer, ref lightInfo, zeroLightInfo);
            WriteData<short>(writer, ref bytes07, zeroBytes07);

            WriteTag(writer, 0x06);
            WriteData<LightDetail>(writer, ref lightDetail, zeroLightDetail);
            WriteData<short>(writer, ref bytes08, zeroBytes08);

            WriteTag(writer, 0x07);
            WriteData<DataBlockI>(writer, ref blockIData, zeroBlockIData);
            WriteData<short>(writer, ref bytes09, zeroBytes09);

            WriteTag(writer, 0x08);
            WriteData<Quad>(writer, ref quads, zeroQuads);
            WriteData<short>(writer, ref bytes10, zeroBytes10);

            WriteTag(writer, 0x09);
            WriteData<DataBlockK>(writer, ref blockKData, zeroBlockKData);
            WriteData<short>(writer, ref bytes13, zeroBytes13);

            WriteTag(writer, 0x0A);
            WriteData<VehicleNavPoint>(writer, ref vehicleNavPoints, zeroVehicleNavPoints);
            WriteData<short>(writer, ref bytes14, zeroBytes14);

            WriteTag(writer, 0x0B);
            WriteData<DataBlockM>(writer, ref blockMData, zeroBlockMData);
            WriteData<short>(writer, ref bytes15, zeroBytes15);

            WriteTag(writer, 0x0C);
            WriteData<NPCNavPoint>(writer, ref navPoints, zeroNavPoints);
            WriteData<short>(writer, ref bytes16, zeroBytes16);

            WriteTag(writer, 0x0D);
            WriteData<NPCBlockLine>(writer, ref blockLines, zeroBlockLines);
            WriteData<short>(writer, ref bytes17, zeroBytes17);

            WriteTag(writer, 0x0E);
            WriteData<DataBlockP>(writer, ref blockPData, zeroBlockPData);

            WriteTag(writer, 0x0F);
            WriteData<DataBlockQ>(writer, ref blockQData, zeroBlockQData);

            WriteTag(writer, 0x10);
            WriteData<DataBlockR>(writer, ref blockRData, zeroBlockRData);

            WriteTag(writer, 0x11);
            WriteType<MapSubHeaderPreamble>(writer, subHeaderPreamble, zeroSubHeaderPreamble);

            WriteData<SubHeaderA>(writer, ref subHeaderA, zeroSubHeaderA);
            WriteType<SubHeaderB>(writer, subHeaderB, zeroSubHeaderB);

            WriteData<SubBlockA>(writer, ref subBlockAData, zeroSubBlockAData);
            WriteData<SubBlockB>(writer, ref subBlockBData, zeroSubBlockBData);

            WriteType<EntityHeader>(writer, entityHeader, zeroEntityHeader);
            WriteData<EntitySubBlock>(writer, ref entitySubBlockData, zeroSpriteSubBlockData);

            WriteType<SubHeaderD>(writer, subHeaderD, zeroSubHeaderD); //This can be zeroed out, doesn't affect next block?

            HashSet<int> buildingIDs = new HashSet<int>();

            foreach(SWars.MeshDetails d in meshes)
            {
                buildingIDs.Add(d.buildingIndex);
            }

            Random.InitState(0); //Seed 0,5 makes a building rotate! 999 makes seed 0 building rotate
            //8 has a flying moving car? 
            //9 puts some police on alert state and instachase the player
            //Zeroing out the 'LightInfo' data fixes the line loops and puts them to default?

            //Buildings, and rain, disappear when this is zeroed out!

            List<int>[] testNumbers = new List<int>[84];
            bool[] nonZero = new bool[84];
            for (int i = 0; i < 84; ++i)
            {
                testNumbers[i] = new List<int>();
            }
            //Definitely related to buildings etc - randomly removing removes building parts
            //for (int i = 0; i < dataBlockD.Count; ++i)
            //{
            //    DataBlockD d = dataBlockD[i];

            //    for(int j = 0; j < 84; ++j)
            //    {                   
            //        testNumbers[j].Add(d.data[j]);

            //        if(j == 1 || j == 76 || j == 77)
            //        {
            //            continue; //these relate to IDs, let's leave them alone...
            //        }
            //        //setting 31 to 0 makes them disappear from 3d and map view
            //        //They look like indices into something or other...
            //        //Forcing to 1 value makes 1 building appear in 3D. some others show in minimap?
            //        if(j == 31)
            //        {
            //            continue;
            //        }

            //        if (j == 3|| j == 7)//Setting these to zero removes building from 3D view, not minimap
            //        {
            //            continue;
            //        }
            //        //2332 2331 2304 2365
            //        //Setting these to all the same value just breaks chain
            //        //if (j == 78)
            //        //{
            //        //    //d.data[j] = 2365; //Let's try and force a weird chain!
            //        //}
            //        //Explodey buildings / people happens between 40 and 50
            //        if (j < 40|| j > 80)
            //        {
            //            continue;//d.data[j] = 2365; //Let's try and force a weird chain!
            //        }

            //        if (d.data[j] > 0)
            //        {
            //            nonZero[j] = true;
            //        }
            //        //THINGS I HAVE SEEN SETTING THIS RANDOMLY when j < 40
            //        //All pedestrians instantly die, some explode?!
            //        //Some buildings seem to be projected to infinity
            //        //Game hangs when looking at certain objects
            //        //Vehicles go fullbright?
            //        //Vehcile spawning into different position?!
            //        //Vehicles in the air!

            //        //Things i have seen when j > 40
            //        //I've finally changed the shape of the minimap polys!
            //        //I've MOVED the entire poly to different places, too...
            //        //Thoughts: It seems to change which vertices are connected
            //        //but doesn't MOVE those vertices, except as a whole unit
            //        //So the vertex positions are a different data struct.
            //       // d.data[j] = (ushort)(Random.value * 4096);
            //    }
            //    dataBlockD[i] = d;
            //}

            WriteData<DataBlockD>(writer, ref dataBlockD, zeroSubBlockDData);
        }
    }
}
