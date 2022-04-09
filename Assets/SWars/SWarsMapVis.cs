using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    string file = null;

    //bool zeroHeader             = false;
    //bool zeroTerrainInfo        = false;
    //bool zeroQuadTexInfo        = false;
    //bool zeroTriTexInfo         = false;
    //bool zeroVertices           = false;
    //bool zeroTris               = false;
    //bool zeroQuads              = false;
    //bool zeroMeshes             = false;
    //bool zeroLightInfo          = true;
    //bool zeroLightDetail        = true;
    //bool zeroBytes01            = true;
    //bool zeroBytes02            = true;
    //bool zeroBytes04            = true;
    //bool zeroBytes05            = true;
    //bool zeroBytes06            = true;
    //bool zeroBytes07            = true;
    //bool zeroBytes08            = true;
    //bool zeroBytes09            = true;
    //bool zeroBytes10            = true;
    //bool zeroBytes13            = true;
    //bool zeroBytes14            = true;
    //bool zeroBytes15            = true;
    //bool zeroBytes16            = true; 
    //bool zeroBytes17            = true;
    //bool zeroBlockIData         = false;
    //bool zeroBlockKData         = false;
    //bool zeroVehicleNavPoints   = false;
    //bool zeroBlockMData         = false;
    //bool zeroNavPoints          = false;
    //bool zeroBlockLines         = false;
    //bool zeroBlockPData         = false;
    //bool zeroBlockQData         = false;
    //bool zeroBlockRData         = false;
    //bool zeroSubHeaderPreamble  = false;
    //bool zeroSubHeaderA         = false;
    //bool zeroSubHeaderB         = false;
    //bool zeroSubBlockAData      = false;
    //bool zeroSubBlockBData      = false;
    //bool zeroEntityHeader       = false;
    //bool zeroSpriteSubBlockData = false;  
    //bool zeroSubHeaderD         = false;
    //bool zeroSubBlockDData      = false;

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

            for(int i = 0; i < quads.Count; ++i)
            {
                Quad q = quads[i];
                q.unknown1 = 0; //Makes the building go a funny colour?
                //q.unknown2 = 0; //goes black?
                ////q.unknown3 = 0;
                q.unknown4 = 0;
                q.unknown5 = 0;
                q.unknown6 = 0;
                q.unknown7 = 0;
                ////q.unknown8 = 0;

                q.unknownStructure1Index = 0;
                q.unknownStructure4Index = 0;

                quads[i] = q;
            }

            for (int i = 0; i < tris.Count; ++i)
            {
                Tri t = tris[i];

                t.unknown1 = 0;
                t.unknown2 = 0; 
                t.unknown3 = 0;
                t.unknown4 = 0;
                t.unknown5 = 0;
                t.unknown6 = 0;
                //t.unknown7 = 0;
                //t.unknown8 = 0;
                //t.unknown9 = 0;
                t.unknown10 = 0;
                t.unknown11 = 0;
                t.unknown12 = 0;
                tris[i] = t;
            }


                WriteData<TerrainInfo>(writer, ref terrainData, zeroTerrainInfo);

            WriteTag(writer, 0x00);
            WriteData<QuadTextureInfo>(writer, ref quadTexInfo, zeroQuadTexInfo);
            WriteData<short>(writer, ref bytes01, zeroBytes01);

            WriteTag(writer, 0x01);
            WriteData<TriTextureInfo>(writer, ref triTexInfo, zeroTriTexInfo);
            WriteData<short>(writer, ref bytes02, zeroBytes02);

            for (int i = 0; i < vertices.Count; ++i)
            {
                Vertex v = vertices[i];
                //v.unknown1 = 0;
                //v.unknown2 = 0;
                //v.unknown2 = (ushort)(Random.Range(0.0f, 1.0f) * 32768.0f);
                v.x = (short)(i * 129);
                v.y = (short)(i * 129);
                //v.x += (short)(Random.Range(-1.0f, 1.0f) * 512.0f);
                //v.y += (short)(Random.Range(-1.0f, 1.0f) * 128.0f);
                //v.z += (short)(Random.Range(-1.0f, 1.0f) * 512.0f);
                //v.z = 64; //vertices at 0 never draw?
                // v.

                vertices[i] = v;
            }

            WriteTag(writer, 0x02);
            WriteData<Vertex>(writer, ref vertices, zeroVertices);
            WriteData<short>(writer, ref bytes04, zeroBytes04);

            WriteTag(writer, 0x03);
            WriteData<Tri>(writer, ref tris, zeroTris);
            WriteData<short>(writer, ref bytes05, zeroBytes05);

            for (int i = 0; i < meshes.Count; ++i)
            {
                MeshDetails m = meshes[i];
                //m.unknown1 = 0;
                //m.unknown3 = 0;
                //m.unknown5 = 0;
                //m.unknown6 = 0;
                //m.unknown10 = 0;
                //m.unknown11 = 0;
                //m.unknown12 = 0;

                //m.noClip = 0;
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

            //for(int i = 0; i < blockLines.Count; ++i)
            //{
            //    NPCBlockLine b = blockLines[i];

            //    b.xStart    += (short)(Random.Range(-1, 1) * 4096.0f);
            //    b.xEnd      += (short)(Random.Range(-1, 1) * 4096.0f);

            //    b.zStart    += (short)(Random.Range(-1, 1) * 4096.0f);
            //    b.zEnd      += (short)(Random.Range(-1, 1) * 4096.0f);

            //    blockLines[i] = b;
            //}

            WriteTag(writer, 0x0D);
            WriteData<NPCBlockLine>(writer, ref blockLines, zeroBlockLines);
            WriteData<short>(writer, ref bytes17, zeroBytes17);

            WriteTag(writer, 0x0E);
            WriteData<DataBlockP>(writer, ref blockPData, zeroBlockPData);

            WriteTag(writer, 0x0F);
            WriteData<DataBlockQ>(writer, ref blockQData, zeroBlockQData);

            WriteTag(writer, 0x10);
            //for(int i = 0; i < blockRData.Count; ++i)
            //{
            //    DataBlockR r = blockRData[i];

            //    if(r.unknown1 != 0)
            //    {
            //        r.unknown1 += (ushort)(Random.Range(-1, 1) * 4096.0f);
            //    }
            //    if (r.unknown2 != 0)
            //    {
            //        r.unknown2 += (ushort)(Random.Range(-1, 1) * 4096.0f);
            //    }
            //    if (r.unknown3 != 0)
            //    {
            //        r.unknown3 += (ushort)(Random.Range(-1, 1) * 4096.0f);
            //    }
            //    if (r.unknown4 != 0)
            //    {
            //        r.unknown4 += (ushort)(Random.Range(-1, 1) * 4096.0f);
            //    }
            //    if (r.unknown5 != 0)
            //    {
            //        r.unknown5 += (ushort)(Random.Range(-1, 1) * 4096.0f);
            //    }
            //    if (r.unknown6 != 0)
            //    {
            //        r.unknown6 += (ushort)(Random.Range(-1, 1) * 4096.0f);
            //    }
            //    if (r.unknown7 != 0)
            //    {
            //        r.unknown7 += (ushort)(Random.Range(-1, 1) * 4096.0f);
            //    }
            //    if (r.unknown8 != 0)
            //    {
            //        r.unknown8 += (ushort)(Random.Range(-1, 1) * 4096.0f);
            //    }

            //    blockRData[i] = r;
            //}
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
            for (int i = 0; i < dataBlockD.Count; ++i)
            {
                DataBlockD d = dataBlockD[i];
                ////d.data[31] = 2;
                //d.data[33] = 2;
                //d.data[1] = 2;

                //d.data[76] = 2;
                //d.data[77] = 2;

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
                dataBlockD[i] = d;
            }

            WriteData<DataBlockD>(writer, ref dataBlockD, zeroSubBlockDData);
        }
    }
}

 * 
 */


