using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    private UsageCase talkcontrol;
    private MainSystem Masys;
    private bool cantalk = false;
    private string talktext;
    private Collider2D Others;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "NPC":
                cantalk = true;
                Others = other;
                break;
            default:
                break;
        }
        //animator.SetTrigger(hashDamage);  
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "NPC":
                cantalk = false;
                talktext = "";
                break;
            default:
                break;
        }
    }
    void Start()
    {
        talkcontrol = GameObject.Find("MsgSystem").GetComponent<UsageCase>();
        Masys = GameObject.Find("MsgSystem").GetComponent<MainSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cantalk)
        {
            Talk();
        }
        
    }
    void Talk()
    {
        if (!talkcontrol.istalk())
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                talktext = Others.GetComponent<NPCcontroller>().Gettalktext();
                if (Others.GetComponent<NPCcontroller>().HasMission)
                {
                    Debug.Log(Others.GetComponent<NPCcontroller>().MissionNumber);
                    Masys.SetNowMissionNum(Others.GetComponent<NPCcontroller>().MissionNumber);
                }
                talkcontrol.RunText(talktext);
            }
        }
    }
}
