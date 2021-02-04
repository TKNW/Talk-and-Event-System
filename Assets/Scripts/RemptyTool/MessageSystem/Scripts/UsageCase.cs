using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RemptyTool.ES_MessageSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Diagnostics.Tracing;

[RequireComponent(typeof(ES_MessageSystem))]
public class UsageCase : MonoBehaviour
{
    private ES_MessageSystem msgSys;
    private MainSystem MaSys;

    [Header("For talking")]
    public Text uiText;
    public Image CCG;
    public TextAsset textAsset;
    public Text Name;
    [Header("For selecting")]
    public Text Answer1;
    public Text Answer2;

    private List<string> textList = new List<string>();
    private int textIndex = 0;
    private GameObject Panel;
    private GameObject Select;
    private bool Istalking = false;
    private bool Isselecting = false;
    private bool Isans1;
    void Start()
    {
        Panel = GameObject.Find("TalkCanvas");
        Select = GameObject.Find("SelectCanvas");
        Panel.SetActive(false);
        Select.SetActive(false);
        CCG.color = new Color(255, 255, 255, 0);
        Name.color = new Color(0, 0, 0, 0);
        msgSys = this.GetComponent<ES_MessageSystem>();
        MaSys = this.GetComponent<MainSystem>();
        if (uiText == null)
        {
            Debug.LogError("UIText Component not assign.");
        }
        //add special chars and functions in other component.
        msgSys.AddSpecialCharToFuncMap("AcceptMission", TakeMission);
        msgSys.AddSpecialCharToFuncMap("Close", Closedialog);
        msgSys.AddSpecialCharToFuncMap("Select", AwakeSelect);
        msgSys.AddSpecialCharToFuncMap("Ans1", Skiptalk1);
        msgSys.AddSpecialCharToFuncMap("Ans2", Skiptalk2);
        #region CCG
        msgSys.AddSpecialCharToFuncMap("M1", M1);
        msgSys.AddSpecialCharToFuncMap("M2", M2);
        msgSys.AddSpecialCharToFuncMap("M6", M6);
        msgSys.AddSpecialCharToFuncMap("A1", A1);
        msgSys.AddSpecialCharToFuncMap("A4", A4);
        msgSys.AddSpecialCharToFuncMap("A5", A5);
        #endregion
    }
    private void Closedialog()
    {
        Panel.SetActive(false);
        CCG.color = new Color(255,255,255,0);
        Name.color = new Color(0,0,0,0);
        uiText.text = "";
        Name.text = "";
        Istalking = false;
    }
    private void AwakeSelect()
    {
        Isselecting = true;
        uiText.text = textList[textIndex];
        textIndex++;
        Answer1.text = textList[textIndex];
        textIndex++;
        Answer2.text = textList[textIndex];
        textIndex++;
        StartCoroutine("highlightBtn");
        Select.SetActive(true);
    }
    private void Skiptalk1()
    {
        if (Isans1)
        {
            textIndex++;
            return;
        }
        else if(!Isans1)
        {
            int temp = 0;
            if (!int.TryParse(textList[textIndex], out temp))
                Debug.LogError("There is no number for skip!");
            else
                textIndex += int.Parse(textList[textIndex]) + 1;
        }
    }
    private void Skiptalk2()
    {
        if (!Isans1)
        {
            textIndex++;
            return;
        }
        else if (Isans1)
        {
            int temp = 0;
            if (!int.TryParse(textList[textIndex], out temp))
                Debug.LogError("There is no number for skip!");
            else
                textIndex += int.Parse(textList[textIndex]) + 1;
        }
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
    private void M6()
    {

        ChangeSP("portrait_kohaku_06", "大鳥");
    }
    private void A1()
    {
        ChangeSP("portrait_misaki_01", "美咲");
    }
    private void A4()
    {
        ChangeSP("portrait_misaki_04", "美咲");
    }
    private void A5()
    {
        ChangeSP("portrait_misaki_05", "美咲");
    }
    #endregion
    //for button control
    public void Selectans1()
    {
        Isans1 = true;
        Isselecting = false;
        Select.SetActive(false);
    }
    public void Selectans2()
    {
        Isans1 = false;
        Isselecting = false;
        Select.SetActive(false);
    }
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
        Name.color = new Color(0, 0, 0, 1);
    }
    public bool istalk()
    {
        return Istalking;
    }
    protected static EventSystem eventSystem
    {
        get { return GameObject.Find("EventSystem").GetComponent<EventSystem>(); }
    }
    IEnumerator highlightBtn()
    {
        eventSystem.SetSelectedGameObject(null);
        yield return null;
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
    private void TakeMission()
    {
        MaSys.AddMission();
    }
    void Update()
    {

        if (Istalking && !Isselecting)
        {
            if (Input.GetKeyDown(KeyCode.Return))
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