public class SWarsMapVis : MonoBehaviour
{
    [SerializeField]
    public SWars.Map loadedMap;
    
    [SerializeField]
    string mapToLoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMap(string filename, Material[] worldMatSet, Material spriteMat)
    {
        loadedMap = new SWars.Map();
        loadedMap.LoadMap(filename);

        CreateMapMesh(filename, worldMatSet);
        transform.localScale = Vector3.one;

        GameObject buildingRoot = new GameObject();
        buildingRoot.name = "Buildings";
        buildingRoot.transform.parent = transform;
        buildingRoot.transform.localScale = Vector3.one;
        //Now to handle any buildings!
        CreateBuildingMeshes(buildingRoot, worldMatSet);

        GameObject spritesRoot = new GameObject();
        spritesRoot.name = "Sprites";
        spritesRoot.transform.parent = transform;
        spritesRoot.transform.localScale = Vector3.one;
        CreateSprites(spritesRoot, spriteMat);

        GameObject blockLines = new GameObject();
        blockLines.name = "BlockLines";
        blockLines.transform.parent = transform;
        blockLines.transform.localScale = Vector3.one;
        CreateBlockLines(blockLines);

        GameObject vehicleNav = new GameObject();
        vehicleNav.name = "Vehicle Nav";
        vehicleNav.transform.parent = transform;
        vehicleNav.transform.localScale = Vector3.one;
        CreateVehicleNavPoints(vehicleNav);

        GameObject lightDetail = new GameObject();
        lightDetail.name = "Light Details";
        lightDetail.transform.parent = transform;
        lightDetail.transform.localScale = Vector3.one;
        CreateLightDetails(lightDetail);
    }

