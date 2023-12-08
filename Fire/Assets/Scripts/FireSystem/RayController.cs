using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using wvr;

public class RayController : MonoBehaviour
{
    //定义手柄
    public WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
    WaveVR_SimplePointer pointer;//射线检测
    GameObject hitObj;
    GameMain gameMain;

   

    // Start is called before the first frame update
    void Start()
    {
        pointer = GetComponent<WaveVR_SimplePointer>();//初始化射线检测
        gameMain = GameObject.Find("WaveVR").GetComponent<GameMain>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!pointer.GetHitTrans())
        {
            //没有扫到路点，路点大小要恢复
            gameMain.ChangeMarkScale(false);
            return;
        }
        hitObj = pointer.GetHitTrans().gameObject;
        if (WaveVR_Controller.Input(device).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Touchpad))
        {
            if (hitObj.name.Contains("ConfirmButton"))//如果射线点击到的Button包含ConfirmButton的话，就跳转路点（ConfirmButton是面板上Button取名字时跳转路点按钮的）
            {
                gameMain.ConfirmButtonEvent(hitObj.name);
                
            }
            if (hitObj.name.Contains("ChoseButton"))
            {
                gameMain.ChoseButtonEvent(hitObj.name);
               
            }
            //如果射线检测到的物体的名字包含PositionMarker，调用GameMain中的移动位置的函数TeleportPos()，
            if (hitObj.name=="PositionMarker")
            {
                //射线检测到时mark路点，调用GameMain中的移动位置函数和路点大小改变函数
                gameMain.TeleportPos();
            }
            if (hitObj.name == "Brake")
            {
                //点击到的物体是Brake电闸，就断电
                gameMain.CloseBrake();
            }
            if(hitObj.name== "LouDao-49")
            {
                //点击到的是警铃，就播放警铃声音
                gameMain.HitWarining();
            }
            if (hitObj.name== "WaterTap")
            {
                //射线检测到水龙头打开水龙头
                gameMain.OpenWaterTap();
            }
            if(hitObj.name== "Towel")
            {
                //射线检测到毛巾，毛巾变为手柄的子物体
                gameMain.pickUpTowel();
            }
            if(hitObj.name == "loidao_dong_03")
            {
                //射线检测到时灭火器，播放拿起灭火器动画
                gameMain.PlayMHQAnimation();
            }
            if (hitObj.name == "body002")
            {
                //射线检测到拿着灭火器的右手
                gameMain.PickUpMHQ();
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))//替换扳机键
        {
            gameMain.EnableMHQSmoke();
        }

        //扫到物体名字是PositionMarker，就改变调用GameMain中的ChangeMarkScale()函数改变路点大小，就是扫到路点，路点变大
        if (hitObj.name == "PositionMarker")
        {
            gameMain.ChangeMarkScale(true);
        }
       

    }
}
