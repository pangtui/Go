using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("====== Key settings =====")]
    public string keyUp = "w";
    public string keyDown = "s";
    public string keyLeft = "a";
    public string keyRight = "d";
    public string keyA;
    public string keyB ;
    public string keyC;
    public string keyD;
    public string keyJUp = "up";
    public string keyJDown = "down";
    public string keyJLeft = "left";
    public string keyJRight = "right";

    public float iMouseHorInput;
    public float iMouseVerInput;
    public float iMouseScrollInput;

    [Header("====== Output signals =====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    public bool run;
    public bool jump;
    private bool lastJump;
    public float Jup;
    public float Jright;

    [Header("====== Others =====")]
    public bool inputEnabled = true;
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;

    private float targetMouseHor;
    private float targetMouseVer;
    private float mouseHorDelta;
    private float mouseVerDelta;
    private float iTargetMouseScroll;
    private float iMouseScrollDelta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //Get joystick input movement
        Jup = (Input.GetKey(keyJUp)?1.0f:0) - (Input.GetKey(keyJDown)?1.0f:0);
        Jright = (Input.GetKey(keyJRight)?1.0f:0) - (Input.GetKey(keyJLeft)?1.0f:0);
        //Get mouse input movement
        if (Input.GetMouseButton(1))
        {
            targetMouseHor = Input.GetAxis("Mouse X"); 
            targetMouseVer = Input.GetAxis("Mouse Y");
            // print("Mouse X: " + iMouseHorInput + " Mouse Y: " + iMouseVerInput);
        }else{
            targetMouseHor = 0;
            targetMouseVer = 0;
        }
        iMouseHorInput = Mathf.SmoothDamp(iMouseHorInput, targetMouseHor, ref mouseHorDelta, 0.1f);
        iMouseVerInput = Mathf.SmoothDamp(iMouseVerInput, targetMouseVer, ref mouseVerDelta, 0.1f);

        iMouseScrollInput = Input.GetAxis("Mouse ScrollWheel");
        // iMouseScrollInput = Mathf.SmoothDamp(iMouseScrollInput, iTargetMouseScroll, ref iMouseScrollDelta, 0.1f);
        if (iMouseScrollInput != 0){
            print("" + iMouseScrollInput);
        }


        // Get keyboard input movement
        targetDup = (Input.GetKey(keyUp)?1.0f:0) - (Input.GetKey(keyDown)?1.0f:0);
        targetDright = (Input.GetKey(keyRight)?1.0f:0) - (Input.GetKey(keyLeft)?1.0f:0);
        if(inputEnabled == false) {
            targetDup = 0;
            targetDright = 0;
        }
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);
        
        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;
        Dmag = Mathf.Sqrt((Dup2 * Dup2) + (Dright2 * Dright2));
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;
        if(keyA != null)
        {
            run = Input.GetKey(keyA);
        }

        bool newJump = Input.GetKey(keyB);
        if (newJump != lastJump && newJump == true){
            jump = true;
        }else{
            jump = false;
        }
        lastJump = newJump;
    }

    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1-(input.y * input.y)/2.0f);
        output.y = input.y * Mathf.Sqrt(1-(input.x * input.x)/2.0f);
        return output;
    }
}