    void CreateLightDetails(GameObject parent)
    {
        for (int i = 0; i < loadedMap.lightDetail.Count; ++i)
        {
            GameObject light = new GameObject();
            light.name = "LightDetail " + i;
            light.transform.parent = parent.transform;
            light.transform.localScale = Vector3.one;

            SWarsLightDataVis vis = light.AddComponent<SWarsLightDataVis>();
            vis.SetLightDetails(loadedMap.lightDetail[i], loadedMap, i);
        }
    }

    void CreateBlockLines(GameObject parent)
    {
        for (int i = 0; i < loadedMap.blockLines.Count; ++i)
        {
            GameObject blockLine = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            blockLine.name = "Line " + i;
            blockLine.transform.parent = parent.transform;
            blockLine.transform.localScale = Vector3.one * 25.0f;

            SWarsBlockLineVis vis = blockLine.AddComponent<SWarsBlockLineVis>();
            vis.SetBlockLineDetails(loadedMap.blockLines[i], loadedMap, i);
        }
    }

    void CreateVehicleNavPoints(GameObject parent)
    {
        SWarsVehicleNavigationSetup setup = parent.AddComponent<SWarsVehicleNavigationSetup>();
        for (int i = 0; i < loadedMap.vehicleNavPoints.Count; ++i)
        {
            GameObject navPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
            navPoint.name = "NavPoint " + i;
            navPoint.transform.parent = parent.transform;
            navPoint.transform.localScale = Vector3.one * 25.0f;

            SWarsVehicleNavigationVis vis = navPoint.AddComponent<SWarsVehicleNavigationVis>();
            vis.SetNavDetails(loadedMap.vehicleNavPoints[i], loadedMap, i);
        }
        setup.BuildConnections();
    }

