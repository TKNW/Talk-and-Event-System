using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
#region Mission class
public enum MissionType
{
    Collect,
    Find,
    Kill
}
struct target
{
    public target(int tnumber,string name)
    {
        Targetnumber = tnumber;
        Targetname = name;
    }
    public int Targetnumber;
    public string Targetname;
}
[Serializable]
public class AllMissions
{
    public Mission[] Missions;
}
[Serializable]
public class Mission
{
    public string Name;
    public int Number;
    public int Phase;
    public string Textname;
    public bool Iscompleted;
    public MissionType Type;
    public string NPCName;
    public Target[] Tar;
    public Mission(){}
    public Mission(string name,int number, string textname,MissionType type)
    {
        Name = name;
        Number = number;
        Textname = textname;
        Iscompleted = false;
        Type = type;
    }
}
[Serializable]
public class Target
{
    public string TargetName;
    public int TargetNum;
}
#endregion
public class MainSystem : MonoBehaviour
{
    public AllMissions LoadedMissions;
    private int NowMissionNumber;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadMission();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #region Load
    private void LoadMission()
    {
        var jsonText = Resources.Load<TextAsset>("Json/Mission");
        LoadedMissions = JsonUtility.FromJson<AllMissions>(jsonText.text);
        #endregion
    }
    public void AddMission()
    {
        GameObject[] NPCs = GameObject.FindGameObjectsWithTag("NPC");
        foreach(GameObject i in NPCs)
        {
            if (i.GetComponent<NPCcontroller>().MissionNumber == NowMissionNumber)
            {
                i.GetComponent<NPCcontroller>().MissionAccept();
                return;
            }
        }
        Debug.Log("AddMission: Can't find TargetMission.");
        return;
    }
    public void SetNowMissionNum(int num)
    {
        NowMissionNumber = num;
    }
}
