using System.Collections;
using System.Collections.Generic;
using System.IO;
using SWars;
using UnityEngine;

public class SWarsMapEditor : MonoBehaviour
{
    [SerializeField]
    public SWars.Map loadedMap;

    [SerializeField]
    SWarsSpritesIO spriteIO;

    [SerializeField]
    SWarsTextureIO textureIO;

    [SerializeField]
    bool writeMapStats = false;

    public void LoadMap(string filename, SWarsTextureIO inTextureIO, SWarsSpritesIO inSpriteIO)
    {
        spriteIO    = inSpriteIO;
        textureIO   = inTextureIO;

        loadedMap = new SWars.Map();
        loadedMap.LoadMap(filename);

        if(writeMapStats)
        {
            WriteMapStats(filename);
        }
        
        CreateMapMesh(filename);
        transform.localScale = Vector3.one;

        GameObject buildingRoot = new GameObject();
        buildingRoot.name = "Buildings";
        buildingRoot.transform.parent = transform;
        buildingRoot.transform.localScale = Vector3.one;
        //Now to handle any buildings!
        CreateBuildingMeshes(buildingRoot);

        GameObject spritesRoot = new GameObject();
        spritesRoot.name = "Sprites";
        spritesRoot.transform.parent = transform;
        spritesRoot.transform.localScale = Vector3.one;
        CreateSprites(spritesRoot);

        GameObject blockLines = new GameObject();
        blockLines.name = "BlockLines";
        blockLines.transform.parent = transform;
        blockLines.transform.localScale = Vector3.one;
        CreateBlockLines(blockLines);

        GameObject npcLines = new GameObject();
        npcLines.name = "NPC Nav";
        npcLines.transform.parent = transform;
        npcLines.transform.localScale = Vector3.one;
        CreateNPCNavLoops(npcLines);



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

    void CreateNPCNavLoops(GameObject parent)
    {
        for (int i = 0; i < loadedMap.navPoints.Count; ++i)
        {
            GameObject navLoop = new GameObject();
            navLoop.name = "NavLoop " + i;
            navLoop.transform.parent = parent.transform;
            navLoop.transform.localScale = Vector3.one;

            NPCNavLoopNode node = navLoop.AddComponent<NPCNavLoopNode>();
            node.SetBlockLineDetails(loadedMap, i);
        }
    }

    void CreateBlockLines(GameObject parent)
    {
        for (int i = 0; i < loadedMap.blockLines.Count; ++i)
        {
            CreateBlockLine(loadedMap.blockLines[i], i, parent);
        }
    }

    void CreateBlockLine(SWars.NPCBlockLine at, int index, GameObject parent)
    {
        GameObject blockLine = new GameObject();
        blockLine.name = "Line " + index;
        blockLine.transform.parent = parent.transform;
        blockLine.transform.localScale = Vector3.one;

        GameObject blockLineStart = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        blockLineStart.name = "Start";
        blockLineStart.transform.parent = blockLine.transform;
        blockLineStart.transform.localScale = Vector3.one * 64.0f;
        blockLineStart.transform.localPosition = Vector3.zero;

        GameObject blockLineSEnd = GameObject.CreatePrimitive(PrimitiveType.Cube);
        blockLineSEnd.name = "End";
        blockLineSEnd.transform.parent = blockLine.transform;
        blockLineSEnd.transform.localScale = Vector3.one * 48.0f; //Help to see overlapping lines
        blockLineSEnd.transform.localPosition = Vector3.zero;

        SWarsBlockLineNode vis = blockLine.AddComponent<SWarsBlockLineNode>();
        vis.SetBlockLineDetails(loadedMap, index);
    }

    void CreateVehicleNavPoints(GameObject parent)
    {
        SWarsVehicleNavigationIO setup = parent.AddComponent<SWarsVehicleNavigationIO>();
        for (int i = 0; i < loadedMap.vehicleNavPoints.Count; ++i)
        {
            CreateVehicleNavPoint(i, parent);
        }
        setup.BuildConnections(loadedMap);
    }

    void CreateVehicleNavPoint(int index, GameObject parent)
    {
        GameObject navPoint = GameObject.CreatePrimitive(PrimitiveType.Cube);
        navPoint.name = "NavPoint " + index;
        navPoint.transform.parent = parent.transform;
        navPoint.transform.localScale = Vector3.one * 64.0f;
        navPoint.transform.localPosition = Vector3.zero;

        SWarsVehicleNavigationNode vis = navPoint.AddComponent<SWarsVehicleNavigationNode>();
        vis.SetNavDetails(loadedMap, index);
    }

    void CreateSprites(GameObject gameObject)
    {
        for (int i = 0; i < loadedMap.entitySubBlockData.Count; ++i)
        {
            SWars.EntitySubBlock block = loadedMap.entitySubBlockData[i];

            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
            o.name = SWars.Functions.SpriteNumToName(block.spritenum) + " " + i;
            o.transform.parent = gameObject.transform;
            o.transform.localScale = Vector3.one;

            if(!Validation.Validate(block, i == loadedMap.entitySubBlockData.Count - 1))
            {
                Debug.LogWarning("Entity " + i + " failed validation!");
            }

            float xPos = block.x1 + (block.x2 << 8);
            float zPos = block.y1 + (block.y2 << 8);

            float yOffset = loadedMap.vHeights[(int)block.x2, (int)block.y2] * 256.0f;

            MeshRenderer r = o.GetComponent<MeshRenderer>();
            r.sharedMaterial = textureIO.baseMaterial;

            Texture2D spriteTex = null;
            spriteIO.spriteLookup.TryGetValue(block.spritenum, out spriteTex);

            if (spriteTex)
            {
                r.sharedMaterial.mainTexture = spriteTex;

                o.transform.localScale = new Vector3(spriteTex.width, spriteTex.height, 1) * 8;

                yOffset += spriteTex.height * 8.0f * 0.5f;
            }

            o.transform.localPosition = new Vector3(xPos, yOffset, zPos);

            o.AddComponent<BillBoard>();
        }
    }
    void CreateMapMesh(string filename)
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
        r.materials = textureIO.gameMaterial;
    }
    void CreateBuildingMeshes(GameObject rootObject)
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

