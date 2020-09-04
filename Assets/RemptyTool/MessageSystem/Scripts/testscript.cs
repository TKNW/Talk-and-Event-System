using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testscript : MonoBehaviour
{
    private UsageCase talkcontrol;
    private bool cantalk = false;
    private string talktext;
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "NPC":
                cantalk = true;
                talktext = other.GetComponent<NPCcontroller>().Gettalktext();
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
    }

    // Update is called once per frame
    void Update()
    {
        if (cantalk)
        {
            if (!talkcontrol.istalk())
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    talkcontrol.RunText(talktext);
                }
            }
        }
        
    }
}