    void CreateSprites(GameObject gameObject, Material spriteMat)
    {
        for (int i = 0; i < loadedMap.entitySubBlockData.Count; ++i)
        {
            SWars.EntitySubBlock block = loadedMap.entitySubBlockData[i];

            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
            o.name = SWars.Functions.SpriteNumToName(block.spritenum) + " " + i;
            o.transform.parent = gameObject.transform;
            o.transform.localScale = Vector3.one * 25.0f;

            float xPos = block.x1 + (block.x2 << 8);
            float zPos = block.y1 + (block.y2 << 8);

            float yOffset = loadedMap.vHeights[(int)block.x2, (int)block.y2] * 256.0f;

            MeshRenderer r = o.GetComponent<MeshRenderer>();
            r.material = spriteMat;

            Texture2D spriteTex = null;
            SWarsLoader.tempSpriteLookup.TryGetValue(block.spritenum, out spriteTex);

            if (spriteTex)
            {
                r.material.mainTexture = spriteTex;

                o.transform.localScale = new Vector3(spriteTex.width, spriteTex.height, 1) * 8;

                yOffset += spriteTex.height * 8.0f * 0.5f;
            }

            o.transform.position = new Vector3(xPos, yOffset, zPos);

            o.AddComponent<BillBoard>();
        }
    }
    void CreateMapMesh(string filename, Material[] matSet)
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
                SWars.TerrainInfo dataA = loadedMap.terrainData[(y * 128) + x];
                SWars.TerrainInfo dataB = loadedMap.terrainData[(y * 128) + (x + 1)];
                SWars.TerrainInfo dataC = loadedMap.terrainData[((y + 1) * 128) + (x + 1)];
                SWars.TerrainInfo dataD = loadedMap.terrainData[((y + 1) * 128) + (x + 0)];

                SWars.QuadTextureInfo quadUV = SWars.Functions.GetQuadTexture(dataA.quadIndex & 0x3FFF, ref loadedMap.quadTexInfo);

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

        //GameObject o = new GameObject();
        name = filename;

        transform.localScale = Vector3.one;

        Mesh map = new Mesh();
        map.SetVertices(meshVertices);
        map.SetUVs(0, meshTexCoords);
        map.subMeshCount = 5;
        for (int i = 0; i < 5; ++i)
        {
            map.SetIndices(meshIndices[i], MeshTopology.Triangles, i);
        }
        map.name = filename;