                int layer = triUV.texNum;

                if (layer < 0 || layer > 4)
                {
                    layer = 0;
                }

                SWars.Unity.AddTriIndices(ref meshIndices[layer], meshVertices.Count);
                SWars.Unity.AddTriTexCoords(ref meshTexCoords, triUV);
                SWars.Unity.AddTriVertices(ref meshVertices, ref loadedMap.vertices, ref tri);
            }
            for (int v = 0; v < loadedMap.meshes[i].quadIndexNum; ++v)
            {
                SWars.Quad quad = SWars.Functions.GetQuad(loadedMap.meshes[i].quadIndexBegin + v, ref loadedMap.quads);
                SWars.QuadTextureInfo quadUV = SWars.Functions.GetQuadTexture(quad.faceIndex, ref loadedMap.quadTexInfo);

                int layer = quadUV.texNum;

                if (layer < 0 || layer > 4)
                {
                    layer = 0;
                }

                SWars.Unity.AddQuadIndices(ref meshIndices[layer], meshVertices.Count);
                SWars.Unity.AddQuadTexCoords(ref meshTexCoords, quadUV);
                SWars.Unity.AddQuadVertices(ref meshVertices, ref loadedMap.vertices, ref quad);
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
            r.materials = textureIO.gameMaterial;

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

    public void WriteMapStats(string filename)
    {
        TextWriter writer = new StreamWriter("Assets/" + filename + "_stats.txt");
        loadedMap.Serialise<SWars.MapHeader>(writer, loadedMap.header);

        loadedMap.BuildMapStats(writer, loadedMap.bytes01, "Bytes01");
        loadedMap.BuildMapStats(writer, loadedMap.bytes02, "Bytes02");
        loadedMap.BuildMapStats(writer, loadedMap.bytes04, "Bytes04");
        loadedMap.BuildMapStats(writer, loadedMap.bytes05, "Bytes05");
        loadedMap.BuildMapStats(writer, loadedMap.bytes06, "Bytes06");
        loadedMap.BuildMapStats(writer, loadedMap.bytes07, "Bytes07");
        loadedMap.BuildMapStats(writer, loadedMap.bytes08, "Bytes08");

        loadedMap.BuildMapStats<DataBlockI>(writer, loadedMap.blockIData);

        loadedMap.BuildMapStats(writer, loadedMap.bytes09, "Bytes09");
        loadedMap.BuildMapStats(writer, loadedMap.bytes10, "Bytes10");
        loadedMap.BuildMapStats(writer, loadedMap.bytes11, "Bytes11");
        loadedMap.BuildMapStats(writer, loadedMap.bytes12, "Bytes12");

        loadedMap.BuildMapStats<DataBlockK>(writer, loadedMap.blockKData);
        loadedMap.BuildMapStats(writer, loadedMap.bytes13, "Bytes13");

        loadedMap.BuildMapStats<VehicleNavPoint>(writer, loadedMap.vehicleNavPoints);
        loadedMap.BuildMapStats(writer, loadedMap.bytes14, "Bytes14");

        loadedMap.BuildMapStats<DataBlockM>(writer, loadedMap.blockMData);
        loadedMap.BuildMapStats(writer, loadedMap.bytes15, "Bytes15");

        loadedMap.BuildMapStats<NPCNavPoint>(writer, loadedMap.navPoints);
        loadedMap.BuildMapStats(writer, loadedMap.bytes16, "Bytes16");

        loadedMap.BuildMapStats<NPCBlockLine>(writer, loadedMap.blockLines);
        loadedMap.BuildMapStats(writer, loadedMap.bytes17, "Bytes17");

        loadedMap.BuildMapStats<DataBlockP>(writer, loadedMap.blockPData);
        loadedMap.BuildMapStats<DataBlockQ>(writer, loadedMap.blockQData);
        loadedMap.BuildMapStats<DataBlockR>(writer, loadedMap.blockRData);

        //loadedMap.BuildMapStats<SubHeaderA>(writer, loadedMap.subHeaderA);

        loadedMap.BuildMapStats<SubBlockA>(writer, loadedMap.subBlockAData);
        loadedMap.BuildMapStats<SubBlockB>(writer, loadedMap.subBlockBData);

        loadedMap.BuildMapStats<EntitySubBlock>(writer, loadedMap.entitySubBlockData);
        //loadedMap.BuildMapStats<DataBlockD>(writer, loadedMap.dataBlockD);

        writer.Close();
    }

    public void SaveMapFile()
    {
        loadedMap.SaveToOriginalFile();
    }

    public void AddNewBlockLine()
    {
        //loadedMap.
    }

    public void AddVehicleNavPoint()
    {

    }
}
