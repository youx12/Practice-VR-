using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    GameObject classRoom;
    GameObject louDao;
    UIMain uiMain;//在开始界面需要显示开始界面的UI
    int currentIndex=0;//路点切换的索引值
    int pointCount;//路点总数量
    GameObject mark;//定义路点
    GameObject brake;//定义电闸开光
    GameObject light;//定义灯
    GameObject loudao_Somke;//定义楼道烟，进入楼道后就要开始冒烟

    GameObject classSmoke;//定义教室的烟雾，发生火灾后，教室会冒烟

    GameObject waterTap;//定义水龙头

    GameObject towel;//定义毛巾

    GameObject DT;//定义电梯

    GameObject boom;//定义爆炸特效

    GameObject door;//定义要爆炸的那个教室的门

    GameObject MHQ;//定义灭火器
    GameObject MHQAnimation;//定义灭火器动画
    GameObject rightHand;//定义拿住灭火器的右手

    Vector3 localPos;//灭火器初始点坐标
    Vector3 localRot;//灭火器初始点角度

    bool isPickUp = false;//判断是否灭火在射线上

    public static bool isWet = false;//判断毛巾是否是湿的，开始的时候毛巾是干的，所以设置为false
    public static bool isFire = true;//判断火是否是着的

    GameObject fireBox;//地面着火的箱子（灭火器灭火那里）

    //public List<Vector3> pointPoss = new List<Vector3>();//用于存储前进的路点
    private Vector3[] pointPoss =
    {
        new Vector3(-4.7f,2.02f,-60),//0,出生点
        new Vector3(16.2f,0.01f,-3.8f),//1.教室门口
        new Vector3(17.2f,0.01f,-2.8f),//2.教室门口与课桌之间
        new Vector3(18,0.01f,-1.3f), //3课桌座位
        new Vector3(19.4f,0.01f,-2.4f),//4讲桌前
        new Vector3(21.9f,0.01f,-2.4f),//5电闸前
        new Vector3(20,0.01f,-2.4f),//6回到讲桌前（119等电话）
        new Vector3(17.5f,0.01f,-2.5f),//7.门口与讲台之间
        new Vector3(16,0.01f,-3.5f),//8.前门
        new Vector3(15.5f,0.01f,0),//9.前门与后门之间
        new Vector3(16,0.01f,3.9f),//10.后门  
        new Vector3(-3.8f,2.02f,-57.6f),//11.走廊警铃旁         
        new Vector3(-4.7f,2.02f,-53),//12 //离开警铃的第一个点
        new Vector3(-4.7f,2.02f,-47.2f),//13水池旁楼道
        new Vector3(-6.9f,2.02f,-45.1f),//14//水池旁
        new Vector3(-4.8f,2.02f,-43),//15
        new Vector3(-4.8f,2.02f,-38f),//16
         new Vector3(-4.8f,2.02f,-33f),//17到达电梯门前的点
         new Vector3(-4.8f,2.02f,-23f),//18 电梯之后的点
         new Vector3(-4.8f,2.02f,-17.5f),//19
         new Vector3(-4.8f,2.02f,-11.3f),//20
         new Vector3(-5.3f, 2.02f, -29f),   //21 电梯门口
         new Vector3(-7.3f, 2.02f, -29f),  //22 电梯内
         new Vector3(-4.8f, 2.02f, -6.5f),  //23 
         new Vector3(-4.8f, 2.02f, -0.1f), //24   灭火器处
         new Vector3(-4.8f, 2.02f, 3.3f),   //25    走廊尽头
         new Vector3(-4.8f, 2.02f, 6.7f),   //26    逃生通道
         new Vector3(-9.9f, 0.4f, 6.8f),   //27
         new Vector3(-9.6f, 0.4f, 8.3f),   //28
         new Vector3(-4.4f, -1.2f, 7.9f) //29
    };

    // Start is called before the first frame update
    void Start()
    {
        //先找到楼道和教室，用find方法找，也可以用拖拽的方法，但是拖拽的方法不适合要拖拽很多物体的时候
        louDao = GameObject.Find("LouDao");
        classRoom = GameObject.Find("classroom");
        classRoom.SetActive(false);//游戏开始界面在楼道中，先让教室失活
        louDao.SetActive(true);//让楼道激活
        uiMain = GameObject.Find("UICanvas").GetComponent<UIMain>();//找到UICanvas，通过找到UICanvas调用其上挂载的UIMain脚本，调用UIMain脚本中的ShowPanel激活Panel17,也就是开始界面的开始面板(初始化)
        uiMain.ShowPanel(17, true);//激活panel17s
        pointCount = pointPoss.Length;//将链表中路点数量复制给pointCount，这样路点总数量就存储在了pointCount中
        mark = GameObject.Find("PositionMarker");//找到路点并且复制给mark
        classSmoke = classRoom.transform.GetChild(0).gameObject;//上面获取到了教室classRoom，smoke是classroom的第一个孩子，可以通过classroom获取它的第一个孩子
        brake = classRoom.transform.GetChild(2).Find("Brake").gameObject;//找到电闸
        light = classRoom.transform.GetChild(1).gameObject;//找到教室灯
        loudao_Somke=louDao.transform.GetChild(2).GetChild(2).gameObject;//找到楼道烟
        waterTap = louDao.transform.GetChild(0).Find("ShuiLongTou").GetChild(0).GetChild(0).gameObject;//找到水龙头
        towel = louDao.transform.GetChild(0).Find("Towel").gameObject;//找到毛巾
        DT = louDao.transform.GetChild(0).Find("elevatorOpen").gameObject;//找到电梯
        boom = louDao.transform.GetChild(2).GetChild(3).gameObject;//找到爆炸特效
        door = louDao.transform.GetChild(0).Find("door break").gameObject;//找到要爆炸教室的门
        MHQAnimation = louDao.transform.GetChild(0).Find("miehuoqi_dh_01").gameObject;//通过楼道找到灭火器动画
        MHQ = MHQAnimation.transform.GetChild(2).gameObject;//通过灭火器动画找到灭火器
        rightHand = MHQAnimation.transform.GetChild(1).gameObject;//通过灭火器动画找到拿灭火器的手
        localPos = MHQ.transform.localPosition;//记录灭火器初始点坐标
        localRot = MHQ.transform.localEulerAngles;//记录灭火器初始角度
        fireBox = louDao.transform.GetChild(2).GetChild(0).GetChild(2).gameObject;//找到楼道中着火的箱子（灭火器旁边）
        AudioManager.instance.PlayAudio(11);//播放开始音乐
    }

    /// <summary>
    /// 确认按钮点击事件
    /// </summary>
    /// <param name="name"></param>
    public void ConfirmButtonEvent(string name)//跳转路点函数,string name用来切割函数，ConfirmButton只有前面的是一样的，根据后面的数字判断在哪个路点，所以将数字切割开来
    {
        uiMain.ConfirmButtonConnect(name);
    }
    /// <summary>
    /// 确认按钮点击事件（报警电话的按钮）
    /// </summary>
    /// <param name="name"></param>
    public void ChoseButtonEvent(string name)
    {
        uiMain.ChoseButtonConnect(name);
    }
    public void GoToClassRoom()
    {
        
        classRoom.SetActive(true);
        louDao.SetActive(false);
        uiMain.ShowPanel(17, false);
        currentIndex = 1;//跳转到教室，为第二个点
        TeleportPos();
        AudioManager.instance.PlayAudio(0);
    }



    /// <summary>
    /// 测试教室后门温度，可以离开，激活楼道，失活教室
    /// </summary>
    public void GoToLouDao()
    {
        louDao.SetActive(true);
        classRoom.SetActive(false);
        uiMain.ShowPanel(7, false);
        currentIndex = 11;//跳转到楼道
        //currentIndex = 25;
        TeleportPos();
    }
    /// <summary>
    /// 走楼梯逃跑
    /// </summary>
    public void GoToLouTi()
    {
        //路点变为26个点，调用改变路点的方法，为了UIMain中好调用，当点击了确认按钮后，就到楼梯去
        currentIndex = 26;
        TeleportPos();
        uiMain.ShowPanel(16, false);
        uiMain.ShowPanel(18, true);
    }


    /// <summary>
    /// 用于路点坐标切换
    /// </summary>
    public void TeleportPos()
    {
        if (currentIndex>pointCount-1)//当路点索引大于路点总数量的最大值的时候，结束操作，什么也不做。路点索引无法大于路点总数量
        {
            return;
        }

        if (currentIndex != 22)//如果说路点不等于22的时候就会移动（currentIndex == 22时是电梯里面的路点，角色不能进入电梯）
        {
            //否则就将索引值的位置给当前位置，达到移动的效果。这句话是移动角色，当射线点击路点的时候，就会移动到该路点
            transform.position = pointPoss[currentIndex]+Vector3 .up*1.5f;//上面给的是路点的值，所以要*1.5f，将视野的位置增高
        }

        if (currentIndex == 1)//索引值在传送到教室（GoToClassRoom()）的时候就给了，传送到教室是第二个点，索引值是1
        {
            ////如果路点索引是1，也就是在教室的位置，ShowMark的索引值就是2，显示下一个位置的路点
            ShowMark(2);
        }
        else if (currentIndex == 2)
        {
            ShowMark(3);
        }
        else if (currentIndex == 3)//回到自己的座位上，到达这个路点之后，等待三秒，开始发生火灾，这个时候就要唤醒发生火灾函数
        {
            Invoke("WaitClassFire", 3);            
        }
        else if(currentIndex == 4)//去提示关闭电源那个路点
        {
            uiMain.ShowPanel(0, true);//激活提示关闭电源的UI面板
            ShowMark(5);
        }
        else if (currentIndex == 5)//去关闭电源的路点
        {
            uiMain.ShowPanel(0, false);
            MaterialEmission.Instance.OpenEmission(brake);//开启电闸的高光
            uiMain.EnableBrakeArrow(true);//显示电闸上面箭头的UI面板
            
        }
        else if (currentIndex == 6)
        {

            uiMain.ShowPanel(1, true);

        }
        else if (currentIndex == 7)
        {
            ShowMark(8);
        }
        else if(currentIndex==8)
        {
            AudioManager.instance.PlayAudio(5);
            uiMain.ShowPanel(5, true);
            Invoke("DisableFrontDoorWarn", 3);
            
        }  
        else if (currentIndex == 9)
        {
            ShowMark(10);
        }
        else if( currentIndex == 10)
        {
            uiMain.ClassRoomUIRest();
            uiMain.ShowPanel(5, true);
            Invoke("WaitGrayHand", 2);
        }
        else if (currentIndex == 11)
        {
            AudioManager.instance.PlayAudio(8);
            uiMain.ShowPanel(8, true);
            transform.localEulerAngles = new Vector3(0,200,0);//强制WaveVR角度（强制当前挂载脚本物体的角度）
            loudao_Somke.SetActive(true);
        }
        else if (currentIndex == 12)
        {
            ShowMark(13);
        }
        else if (currentIndex == 13)
        {
            AudioManager.instance.PlayAudio(9);
            ShowMark(14);
            uiMain.ShowPanel(9, true);
        }
        else if (currentIndex == 14)
        {
            AudioManager.instance.PlayAudio(10);
            uiMain.ShowPanel(9, false);
            uiMain.ShowPanel(10, true);
            MaterialEmission.Instance.OpenEmission(waterTap);
        }
        else if(currentIndex == 15)
        {
            uiMain.HeadPanel(1, false);
            ShowMark(16);
        }
        else if (currentIndex == 16)
        {
            ShowMark(17);
            uiMain.ShowPanel(11, true);
            
        }
        else if (currentIndex == 17)
        {
            AudioManager.instance.PlayAudio(13);
            uiMain.ShowPanel(11, false);
            uiMain.ShowPanel(12, true);
            ShowMark(21);
        }
        else if (currentIndex == 21)
        {
            Invoke("OpenDT", 2);//延迟2秒调用电梯开门动画
            ShowMark(22);//将路点移动到电梯 里面
            uiMain.ShowPanel(12,false);
        }
        else if (currentIndex == 22)
        {
            AudioManager.instance.PlayAudio(15);
            uiMain.ShowPanel(14, true);
            ShowMark(18);
        }
        else if (currentIndex == 18)
        {
            uiMain.ShowPanel(14, false);
            ShowMark(19);
        }
        else if(currentIndex == 19)
        {
            AudioManager.instance.PlayAudio(14);
            door.GetComponent<Animation>().Play();//播放门爆炸的动画
            boom.SetActive(true);
            Invoke("DoorBreak", 1);//延迟1秒显示ui和路点
        }
        else if (currentIndex == 20)
        {
            ShowMark(23);
        }
        else if (currentIndex == 23)
        {
            uiMain.ShowPanel(15, true);
            ShowMark(24);
        }
        else if (currentIndex == 24)
        {
            AudioManager.instance.PlayAudio(16);
            uiMain.ShowPanel(15, false);
            MaterialEmission.Instance.OpenEmission(MHQ);//灭火器高亮
        }
        else if (currentIndex == 25)
        {
            uiMain.ShowPanel(16, true);
        }
        else if( currentIndex == 26)
        {
            AudioManager.instance.PlayAudio(17);
            ShowMark(27);
        }
        else if (currentIndex == 27)
        {
            uiMain.ShowPanel(18, false);
            ShowMark(28);
        }
        else if (currentIndex == 28)
        {
            ShowMark(29);
;       }
        else if(currentIndex==29)
        {
            AudioManager.instance.PlayAudio(20);
            uiMain.ShowPanel(19, true);
        }
    }


    /// <summary>
    /// 显示路点
    /// </summary>
    /// <param name="index"></param>
    public void ShowMark(int index)
    {
        if (index > pointCount - 1)
        {
            return;
        }
        //将移动的位置赋值给路点坐标，（减去高度，路点就在地上了）
        mark.transform.position = pointPoss[index];//给的位置就是路点坐标，所以不用减了，之前赋值的是视野位置，所以要减

        currentIndex = index;//更新索引,将index赋值给currentIndex，currenIndex是路点切换的索引值，index是下一个路点，这个意识就是，把路点的索引给路点切换的索引值，移动路点。就例如说，currenIndex==1，是现在的位置，而当前index==2，是在下一个位置，为了移动到下一个位置，就会点击下一个位置，路点切换的索引值就要改变成下一个位置的索引值，也就是index
    }

    /// <summary>
    /// 播放拿起灭火器动画
    /// </summary>
    public void PlayMHQAnimation()
    {
        MHQAnimation.transform.GetChild(0).gameObject.SetActive(true);//激活拿住灭火器的手
        rightHand.SetActive(true);//激活拿住灭火器插栓的手
        MHQAnimation.GetComponent<Animation>().Play();//播放拿起灭火器动画
        MHQ.GetComponent<BoxCollider>().enabled = false;//失活灭火器
        MaterialEmission.Instance.CloseEmission(MHQ);
        //延时调用显示箭头
        Invoke("ShowRightHand", 3);
    }

    /// <summary>
    /// 调用指引点击灭火器的箭头的方法和右手高亮方法
    /// </summary>
    public void ShowRightHand()
    {
        uiMain.EnableHandArrow(true);
        MaterialEmission.Instance.OpenEmission(rightHand);
    }
    /// <summary>
    /// 右手拿起灭火器
    /// </summary>
    public void PickUpMHQ()
    {
        //失活拿着灭火器的两只手
        for(int i = 0; i < 2; i++)
        {
            MHQAnimation.transform.GetChild(i).gameObject.SetActive(false);
        }
        MHQ.transform.parent = transform.GetChild(0);//将灭火器的父物体变为射线
        MHQ.transform.localPosition =Vector3.zero+ new Vector3();
        MHQ.transform.localEulerAngles =Vector3.zero+ new Vector3(-90,0,90);
        uiMain.EnableHandArrow(false);
        isPickUp = true;
    }


    /// <summary>
    /// 激活灭火器烟雾
    /// </summary>
    public void EnableMHQSmoke()
    {
        if (isPickUp)
        {
            MHQ.transform.GetChild(1).gameObject.SetActive(true);//激活灭火器烟雾
            MHQ.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
        }
    }

    /// <summary>
    /// 箱子上的火变小
    /// 播放火小的动画
    /// </summary>
    public void CloseFire()
    {
        fireBox.GetComponent<Animation>().Play();
        Invoke("ResetMHQ", 5);
    }

    //灭火后重置灭火器
    public void ResetMHQ()
    {
        MHQ.transform.GetChild(1).gameObject.SetActive(false);
        MHQ.transform.parent=MHQAnimation.transform;
        MHQ.transform.localPosition = localPos;
        MHQ.transform.localEulerAngles = localRot;
        ShowMark(25);
    }



    /// <summary>
    ///多媒体教室爆炸之后要显示的UI和路点
    /// </summary>
    public void DoorBreak()
    {
        uiMain.ShowPanel(13, true);
        ShowMark(20);
    }


    /// <summary>
    /// 在电梯门口等待2秒电梯打开
    /// 播放电梯开门动画
    /// </summary>
    public void OpenDT()
    {
        DT.GetComponent<Animation>().Play();//播放电梯开门动画
    }


    /// <summary>
    /// 测试前门温度,让提示前面温度过高面板失活
    /// </summary>
    public void DisableFrontDoorWarn()
    {
        uiMain.ShowPanel(5, false);
        uiMain.EnableGrayHand(true);
        Invoke("DisableGrayHand", 2);
    }

    public void WaitGrayHand()
    {
        AudioManager.instance.PlayAudio(7);
        uiMain.ShowPanel(5, false);
        uiMain.EnableGrayHand(true);
        Invoke("WaitOutDoor", 2);
    }
    public void WaitOutDoor()
    {
        uiMain.EnableGrayHand(false);
        uiMain.ShowPanel(7, true);
    }
    public void DisableGrayHand()
    {
        uiMain.EnableGrayHand(false);
        uiMain.EnableRedHand(true);
        Invoke("DisableRedHand", 3);
    }
    public void DisableRedHand()
    {
        AudioManager.instance.PlayAudio(6);
        uiMain.EnableRedHand(false);
        uiMain.ShowPanel(6, true);
    }

    
    /// <summary>
    /// 毛巾变为手柄的子物体
    /// </summary>
    public void pickUpTowel()
    {
        towel.transform.parent = transform.GetChild(0).GetChild(0);//将毛巾变为手柄的子物体
        towel.transform.localPosition = new Vector3(0, 0, 0.6f);
    }

    /// <summary>
    /// 调整湿毛巾在脸上的位置
    /// </summary>
    public void WetTowelFace()
    {
        AudioManager.instance.PlayAudio(19);
        towel.transform.localPosition = new Vector3(0, -0.22f, 0.3f);
        towel.transform.localEulerAngles = new Vector3(-161, -30, -151);
        ShowMark(15);
        uiMain.ShowPanel(10, false);//失活实施自救面板
        uiMain.HeadPanel(1, true);//激活警告离开卫生间ui
        AudioManager.instance.PlayAudio(12);
    }



    /// <summary>
    /// 打开水龙头
    /// 打开水龙头需要做到高亮关闭、播放水龙头动画，激活水流、毛巾高亮
    /// 激活水流是通过楼道，找到他的 孩子然后SetActive(true)
    /// 
    /// </summary>
    public void OpenWaterTap()
    {
        MaterialEmission.Instance.CloseEmission(waterTap);
        waterTap.transform.parent.parent.GetComponent<Animation>().Play();//播放水龙头动画
        louDao.transform.GetChild(2).GetChild(1).gameObject.SetActive(true);//激活水流
        MaterialEmission.Instance.OpenEmission(towel);//打开毛巾高亮
    }

    /// <summary>
    /// 关闭毛巾高亮
    /// 将毛巾的干湿状态设置为湿
    /// </summary>
    public void CloseTowelEmission()
    {
        MaterialEmission.Instance.CloseEmission(towel);
        
    }


    
    /// <summary>
    /// 敲响警铃
    /// </summary>
    public void HitWarining()
    {
        uiMain.ShowPanel(8, false);
        ShowMark(12);
    }

    /// <summary>
    /// 关闭电闸
    /// 播放关闭电闸动画
    /// 电闸高光失活
    /// 关闭电源
    /// </summary>
    public void CloseBrake()
    {
        AudioManager.instance.PlayAudio(2);
        brake.GetComponent<Animation>().Play();//播放关闭电闸动画
        MaterialEmission.Instance.CloseEmission(brake);//电闸高光失活
        uiMain.EnableBrakeArrow(false);//电闸上的箭头UI失活
        //关灯
        for (int i = 0; i < 2; i++)
        {
            light.transform.GetChild(i).GetComponent<Light>().enabled = false;
        }
        Invoke("OpenLight", 2);//关灯
    }

    //开灯
    public void OpenLight()
    {
        AudioManager.instance.PlayAudio(3);
        //遍历灯
        for (int i = 0; i < 2; i++)
        {
            light.transform.GetChild(i).GetComponent<Light>().enabled =true;
            light.transform.GetChild(i).GetComponent<Light>().intensity = 0.4f;
        }
        ShowMark(6);
    }

    /// <summary>
    /// 等待教室发生火灾
    /// 教室发生火灾要激活烟雾
    /// </summary>
    
    public void WaitClassFire()
    {
        classSmoke.SetActive(true);//激活烟雾
        uiMain.HeadPanel(0, true);//唤醒警告面板
        Invoke("DisablePanel0", 2);//等待2秒，唤醒失活警告面板，意思就是给警告面板闪2秒
        AudioManager.instance.PlayAudio(1);
    }
    public void DisablePanel0()
    {
        uiMain.HeadPanel(0, false);//失活Canvas上的第一个面板
        ShowMark(4);//激活下一个路点

    }




    /// <summary>
    /// 扫到路点mark ，改变大小
    /// </summary>
    /// <param name="isMark"></param>
    public void ChangeMarkScale(bool isMark)
    {
        if (isMark)
        {
            //扫到变大
            mark.transform.localScale = Vector3.one*0.6f;
        }
        else
        {
            //没扫到恢复原状
            mark.transform.localScale = Vector3.one*0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
