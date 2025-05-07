using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplePull : Grapple
{
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate(){
        if(Input.GetMouseButton(0)){
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
                applyMove();
                break;
                // Start Return
            case 3: 
                sendGrappleBack();
                break;

        }
    }


}
