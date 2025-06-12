using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamOffset : MonoBehaviour
{
    float offset;
    Inputs inputs;
    CamFollow camFollow;
    Vector2 camOffset;
    Transform finalPos;
    bool[] inputsPressed;
    Transform originalTarget;
    public Transform newTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<Inputs>();
        camFollow = GetComponent<CamFollow>();
        offset = 5f;
        finalPos = newTarget;
        originalTarget = camFollow.getTarget();
        inputsPressed = new bool[4];
    }

    //check for input
    // record original position if null
    // add offset to original position

    // if no key is being pressed, return the pos to original pos
    void Update()
    {
        camOffset = Vector2.zero;
        resetDirPressed();
        if(inputs.getKeyDown(KeyCode.DownArrow)){
            dirPressed(new Vector2(0,-offset), 0);
        }
        if(inputs.getKeyDown(KeyCode.UpArrow)){
            dirPressed(new Vector2(0,offset), 1);
        }
        if(inputs.getKeyDown(KeyCode.RightArrow)){
            dirPressed(new Vector2(offset,0), 2);
        }
        if(inputs.getKeyDown(KeyCode.LeftArrow)){
            dirPressed(new Vector2(-offset,0), 3);

        }
        if(!isADirPressed()){
            camFollow.setTarget(originalTarget);
            newTarget.position = Vector3.zero;
            return;
        }
        newTarget.position = new Vector3(camOffset.x, camOffset.y, -1);
        newTarget.position += originalTarget.position;
        camFollow.setTarget(newTarget);
    }


    void dirPressed(Vector2 offset, int index){
        if(newTarget.position == Vector3.zero){
            originalTarget = camFollow.getTarget();
        }
        if(!inputsPressed[index]){
            camOffset += offset;
            inputsPressed[index] = true;
        } 
    }

    bool isADirPressed(){
        foreach(bool pressed in inputsPressed){
            if(pressed){
                return true;
            }
        }
        return false;
    }

    void resetDirPressed(){
        for(int i = 0; i < inputsPressed.Length; i++){
            inputsPressed[i] = false;
        }
    }

    
}