        MeshRenderer r = gameObject.AddComponent<MeshRenderer>();
        MeshFilter f = gameObject.AddComponent<MeshFilter>();
        f.mesh = map;
        r.materials = matSet;
    }
    void CreateBuildingMeshes(GameObject rootObject, Material[] matSet)
    {
        List<GameObject> buildings = new List<GameObject>();
        //Each building is actually made up of a number of submeshes!
        int buildingCount = 0;
        for (int i = 0; i < loadedMap.header.numMeshes; ++i)
        {
            buildingCount = Mathf.Max(loadedMap.meshes[i].buildingIndex, buildingCount);
        }
        buildingCount++;
        for (int i = 0; i < buildingCount; ++i)
        {
            GameObject o = new GameObject();
            o.name = "Building " + i;
            o.transform.parent = rootObject.transform;
            o.transform.localScale = Vector3.one;
            buildings.Add(o);

        }
        bool[] vertsUsed = new bool[loadedMap.header.numVerts];

        for (int i = 0; i < loadedMap.header.numMeshes; ++i)
        {
            List<Vector3> meshVertices = new List<Vector3>();
            List<Vector2> meshTexCoords = new List<Vector2>();
            List<int>[] meshIndices = new List<int>[5];

            for (int j = 0; j < 5; ++j)
            {
                meshIndices[j] = new List<int>();
            }

            for (int v = 0; v < loadedMap.meshes[i].triIndexNum; ++v)
            {
                SWars.Tri tri = SWars.Functions.GetTri(loadedMap.meshes[i].triIndexBegin + v, ref loadedMap.tris);
                SWars.TriTextureInfo triUV = SWars.Functions.GetTriTexture(tri.faceIndex, ref loadedMap.triTexInfo);

                meshTexCoords.Add(new Vector2(triUV.v1x, triUV.v1y) / 255.0f);
                meshTexCoords.Add(new Vector2(triUV.v2x, triUV.v2y) / 255.0f);
                meshTexCoords.Add(new Vector2(triUV.v3x, triUV.v3y) / 255.0f);

                int layer = triUV.texNum;

                if (layer < 0 || layer > 4)
                {
                    // Debug.Log("incorrect texnum " + layer);
                    layer = 0;
                }

                vertsUsed[tri.vert0Index] = true;
                vertsUsed[tri.vert1Index] = true;
                vertsUsed[tri.vert2Index] = true;

                meshIndices[layer].Add(meshVertices.Count);
                meshIndices[layer].Add(meshVertices.Count + 2);
                meshIndices[layer].Add(meshVertices.Count + 1);

                meshVertices.Add(new Vector3(loadedMap.vertices[tri.vert0Index].x, loadedMap.vertices[tri.vert0Index].y, loadedMap.vertices[tri.vert0Index].z));
                meshVertices.Add(new Vector3(loadedMap.vertices[tri.vert1Index].x, loadedMap.vertices[tri.vert1Index].y, loadedMap.vertices[tri.vert1Index].z));
                meshVertices.Add(new Vector3(loadedMap.vertices[tri.vert2Index].x, loadedMap.vertices[tri.vert2Index].y, loadedMap.vertices[tri.vert2Index].z));
            }
            for (int v = 0; v < loadedMap.meshes[i].quadIndexNum; ++v)
            {
                SWars.Quad quad = SWars.Functions.GetQuad(loadedMap.meshes[i].quadIndexBegin + v, ref loadedMap.quads);
                SWars.QuadTextureInfo quadUV = SWars.Functions.GetQuadTexture(quad.faceIndex, ref loadedMap.quadTexInfo);

                int layer = quadUV.texNum;

                if (layer < 0 || layer > 4)
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

                vertsUsed[quad.vert0Index] = true;
                vertsUsed[quad.vert1Index] = true;
                vertsUsed[quad.vert2Index] = true;
                vertsUsed[quad.vert2Index] = true;

                meshVertices.Add(new Vector3(loadedMap.vertices[quad.vert0Index].x, loadedMap.vertices[quad.vert0Index].y, loadedMap.vertices[quad.vert0Index].z));
                meshVertices.Add(new Vector3(loadedMap.vertices[quad.vert1Index].x, loadedMap.vertices[quad.vert1Index].y, loadedMap.vertices[quad.vert1Index].z));
                meshVertices.Add(new Vector3(loadedMap.vertices[quad.vert2Index].x, loadedMap.vertices[quad.vert2Index].y, loadedMap.vertices[quad.vert2Index].z));
                meshVertices.Add(new Vector3(loadedMap.vertices[quad.vert3Index].x, loadedMap.vertices[quad.vert3Index].y, loadedMap.vertices[quad.vert3Index].z));
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

            if (loadedMap.meshes[i].buildingIndex >= buildings.Count)
            {
                Debug.Log("Invalid building index " + loadedMap.meshes[i].buildingIndex);
            }

            GameObject o = new GameObject();
            o.name = "Building part " + buildings[loadedMap.meshes[i].buildingIndex].transform.childCount;
            o.transform.parent = buildings[loadedMap.meshes[i].buildingIndex].transform;
            float yPos = loadedMap.meshes[i].yPosition;
            if (yPos > 16384) //TODO
            {
                yPos = 0;
            }
            o.transform.localPosition = new Vector3(loadedMap.meshes[i].xPosition, yPos, loadedMap.meshes[i].zPosition);
            o.transform.localScale = Vector3.one;
            MeshRenderer r = o.AddComponent<MeshRenderer>();
            MeshFilter f = o.AddComponent<MeshFilter>();
            f.mesh = buildingPart;
            r.materials = matSet;

            SWarsBuildingDataVis dataVis = o.AddComponent<SWarsBuildingDataVis>();
            dataVis.SetMeshDetails(loadedMap.meshes[i], loadedMap, i);

            //dataVis.SetBlockDDetails(dataBlockD[i], this, i);

            int b = 0;
            foreach (SWars.DataBlockD d in loadedMap.dataBlockD)
            {
                //seems that 1 isn't the index?
                //3,76,77,31
                if (d.data[1] == loadedMap.meshes[i].buildingIndex)
                {
                    dataVis.SetBlockDDetails(d, loadedMap, b);
                    break;
                }
                b++;
            }
        }
    }

}
