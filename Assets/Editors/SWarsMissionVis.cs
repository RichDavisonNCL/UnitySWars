using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SWars.Mission))]
public class SWarsMissionVis : Editor
{
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}
