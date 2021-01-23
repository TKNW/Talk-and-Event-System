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
#region Item class
public enum ItemType
{
    Material,
    Use,
    Key,
    Other
}
[Serializable]
public class Item
{
    public int Number;
    public string Name;
    public ItemType Type;
    public Item(int num, string name, ItemType type)
    {
        Number = num;
        Name = name;
        Type = type;
    }
    public Item()
    {

    }
}
[Serializable]
public class AllItems
{
    public Item[] Items;
}
[Serializable]
public class PlayerItem : Item
{
    public int Amount;
    public PlayerItem(int num, string name, int amount)
    {
        Number = num;
        Name = name;
        Amount = amount;
    }
}
#endregion
public class MainSystem : MonoBehaviour
{
    public AllItems LoadItems;
    public AllMissions LoadedMissions;
    private int NowMissionNumber;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadResource();
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
    private void LoadResource()
    {
        var jsonText = Resources.Load<TextAsset>("Json/Mission");
        LoadedMissions = JsonUtility.FromJson<AllMissions>(jsonText.text);
        jsonText = Resources.Load<TextAsset>("Json/Item");
        LoadItems = JsonUtility.FromJson<AllItems>(jsonText.text);
        Debug.Log("Name:" + LoadItems.Items[0].Name + " No." + LoadItems.Items[0].Number+" Type:" + LoadItems.Items[0].Type);
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
        Debug.LogWarning("AddMission: Can't find TargetMission.");
        return;
    }
    public void SetNowMissionNum(int num)
    {
        NowMissionNumber = num;
    }
}
