using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SWarsLoader : MonoBehaviour
{
    [SerializeField]
    SWarsTextureIO textureIO;

    [SerializeField]
    SWarsVehicleIO vehicleIO;

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

        vehicleIO?.LoadVehicles();

        spritesIO?.LoadSpriteFiles();

        uiMapIO?.LoadUIMap();

        mapIO?.LoadMaps();
    }
}
