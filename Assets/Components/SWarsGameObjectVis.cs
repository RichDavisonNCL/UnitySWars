using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWarsGameObjectVis : MonoBehaviour
{

    [SerializeField]
    public SWars.BaseObjectData data;

    [SerializeField]
    public MissionLoader sourceMission;

    public int dataIndex = 0;

    Renderer r = null;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Color c = Color.white;

        switch(data.type)
        {
            case SWars.ObjectType.Invalid: c = Color.black; break;
            case SWars.ObjectType.Agent: c = Color.red; break;
            case SWars.ObjectType.Zealot: c = Color.grey; break;
            case SWars.ObjectType.FemaleUnguidedA: c = Color.green; break;
            case SWars.ObjectType.MaleCivilianA: c = Color.white; break;
            case SWars.ObjectType.FemaleCivilianA: c = Color.white; break;
            case SWars.ObjectType.EurocorpGuard: c = Color.magenta; break;
            case SWars.ObjectType.SpiderZealot: c = Color.cyan; break;
            case SWars.ObjectType.Police: c = Color.blue; break;
            case SWars.ObjectType.MaleUnguidedA: c = Color.green; break;
            case SWars.ObjectType.Scientist: c = Color.yellow;break;
            case SWars.ObjectType.AgentWu: c = Color.yellow; break;
            case SWars.ObjectType.SuperZealot: c = Color.grey; break;
            case SWars.ObjectType.FemaleCivilianB: c = Color.white; break;
            case SWars.ObjectType.MaleCivilianB: c = Color.white; break;
            case SWars.ObjectType.FemaleCivilianC: c = Color.white; break;
        }

        r.material.color = c;
    }
}
