using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsMapVis : MonoBehaviour
{
    [SerializeField]
    public SWars.Map loadedMap;
    
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

                SWars.Unity.AddQuadIndices(ref meshIndices[quadUV.texNum], meshVertices.Count);
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
    public void SaveMapFile()
    {
        loadedMap.SaveToOriginalFile();
    }
}
