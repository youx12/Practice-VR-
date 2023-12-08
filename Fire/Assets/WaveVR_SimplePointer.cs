
using UnityEngine;
using System.Collections.Generic;
using wvr;
using WaveVR_Log;
using System;
using System.Text;
public class WaveVR_SimplePointer : MonoBehaviour
{
    public float pointerThickness = 0.002f;
    public float pointerLength = 100f;
    //public bool showPointerTip = true;
   
    public LayerMask layersToIgnore = Physics.IgnoreRaycastLayer;
    public WVR_DeviceType type;
    GameObject customPointerCursor;

    private GameObject pointerHolder;//射线起点
    private GameObject pointer;//射线
    private GameObject pointerTip;//射线尖端
    private Vector3 pointerTipScale = new Vector3(0.05f, 0.05f, 0.05f);//射线尖端大小
    protected Transform pointerContactTarget = null;//射线接触目标

        // material of customPointerCursor (if defined)
        private Material customPointerMaterial;//射线的正常材质


      //Pointer
    public Material pointerMaterial;//射线材质
    public Color pointerHitColor = new Color(0f, 0.5f, 0f, 1f); //点击颜色
    public Color pointerMissColor = new Color(0.8f, 0f, 0f, 1f);//丢失颜色
    public Color pointerModel = new Color(0f, 0.6f, 0.8f, 1f);//
    float pointerContactDistance = 0f;//射线接触的距离
    Vector3 destinationPosition; //目标点坐标
     Transform detalhititem = null;//碰到的物体
     Vector3 detalhitpos = Vector3.zero;//命中坐标
     float detaldistance = 0f;//距离
     Transform previousTarget = null;//之前的坐标

      void OnEnable()
    {
        var tmpMaterial = Resources.Load("WorldPointer") as Material;//首先加载材质
        if (pointerMaterial != null)//如果当前已经有材质了就用现有的
        {
            tmpMaterial = pointerMaterial;
        }

        pointerMaterial = new Material(tmpMaterial);//如果没有就用加载的材质
        pointerMaterial.color = pointerMissColor;//材质的颜色定义为没有扫到目标点的颜色
        InitPointer();
    }

      void OnDisable()
    {
        //base.OnDisable();
        if (pointerHolder != null)
        {
            Destroy(pointerHolder);
        }
    }

    void InitPointer()
    {
        pointerHolder = new GameObject(string.Format("[{0}]WorldPointer_SimplePointer_Holder", gameObject.name));
        pointerHolder.transform.parent = transform;
        pointerHolder.transform.localPosition = Vector3.zero;

        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.name = string.Format("[{0}]WorldPointer_SimplePointer_Pointer", gameObject.name);
        pointer.transform.parent = pointerHolder.transform;

        //pointer.GetComponent<BoxCollider>().isTrigger = true;
        pointer.GetComponent<BoxCollider>().enabled = false;
        if (customPointerCursor == null)
        {
            pointerTip = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            pointerTip.transform.localScale = pointerTipScale;
        }
        //else
        //{
        //    Renderer renderer = customPointerCursor.GetComponentInChildren<MeshRenderer>();
        //    if (renderer)
        //    {
        //        customPointerMaterial = Material.Instantiate(renderer.sharedMaterial);
        //    }
        //    pointerTip = Instantiate(customPointerCursor);
        //    foreach (Renderer mr in pointerTip.GetComponentsInChildren<Renderer>())
        //    {
        //        mr.material = customPointerMaterial;
        //    }
        //}

        pointerTip.transform.name = string.Format("[{0}]WorldPointer_SimplePointer_PointerTip", gameObject.name);
        pointerTip.transform.parent = pointerHolder.transform;

       //pointerTip.GetComponent<Collider>().isTrigger = true;
        pointerTip.GetComponent<Collider>().enabled = false;

        SetPointerTransform(pointerLength, pointerThickness);//设置射线位置
        //TogglePointer(false);
    }
      void Update()
    {
        if (pointer.gameObject.activeSelf)
        {
            Ray pointerRaycast = new Ray(transform.position, transform.forward);
            RaycastHit pointerCollidedWith;
            var rayHit = Physics.Raycast(pointerRaycast, out pointerCollidedWith, pointerLength, ~layersToIgnore);
            //Debug.Log("PointerRay   name " + pointerCollidedWith.collider.name + "    length   " + pointerCollidedWith.distance);
            var pointerBeamLength = GetPointerBeamLength(rayHit, pointerCollidedWith);
            SetPointerTransform(pointerBeamLength, pointerThickness);
            //if (WaveVR_Controller.Input(type).GetPressDown(WVR_InputId.WVR_InputId_Alias1_Bumper))
            {//按下triger键
                RayClickedArgs args = new RayClickedArgs();
                args.target = detalhititem;
                args.device = type;
                args.hitpos = detalhitpos;
                //NotifyEvent(detalhititem.gameObject, "OnRayTiggerClicked", args);
            }
            if (WaveVR_Controller.Input(type).GetPressUp(WVR_InputId.WVR_InputId_Alias1_Touchpad))
            {
                RayClickedArgs args = new RayClickedArgs();
                args.target = detalhititem;
                args.hitpos = detalhitpos;
               // NotifyEvent(detalhititem.gameObject, "OnRayPadClicked", args);
            }
            if(previousTarget && previousTarget != detalhititem)
            {
                RayPointerArgs args = new RayPointerArgs();
                args.device = type;
                args.distance = detaldistance;
                args.target = previousTarget;
                NotifyEvent(previousTarget.gameObject, "OnRayPointerOut", args);
                previousTarget = null;
            }
            if(rayHit && previousTarget != detalhititem)
            {
                RayPointerArgs args = new RayPointerArgs();
                args.device = type;
                args.distance = detaldistance;
                previousTarget = detalhititem; 
                args.target = previousTarget;
                NotifyEvent(previousTarget.gameObject, "OnRayPointerIn", args);
                
            }
            if (!rayHit)
                previousTarget = null;
        }
    }

  
    void NotifyEvent(GameObject go, string funcName, object obj)
      {
          if (go == null || !go.activeSelf)
              return;
          go.SendMessage(funcName, obj, SendMessageOptions.DontRequireReceiver);
      }
      public Transform GetHitTrans()
      {
          return detalhititem;//碰撞的物体
      }
      public Vector3 GetHitPos()
      {
          return detalhitpos;//碰撞点
      }
      //public float GetHitDis()
      //{
      //    return detaldistance;
      //}
      void SetPointerMaterial()
    {
        //base.SetPointerMaterial();
        pointer.GetComponent<Renderer>().material = pointerMaterial;
        if (customPointerMaterial != null)
        {
            customPointerMaterial.color = pointerMaterial.color;
        }
        else
        {
            pointerTip.GetComponent<Renderer>().material = pointerMaterial;
        }
    }

