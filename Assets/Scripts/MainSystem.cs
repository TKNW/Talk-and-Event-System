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
[Serializable]
public class PlayerMissions
{
    public List<int> Missions;
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
    public int Amount = 0;
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
    public Item CheckItemisvalid(string name)
    {
        foreach(Item i in Items)
        {
            if(i.Name == name)
            {
                Debug.Log("Valid"); 
                return i;
            }
        }
        Debug.LogError("Check: Item's name is not exist.");
        return null;
    }
    public Item CheckItemisvalid(int number)
    {
        foreach (Item i in Items)
        {
            if (i.Number == number)
            {
                return i;
            }
        }
        Debug.LogError("Check: Item's name is not exist.");
        return null;
    }
}
[Serializable]
public class PlayerItems
{
    public List<Item> Items;
    public void AddItem(Item item)
    {
        foreach(Item i in Items)
        {
            if(item.Name == i.Name)
            {
                i.Amount++;
                return;
            }
        }
        item.Amount = 1;
        Items.Add(item);
        foreach(Item i in Items)
        {
            Debug.Log("Name:" +i.Name +" Type:"+ i.Type +"Number:" + i.Number + "Amount:" + i.Amount);
        }
        return;
    }
    public void AddItem(Item item,int number)
    {
        foreach (Item i in Items)
        {
            if (item.Name == i.Name)
            {
                i.Amount+=number;
                return;
            }
        }
        item.Amount = number;
        Items.Add(item);
        foreach (Item i in Items)
        {
            Debug.Log("Name:" + i.Name + " Type:" + i.Type + "Number:" + i.Number + "Amount:" + i.Amount);
        }
        return;
    }
    public void DeleteItem(Item item)
    {
        foreach (Item i in Items)
        {
            if (item.Name == i.Name && i.Amount > 1)
            {
                i.Amount--;
                return;
            }
        }
        Items.Remove(item);
        return;
    }
    public void DeleteItem(Item item,int number)
    {
        foreach (Item i in Items)
        {
            if (item.Name == i.Name && i.Amount > number)
            {
                i.Amount-=number;
                return;
            }
        }
        Items.Remove(item);
        return;
    }
}
#endregion
public class MainSystem : MonoBehaviour
{
    public AllItems LoadItems;
    public AllMissions LoadedMissions;
    public PlayerItems Item_in_hands;
    public PlayerMissions Mission_in_activate;
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
        #endregion
    }
    public void SetnowMission(int _number)
    {
        NowMissionNumber = _number;
    }
    public void AddMission()
    {
        if (Mission_in_activate.Missions.Exists(x => x == NowMissionNumber))
        {
            Debug.LogWarning("This Mission has already added.");
            return;
        }
        Debug.Log(LoadedMissions.Missions[NowMissionNumber - 1].NPCName);
        NPCcontroller NPC = GameObject.Find(LoadedMissions.Missions[NowMissionNumber - 1].NPCName).GetComponent<NPCcontroller>();
        NPC.MissionAccept();
        Mission_in_activate.Missions.Add(NowMissionNumber);
        return;
    }
    public bool CheckMissionisComplete(int missionnumber)
    {
        foreach(Target target in LoadedMissions.Missions[missionnumber - 1].Tar)
        {
            Item itemforcheck = Item_in_hands.Items.Find(x => x.Name == target.TargetName);
            if(itemforcheck == null || itemforcheck.Number < target.TargetNum)
            {
                Debug.Log("False");
                return false;
            }
        }
        foreach (Target target in LoadedMissions.Missions[missionnumber - 1].Tar)
        {
            DropItemfromPlayer(target.TargetName,target.TargetNum);
        }
        Debug.Log("True");
        return true;
    }
    public bool AddItemtoPlayer(string _itemname)
    {
        Item target = LoadItems.CheckItemisvalid(_itemname);
        if(target != null)
        {
            Item_in_hands.AddItem(target);
            return true;
        }
        return false;
    }
    public bool AddItemtoPlayer(string _itemname,int _number)
    {
        Item target = LoadItems.CheckItemisvalid(_itemname);
        if (target != null)
        {
            Item_in_hands.AddItem(target,_number);
            return true;
        }
        return false;
    }
    public bool DropItemfromPlayer(string _itemname)
    {
        Item target = LoadItems.CheckItemisvalid(_itemname);
        if(target != null)
        {
            Item_in_hands.DeleteItem(target);
            return true;
        }
        return false;
    }
    public bool DropItemfromPlayer(string _itemname,int _number)
    {
        Item target = LoadItems.CheckItemisvalid(_itemname);
        if (target != null)
        {
            Item_in_hands.DeleteItem(target,_number);
            return true;
        }
        return false;
    }
}
