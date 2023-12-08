using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//毛巾与水发生碰撞的脚本

public class TowelCollider : MonoBehaviour
{
    GameMain gameMain;
    float t=0;
    
    // Start is called before the first frame update
    void Start()
    {
        gameMain = GameObject.FindObjectOfType<GameMain>();
    }
    /// <summary>
    /// 粒子碰撞
    /// 粒子碰撞是持续触发状态
    /// </summary>
    /// <param name="other"></param>
    private void OnParticleCollision(GameObject other)
    {
        t+=Time.deltaTime;
        if (t > 3)//等毛巾与水碰撞3秒，再设置为湿，关闭高亮
        {
            if (!GameMain.isWet)//在GameMain中isWet的初始值是false，毛巾是干的就进入这个判断
            {
                GameMain.isWet = true;//将毛巾设置为湿的
                gameMain.CloseTowelEmission();//关闭高亮

            }
        }
    }


    //将毛巾扣在脑袋上
     void OnTriggerEnter(Collider other)
    {
        if (GameMain.isWet)
        {
            if (other.name == "head")
            {
                transform.parent = other.transform;//将毛巾的父物体变为head
                gameMain.WetTowelFace();//调整湿毛巾在脸上的位置
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
