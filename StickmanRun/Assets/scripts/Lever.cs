using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum LeverState{
    NotActive,
    Off,
    On,
}
public class Lever : MonoBehaviour
{
    LeverState currState;
    SpriteRenderer spriteRenderer;
    public Sprite notActiveSprite;
    public Sprite activeSprite;
    // variable to keep track of when we initalliy change states to set run intial methods 
    bool initSwapState;

    void Start()
    {
        initSwapState = false;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currState = LeverState.NotActive;
    }


    void Update()
    {
        switch(currState){
            // wait for the stick to be attacghed to set the state to off
            case LeverState.NotActive:
                break;
            // 
            case LeverState.Off:
                spriteRenderer.sprite = activeSprite;
                break;
            case LeverState.On:
                spriteRenderer.flipY = true;
                break;
        }
    }

}