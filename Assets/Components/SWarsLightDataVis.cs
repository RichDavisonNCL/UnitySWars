﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SWars;

public class SWarsLightDataVis : MonoBehaviour
{
    int lightIndex = 0;
    public SWars.Map sourceMap;

    [SerializeField]
    public LightDetail lightDetail;

    public void SetLightDetails(LightDetail details, SWars.Map source, int index)
    {
        lightDetail = details;
        sourceMap   = source;
        lightIndex  = index;

        transform.localPosition = new Vector3(lightDetail.x, lightDetail.y, lightDetail.z);
    }
}
