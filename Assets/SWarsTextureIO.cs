using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsTextureIO : MonoBehaviour
{
    [SerializeField]
    public Material baseMaterial;

    [SerializeField]
    public Material[] gameMaterial;

    [SerializeField]
    List<Texture2D> mapTextures;

    public void CreateMaterials()
    {
        mapTextures.Add(TextureLoader.CreateTexture("TEX00.DAT", "PAL0.DAT", 256, 256));
        mapTextures.Add(TextureLoader.CreateTexture("TEX01.DAT", "PAL0.DAT", 256, 256));
        mapTextures.Add(TextureLoader.CreateTexture("TEX02.DAT", "PAL0.DAT", 256, 256));
        mapTextures.Add(TextureLoader.CreateTexture("TEX03.DAT", "PAL0.DAT", 256, 256));
        mapTextures.Add(TextureLoader.CreateTexture("TEX04.DAT", "PAL0.DAT", 256, 256));
        VisualiseTextureSet(ref mapTextures, "MapTextures");

        gameMaterial = new Material[5];
        for (int i = 0; i < 5; ++i)
        {
            gameMaterial[i] = new Material(baseMaterial);
            gameMaterial[i].mainTexture = mapTextures[i];
        }
    }

    void VisualiseTextureSet(ref List<Texture2D> textures, string objectName)
    {
        GameObject textureObj = new GameObject(objectName);
        textureObj.transform.parent = transform;
        textureObj.transform.localScale = Vector3.one;

        float offset = 0.0f;

        for (int i = 0; i < textures.Count; ++i)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Quad);
            o.transform.parent = textureObj.transform;

            o.transform.localPosition = new Vector3(offset, 0, 0);
            o.transform.localScale = new Vector3(textures[i].width, textures[i].height, 0.0f);
            Material m = new Material(baseMaterial);
            m.mainTexture = textures[i];
            o.GetComponent<MeshRenderer>().material = m;
            o.name = textures[i].name;

            offset += textures[i].width;
        }
    }
}
