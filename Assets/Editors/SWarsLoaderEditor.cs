using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;
[CustomEditor(typeof(SWarsLoader))]
public class SWarsLoaderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SWarsLoader loader = (SWarsLoader)target;

        if (GUILayout.Button("Initialise"))
        {
            loader.Initialise();
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
