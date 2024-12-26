using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGroundSensor : MonoBehaviour
{
    public CapsuleCollider capcol;
    private Vector3 point1;
    private Vector3 point2;
    private float radius;
    private float offset = 0.1f;
    void Awake()
    {
        radius = capcol.radius - 0.05f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        point1 = transform.position + transform.up*(radius - offset);
        point2 = transform.position + transform.up*(capcol.height - offset) - transform.up*radius;

        Collider[] collider = Physics.OverlapCapsule(point1, point2, radius, LayerMask.GetMask("Ground"));
        if ( collider.Length > 0 ){
            // foreach (var item in collider){
            //     print( "collision:" + item.name );
            // }
            SendMessageUpwards("IsGround");
            
        }else{
            SendMessageUpwards("IsNotGround");
        }
    }
}
