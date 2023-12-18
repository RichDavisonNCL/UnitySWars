using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsVehicleIO : MonoBehaviour
{
    [SerializeField]
    List<Mesh> loadedVehicles;

    [SerializeField]
    SWarsTextureIO textureIO;

    public void LoadVehicles()
    {
        VehicleFile.LoadVehicles(SWars.FilePath.Get() + "GAME/QDATA/PRIMVEH.OBJ", ref loadedVehicles);

        transform.parent = transform;
        transform.localScale = Vector3.one;

        for (int i = 0; i < loadedVehicles.Count; ++i)
        {
            GameObject o = new GameObject();
            o.name = "Vehicle " + i;
            o.transform.parent = transform;

            o.transform.localScale      = new Vector3(1, 1, 1);
            o.transform.localPosition   = new Vector3(i * 512, 0, 0);

            MeshRenderer r = o.AddComponent<MeshRenderer>();
            MeshFilter f = o.AddComponent<MeshFilter>();
            f.mesh = loadedVehicles[i];
            r.materials = textureIO.gameMaterial;
        }
    }
}
