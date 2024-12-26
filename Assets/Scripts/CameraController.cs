using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public PlayerInput pi;
    private GameObject playerHandle;
    private GameObject cameraHandle;
    public float horizontalSpeed = 100.0f;
    public float verticalSpeed = 100.0f;
    public float horizontalSpeedMouse = 100.0f;
    public float verticalSpeedMouse = 100.0f;
    //相机缩放范围
    public float iCameraScaleZMax = -0.3f;
    public float iCameraScaleZMin = -10.0f;
    public float iCameraScaleYMax = 4.0f;
    public float iCameraScaleYMin = -0.5f;
    private float iCameraScaleYScope;
    private float iCameraScaleZScope;
    public float iCameraScaleY;
    public float iCameraScaleZ;

    private float tempEulerX;
    private GameObject model;
    private GameObject camera;

    private Vector3 cameraDampVelocity;

    void Awake()
    {
        iCameraScaleYScope = iCameraScaleYMax - iCameraScaleYMin;
        iCameraScaleZScope = iCameraScaleZMax - iCameraScaleZMin;
        cameraHandle = transform.parent.gameObject;
        playerHandle = cameraHandle.transform.parent.gameObject;
        tempEulerX = 0;
        model = playerHandle.GetComponent<ActorController>().model;
        camera = Camera.main.gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        // playerHandle.transform.Rotate(Vector3.up, pi.Dright * 10.0f * Time.deltaTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        Vector3 tempModelEuler = model.transform.eulerAngles;

        if (pi.iMouseVerInput != 0 || pi.iMouseHorInput != 0)
        {
            playerHandle.transform.Rotate(Vector3.up, pi.iMouseHorInput * horizontalSpeedMouse * Time.deltaTime);
            tempEulerX -= pi.iMouseVerInput * verticalSpeedMouse * Time.deltaTime;
        }else{
            playerHandle.transform.Rotate(Vector3.up, pi.Jright * horizontalSpeed * Time.deltaTime);
            tempEulerX -= pi.Jup * verticalSpeed * Time.deltaTime;
        }

        if (pi.iMouseScrollInput != 0)
        {
            iCameraScaleY = cameraHandle.transform.localPosition.y + pi.iMouseScrollInput * iCameraScaleYScope;
            iCameraScaleZ = cameraHandle.transform.localPosition.z - pi.iMouseScrollInput * iCameraScaleZScope;
            iCameraScaleY = Mathf.Min(Mathf.Max(iCameraScaleY, iCameraScaleYMin), iCameraScaleYMax);
            iCameraScaleZ = Mathf.Min(Mathf.Max(iCameraScaleZ, iCameraScaleZMin), iCameraScaleZMax);
            
            cameraHandle.transform.localPosition = new Vector3(
                cameraHandle.transform.localPosition.x,
                iCameraScaleY,
                iCameraScaleZ
            );
        }

        cameraHandle.transform.localEulerAngles = new Vector3(
            Mathf.Clamp(tempEulerX, -40, 30),
            0,0);

        model.transform.eulerAngles = tempModelEuler;

        // camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position, 0.2f);
        camera.transform.position = Vector3.SmoothDamp(camera.transform.position, transform.position, ref cameraDampVelocity, 0.05f);
        camera.transform.eulerAngles = transform.eulerAngles;
    }
}
