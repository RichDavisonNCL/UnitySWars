using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MissionLoader : MonoBehaviour
{
    [SerializeField]
    GameObject visualiseRoot;
    // Start is called before the first frame update
    [SerializeField]
    bool quickSave = false;

    [SerializeField]
    byte forceTypeValue = 0;

    [SerializeField]
    string filename = "C002L003.DAT";
    string missionFile = "";

    [SerializeField]
    public SWars.Mission mission = new SWars.Mission();

    //[SerializeField]
    //ushort fileType;   

    //public List<SWars.BaseObjectData> objectData = new List<SWars.BaseObjectData>();
    //public List<SWars.MissionEvent> eventData = new List<SWars.MissionEvent>();

    //public List<SWars.MissionUnknown1> unknown1Data = new List<SWars.MissionUnknown1>();
    //public List<SWars.MissionUnknown2> unknown2Data = new List<SWars.MissionUnknown2>();
    //public List<SWars.MissionUnknown3> unknown3Data = new List<SWars.MissionUnknown3>();

    //public SWars.MissionPlayerData playerData;
    //public SWars.MissionUnknown1Header unknown1Header;

    //public SWars.MissionUnknown2Header unknown2Header;

    //public SWars.MissionUnknown3 unknown3;

    //public SWars.MissionHeader header;

    void Start()
    {
        missionFile = "Assets/GAME/LEVELS/" + filename;
        LoadMission();
        SaveData();
    }

    private void Update()
    {
        if(quickSave)
        {
            SaveData();
            quickSave = false;
        }
    }

    public void LoadMission()
    {
        int baseSize = 168;
        int optionalSize = 36;
        int additionalSize = 48;

        using (BinaryReader reader = new BinaryReader(File.Open(missionFile, FileMode.Open)))
        {
            mission.header = SWars.Functions.ByteToType<SWars.MissionHeader>(reader);

            ushort num = reader.ReadUInt16();

            for (int i = 0; i < num; ++i)
            {
                SWars.BaseObjectData baseData = SWars.Functions.ByteToType<SWars.BaseObjectData>(reader, baseSize);

                if (mission.header.type == 9 || mission.header.type == 11 || mission.header.type == 12)
                {
                    baseData.additionalData = SWars.Functions.ByteToType<SWars.AdditionalObjectData>(reader, additionalSize);
                }

                if (baseData.optional_type == 33 || baseData.optional_type == 64 || baseData.optional_type == 61)
                {
                    baseData.optionalData = SWars.Functions.ByteToType<SWars.OptionalObjectData>(reader, optionalSize);
                }
                mission.objectData.Add(baseData);
            }
            ushort numEvents = reader.ReadUInt16();

            for (int i = 0; i < numEvents; ++i)
            {
                SWars.MissionEvent data = SWars.Functions.ByteToType<SWars.MissionEvent>(reader);
                mission.eventData.Add(data);
            }

            mission.playerData = SWars.Functions.ByteToType<SWars.MissionPlayerData>(reader);

            mission.unknown1Header = SWars.Functions.ByteToType<SWars.MissionUnknown1Header>(reader);

            for (int i = 0; i < mission.unknown1Header.numEntries; ++i)
            {
                SWars.MissionUnknown1 data = SWars.Functions.ByteToType<SWars.MissionUnknown1>(reader);
                mission.unknown1Data.Add(data);
            }

            mission.unknown2Header = SWars.Functions.ByteToType<SWars.MissionUnknown2Header>(reader);

            for (int i = 0; i < mission.unknown2Header.numEntries; ++i)
            {
                SWars.MissionUnknown2 data = SWars.Functions.ByteToType<SWars.MissionUnknown2>(reader);
                mission.unknown2Data.Add(data);
            }

            //Need to load in the constant size data afterwards...
            int sizeToLoad = GetBasePlayerDataSize(mission.header);
            mission.unknown3 = SWars.Functions.ByteToType<SWars.MissionUnknown3>(reader, sizeToLoad);
            bool a = true;
        }
        VisualiseObjects();
    }

    unsafe int GetBasePlayerDataSize()
    {
        return sizeof(SWars.MissionUnknown3);
    }

    unsafe int GetBasePlayerDataSize(SWars.MissionHeader h)
    {
        int size = sizeof(SWars.MissionUnknown3);
        if (mission.header.type == 9)
        {
            size -= 4404;
        }
        if (mission.header.type == 11 || mission.header.type == 12 || mission.header.type == 15)
        {
            size -= 4;
        }
        return size;
    }


    public void SaveData()
    {
        SetUnknownState();
        SetObjectState();
        SetEventState();
        SetPlayerData();
        //SetAllObjectPositions();
        SetAllObjectTypes(forceTypeValue);
        int divider = missionFile.LastIndexOf('/');
        string realname = missionFile.Substring(divider);
        string path = missionFile.Substring(0, divider);
        string newName = path + "/Saved" + realname;

        if (!Directory.Exists(path + "/Saved/"))
        {
            Directory.CreateDirectory(path + "/Saved/");
        }

        using (BinaryWriter writer = new BinaryWriter(File.Open(newName, FileMode.Create)))
        {
            SWars.Functions.WriteType<SWars.MissionHeader>(writer, mission.header, false);

            SWars.Functions.WriteType<ushort>(writer, (ushort)mission.objectData.Count);

            //Debug.LogWarning("Real size of baseobject data is: " + GetBaseObjectDataSize());
            foreach (SWars.BaseObjectData b in mission.objectData)
            {
                WriteObjectData(b, writer);
            }

            SWars.Functions.WriteType<ushort>(writer, (ushort)mission.eventData.Count);
            foreach (SWars.MissionEvent b in mission.eventData)
            {
                SWars.Functions.WriteType<SWars.MissionEvent>(writer, b);
            }

            SWars.Functions.WriteType<SWars.MissionPlayerData>(writer, mission.playerData);

            SWars.Functions.WriteType<SWars.MissionUnknown1Header>(writer, mission.unknown1Header);
            foreach (SWars.MissionUnknown1 b in mission.unknown1Data)
            {
                SWars.Functions.WriteType<SWars.MissionUnknown1>(writer, b);
            }

            SWars.Functions.WriteType<SWars.MissionUnknown2Header>(writer, mission.unknown2Header);
            foreach (SWars.MissionUnknown2 b in mission.unknown2Data)
            {
                SWars.Functions.WriteType<SWars.MissionUnknown2>(writer, b);
            }

            int sizeToWrite = GetBasePlayerDataSize(mission.header);
            SWars.Functions.WriteType<SWars.MissionUnknown3>(writer, mission.unknown3, sizeToWrite);
        }
    }
    unsafe int GetFullbjectDataSize()
    {
        return sizeof(SWars.BaseObjectData);
    }

    unsafe int GetBaseObjectDataSize()
    {
        return sizeof(SWars.BaseObjectData) - sizeof(SWars.AdditionalObjectData) - sizeof(SWars.OptionalObjectData);
    }

    void SetAllObjectTypes(byte type)
    {
        //for(int i = 0; i < objectData.Count; ++i)
        //{
        //    SWars.BaseObjectData b = objectData[i];
        //    b.type = (SWars.ObjectType)Random.Range(0, 16);
        //    objectData[i] = b;
        //}
    }

    void SetAllObjectPositions()
    {
        for (int i = 0; i < mission.objectData.Count; ++i)
        {
            SWars.BaseObjectData b = mission.objectData[i];
            b.x0 = 0;
            b.x1 = 0;
            b.x2 = 0;
            b.x3 = 0;

            b.y0 = 0;
            b.y1 = 0;
            b.y2 = 0;
            b.y3 = 0;

            b.z0 = 0;
            b.z1 = 0;
            b.z2 = 0;
            b.z3 = 0;

            mission.objectData[i] = b;
        }
    }

    unsafe void SetObjectState()
    {
        //for (int i = 0; i < objectData.Count; ++i)
        //{
        //    SWars.BaseObjectData b = objectData[i];
        //    b.unkn05 = 0;
        //    b.unkn06 = 0; 
        //    b.unkn07 = 0;
        //    b.unkn08 = 0;
        //    b.unkn09 = 0;
        //    b.unkn10 = 0;
        //    b.unkn11 = 0;
        //    b.unkn12 = 0;

        //    Debug.LogWarning("Unknown14: " + b.unkn14);

        //    // b.unkn14 = 0;    //Setting this to 0 eventually causes a crash when entities come on screen 

        //    b.unkn15 = 0;
        //    b.unkn16 = 0;
        //    b.unkn24 = 0;
        //    b.unkn25 = 0;
        //    b.unkn26 = 0;

        //    b.unkn01 = 0;
        //    b.unkn02 = 0;

        //    b.obj_num1 = 0;
        //    b.obj_num2 = 0;
        //    //Setting these ALL to 0 removed most of the entities, made the players different chars
        //    //AND the player characters were at different places within the world
        //    //Whatver does this comes after element (123/4)
        //    for(int x = 0; x < 123 / 4; ++x)
        //    {
        //        b.unkn99[x] = 0;
        //    }
        //    b.optional_type = 0;

        //    objectData[i] = b;
        //}

        for (int i = 0; i < mission.objectData.Count; ++i)
        {
            if (i == 300)
            {
                SWars.BaseObjectData b = mission.objectData[i];
                //for (int x = 0; x < 123; ++x)
                //{
                //    b.unkn99[x] = 0;
                //}
                //b.optional_type = 0;
                for (int x = 0; x < 48; ++x)
                {
                    b.additionalData.ADDITIONAL_DATA[x] = 0;
                }

                for (int x = 0; x < 36; ++x)
                {
                    b.optionalData.OPTIONAL_DATA[x] = 0;
                }

                mission.objectData[i] = b;
            }
        }


    }

    unsafe void SetEventState()
    {
        //for (int i = 0; i < eventData.Count; ++i)
        //{
        //    SWars.MissionEvent e = eventData[i];

        //    //for (int x = 0; x < 32; ++x)
        //    //{
        //    //    e.unknown[x] = 0;
        //    //}

        //    eventData[i] = eventData[169];
        //}

        //for (int i = 0; i < eventData.Count; ++i)
        //{
        //    SWars.MissionEvent e = eventData[i];

        //    for (int x = 0; x < 32; ++x)
        //    {
        //       if(e.unknown[x] == 44)
        //        {
        //            Debug.LogWarning("Event " + i + " references 300");
        //        }
        //    }
        //}
    }

    unsafe void SetUnknownState()
    {
        //unknown3.unknown3[0] = 0;
        //unknown3.unknown3[1] = 0;
        //unknown3.unknown3[2] = 0;
        //unknown3.unknown3[3] = 0;
    }

    unsafe void SetPlayerData()
    {
        for (int i = 0; i < 1240; ++i) {
          //playerData.player_data[i] = 0;
        }
    }

    void VisualiseObjects()
    {
        GameObject missionObjects = new GameObject("Mission Objects");
        missionObjects.transform.parent     = visualiseRoot.transform;
        missionObjects.transform.localScale = Vector3.one;
        for (int i = 0; i < mission.objectData.Count; ++i)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cube);
            o.name = "Object " + i;
            o.transform.parent = missionObjects.transform;

            ushort x = 0; ushort y = 0; ushort z = 0;
            SWars.Functions.SwarsMissionObjectPosition(mission.objectData[i], out x, out y, out z);

            o.transform.localPosition = new Vector3(x, y, z);

            SWarsGameObjectVis vis = o.AddComponent<SWarsGameObjectVis>();
            vis.sourceMission = this;
            vis.data = mission.objectData[i];
            vis.dataIndex = i;
        }
    }

    public void WriteObjectData(SWars.BaseObjectData data, int index)
    {
        mission.objectData[index] = data;
    }

    unsafe void WriteObjectData(SWars.BaseObjectData d, BinaryWriter writer)
    {
        int baseSize = 168;
        int optionalSize = 36;
        int additionalSize = 48;

        //The function crashes if we try and force the amount written?
        //So i'll just rewind and write over the incorrect data...
        long pos = writer.Seek(0, SeekOrigin.Current);
        SWars.Functions.WriteType<SWars.BaseObjectData>(writer, d/*, baseSize*/);
        writer.BaseStream.Seek(pos + baseSize, SeekOrigin.Begin);
        if (mission.header.type == 9 || mission.header.type == 11 || mission.header.type == 12)
        {
            SWars.Functions.WriteType<SWars.AdditionalObjectData>(writer, d.additionalData, additionalSize);
        }

        if (d.optional_type == 33 || d.optional_type == 64 || d.optional_type == 61)
        {
            SWars.Functions.WriteType<SWars.OptionalObjectData>(writer, d.optionalData, optionalSize);
        }
    }
}
