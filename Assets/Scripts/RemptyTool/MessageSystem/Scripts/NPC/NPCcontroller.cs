﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCcontroller : MonoBehaviour
{
    MainSystem Masys;
    [Header("NPC Setting")]
    public string NPC_Name;
    public string[] DialogName;
    [SerializeField] private bool IsActMission = false;
    [HideInInspector] public bool HasMission;
    [HideInInspector] public string MissionName;
    [HideInInspector] public int MissionNumber;

    public string Gettalktext()
    {
        if (!HasMission)
        {
            return DialogName[0];
        }
        else
        {
            if (!IsActMission)
            {
                Masys.SetnowMission(MissionNumber);
                return "Missions/Mission" + MissionNumber + "Start";
            }
            else if (IsActMission)
            {
                if (CheckCompleted())
                {
                    MissionComplete();
                    return "Missions/Mission" + MissionNumber + "Finish";
                }
                else
                    return "Missions/Mission" + MissionNumber + "Stuck";
            }
        }
        Debug.Log("Gettalktext: There's no Dialog to say.");
        return "";
    }
    public void MissionAccept()
    {
        IsActMission = true;
    }
    private bool CheckCompleted()
    {
        return Masys.CheckMissionisComplete(MissionNumber);
    }
    private void MissionComplete()
    {
        IsActMission = false;
        HasMission = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        Masys = GameObject.Find("MsgSystem").GetComponent<MainSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
