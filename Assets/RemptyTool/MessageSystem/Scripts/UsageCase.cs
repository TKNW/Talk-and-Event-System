using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RemptyTool.ES_MessageSystem;
using UnityEngine.UI;
using System.Diagnostics.Tracing;

[RequireComponent(typeof(ES_MessageSystem))]
public class UsageCase : MonoBehaviour
{
    private ES_MessageSystem msgSys;
    public UnityEngine.UI.Text uiText;
    public UnityEngine.UI.Image CCG;
    public TextAsset textAsset;
    public UnityEngine.UI.Text Name;
    private List<string> textList = new List<string>();
    private int textIndex = 0;
    private GameObject Panel;
    private bool Istalking = false;

    void Start()
    {
        Panel = GameObject.Find("TalkCanvas");
        Panel.SetActive(false);
        msgSys = this.GetComponent<ES_MessageSystem>();
        if (uiText == null)
        {
            Debug.LogError("UIText Component not assign.");
        }

        //add special chars and functions in other component.
        msgSys.AddSpecialCharToFuncMap("UsageCase", CustomizedFunction);
        msgSys.AddSpecialCharToFuncMap("Close", Closedialog);
        #region CCG
        msgSys.AddSpecialCharToFuncMap("M1", M1);
        msgSys.AddSpecialCharToFuncMap("M2", M2);
        msgSys.AddSpecialCharToFuncMap("A1", A1);
        #endregion
    }

    private void CustomizedFunction()
    {
        Debug.Log("Hi! This is called by CustomizedFunction!");
    }
    private void Closedialog()
    {
        Panel.SetActive(false);
        CCG.color = new Color(255,255,255,0);
        Name.color = new Color(255,255,255,0);
        Istalking = false;
    }
    #region SpicF
    private void M1()
    {
        ChangeSP("portrait_kohaku_01","大鳥");
    }
    private void M2()
    {
        ChangeSP("portrait_kohaku_05","大鳥");
    }
    private void A1()
    {
        ChangeSP("portrait_misaki_04", "美咲");
    }
    #endregion
    private void ReadTextDataFromAsset(TextAsset _textAsset)
    {
        textList.Clear();
        textList = new List<string>();
        textIndex = 0;
        var lineTextData = _textAsset.text.Split('\n');
        foreach (string line in lineTextData)
        {
            textList.Add(line);
        }
    }
    public void RunText()
    {
        if (!Istalking)
        {
            ReadTextDataFromAsset(textAsset);
            Istalking = true;
            Panel.SetActive(true);
        }
        //Continue the messages, stoping by [w] or [lr] keywords.
        msgSys.Next();
    }
    public void RunText(string Textname)
    {
        if (!Istalking)
        {
            textAsset = Resources.Load<TextAsset>("Text/"+ Textname);
            ReadTextDataFromAsset(textAsset);
            Istalking = true;
            Panel.SetActive(true);
        }
        //Continue the messages, stoping by [w] or [lr] keywords.
        msgSys.Next();


    }
    private void ChangeSP(string SPN,string name)
    {
        CCG.sprite = Resources.Load("PNG/"+ SPN,typeof(Sprite)) as Sprite;
        CCG.color = new Color(255, 255, 255, 1);
        Name.text = name;
        Name.color = new Color(255, 255, 255, 1);
    }
    public bool istalk()
    {
        return Istalking;
    }
    void Update()
    {

        if (Istalking)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                RunText();
            }

            //If the message is complete, stop updating text.
            if (msgSys.IsCompleted == false)
            {
                uiText.text = msgSys.text;
            }
            //Auto update from textList.
            if (msgSys.IsCompleted == true && textIndex < textList.Count)
            {
                msgSys.SetText(textList[textIndex]);
                textIndex++;
            }
        }
        /*if (Input.GetKeyDown(KeyCode.S))
        {
            //You can sending the messages from strings or text-based files.
            if (msgSys.IsCompleted)
            {
                msgSys.SetText("Send the messages![lr] HelloWorld![w]");
            }
        }*/
    }
}
