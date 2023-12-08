using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMain : MonoBehaviour
{
    List<GameObject> ui_Panels = new List<GameObject>();//存UICanvas上的panels
    GameMain gameMain;
    List<GameObject> gamePanels=new List<GameObject>();//存Canvas上的Panels，就用警告界面的Panels
    GameObject brakeArrow;
    GameObject grayHand;
    GameObject redHand;
    GameObject handArrow;
    
    // Start is called before the first frame update
    private void Awake()//将其生命周期提升两个等级，一开始就将面板存在ui_Panels中
    {
        //遍历UICanvas中的Panels
        for (int i = 0; i < 20; i++)//已知面板个数，遍历面板
        {
            ui_Panels.Add(transform.GetChild(i).gameObject);//获取UICanves的孩子，各个Panels，并将它们存在ui_panels中
            ui_Panels[i].SetActive(false);//给所有的ui_Panels失活
        }

        gameMain = GameObject.FindObjectOfType<GameMain>();//查找面板具有GameMain脚本的物体,也就是WaveVR
        //gameMain = GameObject.Find("WaveVR").GetComponent<GameMain>();
        
        //遍历Canvas中Panels
        for(int i = 0; i < gameMain.transform.GetChild(1).GetChild(0).childCount; i++)
        {
            gamePanels.Add(gameMain.transform.GetChild(1).GetChild(0).GetChild(i).gameObject);
        }
        //获得UICanvas上的BrakeArrow面板
        brakeArrow = transform.Find("BrakeArrow").gameObject;
        //获取UICanvas上的灰手和红手
        grayHand = transform.Find("PanelGrayHand").gameObject;
        redHand = transform.Find("PanelRedHand").gameObject;
        //灭火器上面的箭头
        handArrow = transform.Find("HandArrow").gameObject;

    }


    /// <summary>
    /// 调用的UI激活与失活
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isShow"></param>
    public void ShowPanel(int index, bool isShow)//调用的UI失活与激活
    {
        
        ui_Panels[index].SetActive(isShow);
    }
    /// <summary>
    /// 调用警告面板的激活与失活
    /// </summary>
    /// <param name="index"></param>
    /// <param name="isShow"></param>
    public void HeadPanel(int index,bool isShow)
    {
        gamePanels[index].SetActive(isShow);
    }
    /// <summary>
    /// 开启电闸上面那个箭头的面板
    /// </summary>
    public void EnableBrakeArrow(bool isShow)
    {
        brakeArrow.SetActive(isShow);
    }
    /// <summary>
    /// 灰色的手和红色的手（测门温那里)
    /// </summary>
    public void EnableGrayHand(bool isShow)
    {
        grayHand.SetActive(isShow);
    }
    public void EnableRedHand(bool isShow)
    {
        redHand.SetActive(isShow);
        redHand.GetComponent<Animator>().enabled=true;
    }
    /// <summary>
    /// UI重置。即将离开教室，去到楼道
    /// 是指将原本在前门处的Panel5和白手设置到后门处，这里直接给的具体坐标
    /// </summary>
    public void ClassRoomUIRest()
    {
        ui_Panels[5].transform.localPosition = new Vector3(3007, 267, -1397);
        grayHand.transform.localPosition = new Vector3(3000, 234, -1417);
    }

    /// <summary>
    /// 指引点击灭火器上方箭头
    /// 判断那个箭头是否显示
    /// </summary>
    public void EnableHandArrow(bool isShow)
    {
        handArrow.SetActive(isShow);
    }

    /// <summary>
    /// 点击确认开始火灾体验的按钮
    /// </summary>
    /// <param name="name"></param>
    public void ConfirmButtonConnect(string name)
    {
        int num=int.Parse(name.Substring(name.LastIndexOf("n")+1));//切割，获得字母n以后的索引 
        print(num);
        if (num == 8)
        {
            gameMain.GoToClassRoom();
            //gameMain.GoToLouDao();
           


        }
        if (num == 1)
        {
            ShowPanel(1, false);
            ShowPanel(2, true);
        }
        if(num == 3)
        {
            ShowPanel(3, false);
            ShowPanel(2, true);
        }
        if (num == 6)
        {
            ShowPanel(6, false);
            gameMain.ShowMark(9);
        }
        if (num == 7)
        {
            gameMain.GoToLouDao();
        }
        if (num == 10)
        {
            gameMain.GoToLouTi();
        }
        if (num == 9)
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// 报警电话选择的按钮
    /// </summary>
    /// <param name="name"></param>
    public void ChoseButtonConnect(string name)
    {
        int num = int.Parse(name.Substring(name.Length - 1));
        if(num==1)
        {
            ShowPanel(2, false);
            ShowPanel(4, true);
            Invoke("WaitSuccess", 3);
            AudioManager.instance.PlayAudio(18);
        }
        else
        {
            AudioManager.instance.PlayAudio(4);
            ShowPanel(3, true);
            ShowPanel(2, false);
        }
    }

    public void WaitSuccess()
    {
        ShowPanel(4, false);
        gameMain.ShowMark(7);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
