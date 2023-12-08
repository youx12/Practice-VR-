using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wvr;

public class PlayMove : MonoBehaviour
{
    WVR_DeviceType device = WVR_DeviceType.WVR_DeviceType_Controller_Right;
    WaveVR_SimplePointer pointer;
    bool isShow = false;
    public Texture[] textures = new Texture[4];
    int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        pointer = GetComponent<WaveVR_SimplePointer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pointer.GetHitTrans())
        {
            return;
        }
        GameObject obj = pointer.GetHitTrans().gameObject;
        if (WaveVR_Controller.Input(device).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Touchpad))
        {
            // transform.parent.position = pointer.GetHitPos()+Vector3.up*1.5f;
            if (
obj.name.Contains("Cube"))
            {
                // transform.parent.position = obj.transform.position + Vector3.up * 1.5f;
                // isShow = !isShow;
                // obj.transform.GetChild(0).GetChild(1).gameObject.SetActive(isShow);
                obj.GetComponent<MeshRenderer>().material.mainTexture = textures[index];
                index++;
                if (index == 4)
                {
                    index = 0;
                }
            }
        }
    }
}
