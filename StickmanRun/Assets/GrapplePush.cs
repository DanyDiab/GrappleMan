using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePush : Grapple
{

    Rigidbody2D pullingRB;
    float pullingSpeed;
    // Start is called before the first frame update

    // Update is called once per frame
    void Start()
    {
        pullingSpeed = 50;
        base.Start();
    }
    void FixedUpdate(){
        if(Input.GetMouseButton(1)){
            if(state == 0){
                keepHeadInPlace(false);
                cast();
            }
        }
        else if(state != 0){
            if(state == 2){
                resetStates(); 
                return;              
            }
            state = 3;
        }

        switch(state){
            // isCast;
            case 1:
                sendHead();
                break;
                // addMove
            case 2:
                
                break;
                // Start Return
            case 3: 
                sendGrappleBack();
                break;

        }
    }


    private void pullObject(){
        // pull the object towards the player
        Vector3 moveDir = transform.position - transform.parent.position;
        moveDir.Normalize();
        pullingRB.AddForce(pullingSpeed * moveDir);
    }


    // void OnCollisionStay2D(Collision2D collision)
    // {
    //    if(collision.collider.tag == "pull"){
    //         pullingRB = collision.collider.attachedRigidbody;
    //         pullObject();
    //    } 
    // }

}