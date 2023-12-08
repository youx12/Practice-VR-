using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;

public struct RayClickedArgs//射线照射到的物体信息
{
    public WVR_DeviceType device;
    public float padx, pady;//按下圆盘的XY坐标
    public Vector3 hitpos;
    public Transform target;
}
public struct RayPointerArgs//射线信息
{
    public WVR_DeviceType device;
    public uint flags;
    public float distance;
    public Transform target;
}

public delegate void RayClickedEventHandler(RayClickedArgs e);
//定义委托 参数类型为 结构体1
public delegate void RayPointerEventHandler(RayPointerArgs e);
//定义委托 参数类型为结构体2
public class WaveVR_RayEvent : MonoBehaviour
{
    //结构体1 定义两个事件 Trigger按下  Pad按下
    public event RayClickedEventHandler RayTiggerClicked;
    public event RayClickedEventHandler RayPadClicked;

    //结构体2定义两个事件 射线进入的时候 射线退出的时候
    public event RayPointerEventHandler RayPointerIn;
    public event RayPointerEventHandler RayPointerOut;

    public virtual void OnRayTiggerClicked(RayClickedArgs e)
    {
        if (RayTiggerClicked != null)
            RayTiggerClicked( e);
    }

    public virtual void OnRayPadClicked(RayClickedArgs e)
    {
        if (RayPadClicked != null)
            RayPadClicked(e);
    }

    public virtual void OnRayPointerIn( RayPointerArgs e)
    {
        if (RayPointerIn != null)
            RayPointerIn(e);
    }

    public virtual void OnRayPointerOut(RayPointerArgs e)
    {
        if (RayPointerOut != null)
            RayPointerOut(e);
    }

    static public WaveVR_RayEvent Get(GameObject go)
    {
        WaveVR_RayEvent rayevent = go.GetComponent<WaveVR_RayEvent>();
        if (rayevent == null)
            rayevent = go.AddComponent<WaveVR_RayEvent>();
        return rayevent;
    }
}
