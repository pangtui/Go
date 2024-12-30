using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public float walkSpeed = 2.0f;
    public float jumpVelocity = 3.0f;
    public float rollVelocity = 3.0f;
    public GameObject model;
    public PlayerInput pi;
    [SerializeField]
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 plannarVec;
    private Vector3 thrustVec;

    [SerializeField]
    private bool lockPlanar = false;

    void Awake(){
        pi = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()//Time.deltaTime
    {
        // float targetRunMulti = ((pi.run)?2.0f:1.0f);
        // merge test3
        // merge test1
        anim.SetFloat("forward", pi.Dmag * Mathf.Lerp(anim.GetFloat("forward"), ((pi.run)?2.0f:1.0f), 0.5f));
        // if ( rigid.velocity.magnitude > 0.05f ){ //速度求模
        //     anim.SetTrigger("roll");
        // }
        if (pi.jump){
            anim.SetTrigger("jump");
        }
        if (pi.Dmag > 0.1f)
        {
            model.transform.forward = Vector3.Slerp(model.transform.forward, pi.Dvec, 0.5f);
        }
        if(lockPlanar == false){
            plannarVec = pi.Dmag * model.transform.forward * walkSpeed * ((pi.run)?2.0f:1.0f);
        }
    }
    void FixedUpdate(){
        rigid.velocity = new Vector3(plannarVec.x, rigid.velocity.y, plannarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }
    //Message precessing block
    public void OnJumpEnter(){
        // print("on jump enter!");
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
    }
    public void OnRollEnter(){
        pi.inputEnabled = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, rollVelocity, 0);
    }
    public void OnFallEnter(){
        pi.inputEnabled = false;
        lockPlanar = true;
    }
    public void OnGroundEnter(){
        pi.inputEnabled = true;
        lockPlanar = false;
    }
    public void OnJabEnter(){
        pi.inputEnabled = false;
        lockPlanar = true;
    }
    public void OnJabUpdate(){
        thrustVec = model.transform.forward * anim.GetFloat("jabVelocity");
    }
    public void IsGround(){
        // print("IsGround!");
        anim.SetBool("isGround", true);
    }
    public void IsNotGround(){
        // print("IsNotGround!");
        anim.SetBool("isGround", false);
    }
}