    //protected override void TogglePointer(bool state)
    //{
    //    state = (pointerVisibility == pointerVisibilityStates.Always_On ? true : state);
    //    base.TogglePointer(state);
    //    pointer.gameObject.SetActive(state);

    //    var tipState = (showPointerTip ? state : false);
    //    pointerTip.gameObject.SetActive(tipState);

    //    if (pointer.GetComponent<Renderer>() && pointerVisibility == pointerVisibilityStates.Always_Off)
    //    {
    //        pointer.GetComponent<Renderer>().enabled = false;
    //    }
    //}

    private void SetPointerTransform(float setLength, float setThicknes)
    {//设置射线长度和大小
        //Debug.Log("SetPointerTransform     length   " + setLength );
        //if the additional decimal isn't added then the beam position glitches
        var beamPosition = setLength / (2 + 0.00001f);

        pointer.transform.localScale = new Vector3(setThicknes, setThicknes, setLength);
        pointer.transform.localPosition = new Vector3(0f, 0f, beamPosition);
        pointerTip.transform.localPosition = new Vector3(0f, 0f, setLength - (pointerTip.transform.localScale.z / 2));
        pointerHolder.transform.localRotation = Quaternion.identity;
    }

    private float GetPointerBeamLength(bool hasRayHit, RaycastHit collidedWith)
    {
        var actualLength = pointerLength;

        //reset if beam not hitting or hitting new target
        if (!hasRayHit || (pointerContactTarget && pointerContactTarget != collidedWith.transform))
        {
            if (pointerContactTarget != null)
            {
                //base.PointerOut();
            }

            pointerContactDistance = 0f;
            pointerContactTarget = null;
            destinationPosition = Vector3.zero;
            detalhititem = null;
            UpdatePointerMaterial(pointerMissColor);
        }

        //check if beam has hit a new target
        if (hasRayHit)
        {
            pointerContactDistance = collidedWith.distance;
            pointerContactTarget = collidedWith.transform;
            destinationPosition = pointerTip.transform.position;
            detalhititem = collidedWith.collider.transform;
            detalhitpos = collidedWith.point;
            detaldistance = collidedWith.distance;

            UpdatePointerMaterial(pointerHitColor);

            //base.PointerIn();
        }

        //adjust beam length if something is blocking it
        if (hasRayHit && pointerContactDistance < pointerLength)
        {
            actualLength = pointerContactDistance;
        }

        return actualLength;
    }

     void UpdatePointerMaterial(Color color)
    {
        //if (playAreaCursorCollided || !ValidDestination(pointerContactTarget, destinationPosition))
        //{
        //    color = pointerMissColor;
        //}
        pointerMaterial.color = color;
        ////if (!detalhititem.name.StartsWith("item_") && detalhititem.name.StartsWith("btn_"))
        //if (detalhititem.transform.parent.name == "modelroot")
        //    pointerMaterial.color = pointerModel;
        SetPointerMaterial();
    }
}