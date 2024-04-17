using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SWarsLoader : MonoBehaviour
{
    [SerializeField]
    SWarsTextureIO textureIO;

    [SerializeField]
    VehicleFile vehicleFile;

    [SerializeField]
    SWarsMapIO mapIO;

    [SerializeField]
    SWarsSpritesIO spritesIO;

    [SerializeField]
    SWarsUIMapIO uiMapIO;

    [SerializeField]
    string swarsGameLocation = "Assets/";

    void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        SWars.FilePath.Set(swarsGameLocation);

        textureIO?.CreateMaterials();

        vehicleFile?.LoadVehicles();

        spritesIO?.LoadSpriteFiles();

        uiMapIO?.LoadUIMap();

        mapIO?.LoadMaps();
    }
}
