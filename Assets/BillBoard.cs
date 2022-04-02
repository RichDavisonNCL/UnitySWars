using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Transform t = Camera.main.transform;

        Vector3 targetPos = new Vector3(t.position.x, transform.position.y, t.position.z);

        transform.LookAt(targetPos, Vector3.up);
    }
}
