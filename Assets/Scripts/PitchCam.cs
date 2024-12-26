using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 俯仰摄像机
/// </summary>
public class PitchCam : MonoBehaviour
{
    /// <summary>
    /// 水平输入
    /// </summary>
    public float horInput;
    /// <summary>
    /// 垂直输入
    /// </summary>
    public float verInput;

    /// <summary>
    /// 反转X轴
    /// </summary>
    public bool reversalX = true;
    /// <summary>
    /// 反转Y轴
    /// </summary>
    public bool reversalY = true;

    /// <summary>
    /// 仰角限制
    /// </summary>
    public float upLimite = 30f;
    /// <summary>
    /// 俯角限制
    /// </summary>
    public float downLimite = 30f;
    /// <summary>
    /// 左侧限制
    /// </summary>
    public float leftLimit = 0f;
    /// <summary>
    /// 右侧限制
    /// </summary>
    public float rightLImit = 0f;

    /// <summary>
    /// 旋转的速度
    /// </summary>
    public float rotateSpeed = 10f;
    /// <summary>
    /// 矫正系数
    /// </summary>
    public float ratio = 100;
    /// <summary>
    /// 是否可以旋转
    /// </summary>
    public bool canRotate = true;

    /// <summary>
    /// 是否可以移动
    /// </summary>
    public bool canMove = false;
    //基础移动速度
    public float moveSpeed = 5f;
    /// <summary>
    /// 移动速度的乘数 ，范围为1-5的整数
    /// </summary>
    public float speedIntensity = 1;
    private float hor = 0;
    private float ver = 0;
    private float scroll;

    private Rigidbody rig;

    private void Awake()
    {
        rig = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        //在编辑器windows上
#if UNITY_EDITOR_WIN || UNITY_STANDALONE||UNITY_WEBGL
        CheckPointOnUI_StandALONE();
        //在安卓
#elif UNITY_ANDROID
            CheckPointOnUI_Android();
#endif
        //俯仰摄像机
        PicthCamera();
        //获取方向键
        GetArrow();
        //设置移动速度
        SetMoveSpeed();
    }
    private void FixedUpdate()
    {
        //移动相机
        MoveGo();
    }

    /// <summary>
    /// 检测鼠标是否在UI上进行了点击，如果在，不能转动相机 编辑器和独立平台
    /// </summary>
    private void CheckPointOnUI_StandALONE()
    {
        if (EventSystem.current == null) return;
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                canRotate = false;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonUp(0))
            {
                canRotate = true;
            }
        }
    }
    /// <summary>
    /// 检测鼠标是否在UI上进行了点击，如果在，不能转动相机 安卓
    /// </summary>
    private void CheckPointOnUI_Android()
    {
        if (EventSystem.current == null) return;
        if (Input.touchCount > 0)//手指按下并且触点大于0
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))//在UI上
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    canRotate = false;
                }
            }
            else
            {
                if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    canRotate = true;
                }
            }
        }
    }

    /// <summary>
    /// 俯仰摄像机
    /// </summary>
    private void PicthCamera()
    {

        if (Input.GetMouseButton(0) && canRotate)
        {
            horInput = Input.GetAxis("Mouse X"); 
            verInput = Input.GetAxis("Mouse Y");
            //print("horInput " + horInput);

            if (reversalX)
            {
                horInput = -horInput;
            }
            if (reversalY)
            {
                verInput = -verInput;
            }

            if (horInput != 0 || verInput != 0)
            {
                transform.localEulerAngles += new Vector3(verInput * rotateSpeed * Time.deltaTime * ratio, -horInput * rotateSpeed * Time.deltaTime * ratio, 0);

                //限制仰角
                if (transform.localEulerAngles.x > 180 && transform.localEulerAngles.x < 360 - upLimite)
                {
                    transform.localEulerAngles = new Vector3(360 - upLimite, transform.localEulerAngles.y, 0);
                }
                //限制俯角
                if (transform.localEulerAngles.x < 180 && transform.localEulerAngles.x > downLimite)
                {
                    transform.localEulerAngles = new Vector3(downLimite, transform.localEulerAngles.y, 0);
                }
                //限制左侧
                if (leftLimit != 0 && transform.localEulerAngles.y > 180 && transform.localEulerAngles.y < 360 - leftLimit)
                    if (leftLimit != 0)
                    {
                        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 360 - leftLimit, 0);
                    }
                //限制右侧
                if (rightLImit != 0 && transform.localEulerAngles.y < 180 && transform.localEulerAngles.y > rightLImit)
                {
                    transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, rightLImit, 0);
                }
            }
        }
    }

    /// <summary>
    /// 获取方向键
    /// </summary>
    private void GetArrow()
    {
        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");
    }
    /// <summary>
    /// 移动摄像机
    /// </summary>
    private void MoveGo()
    {
        if (!canMove) return;
        print("移动中");
        transform.Translate(new Vector3(hor, 0, ver) * moveSpeed * speedIntensity * Time.deltaTime, Space.Self);
        rig.velocity = Vector3.zero;
    }
    /// <summary>
    /// 通过滚轮设置相机的移动速度
    /// </summary>
    private void SetMoveSpeed()
    {
        scroll = Input.GetAxis("Mouse ScrollWheel");
        //如果滚轮没动，返回
        if (scroll == 0) return;
        if (scroll > 0)
        {
            speedIntensity += 1;
            if (speedIntensity > 7) speedIntensity = 7;
        }
        else
        {
            speedIntensity -= 1;
            if (speedIntensity < 1) speedIntensity = 1;
        }
    }
}





