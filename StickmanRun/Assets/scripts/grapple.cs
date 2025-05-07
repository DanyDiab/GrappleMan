using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum grapplerState {
    Idle,
    Casting,
    Attached,
    PullingPlayer,
    PullingObject,
    Swinging,
    Retracting
}

public class Grapple : MonoBehaviour{

    // how far the grappler can reach
    float length;
    Vector2 dir;
    Vector2 attachPos;
    float grappleSpeed;

    float totalMoved;

    protected LineRenderer lineRenderer;
    public float speedBoost;
    float currSpeed;
    protected Rigidbody2D rb;
    Vector2 moveDir;
    bool addMove;
    float totalGrapplerDisplacement;
    protected int state; // current state of the grappler
    static bool didDrawLine;
    bool leftClick;
    bool rightClick;
    bool ePressed;
    grapplerState currState;


    protected void Start(){
        currState = grapplerState.Idle;
        speedBoost = 50;
        length = 15;
        grappleSpeed = 25;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        resetStates();
    }


    // e = cast grappler.
    // holding left click = pull player in 
    // holding right click push player out  or pull object towards player
    // holding both = no push or pull allwing player to swing

    void Update()
    {
        Debug.Log(currState);
        ePressed = Input.GetKey(KeyCode.E);
        leftClick = Input.GetMouseButton(0);
        rightClick = Input.GetMouseButton(1);
        switch(currState){
            case grapplerState.Idle:
                resetStates();
                if(ePressed){
                    cast();
                    currState = grapplerState.Casting;
                }
                break;
            case grapplerState.Casting:
                sendHead();
                if(totalMoved > length){
                     currState = grapplerState.Retracting;
                }
                break;
            case grapplerState.Attached:

                if(leftClick){
                    currState = grapplerState.PullingPlayer;
                }
                else if(rightClick){
                    currState = grapplerState.PullingObject;
                }
                else if(ePressed){
                    currState = grapplerState.Retracting;
                }
                break;
            case grapplerState.PullingPlayer:
                if(!leftClick) currState = grapplerState.Attached;
                calculateMoveDirection();
                applyMove();
                break;
            case grapplerState.PullingObject:
                if(!rightClick) currState = grapplerState.Attached;
                // pulling function
                break;
            case grapplerState.Swinging:
                break;
            case grapplerState.Retracting:
                keepHeadInPlace(false);
                sendGrappleBack();
                if((transform.position - transform.parent.position).magnitude <= 0.3f){
                     currState = grapplerState.Idle;
                }
                break;
        }
    }

    void LateUpdate()
    {
        drawLine();
    }

    public void cast(){
        Vector3 startPos = transform.parent.position;
        transform.position = startPos;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        dir = worldMousePos - transform.position;
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        foreach(Transform child in transform){
            child.gameObject.SetActive(true);
        }
        GetComponent<BoxCollider2D>().enabled = true;
    }

    
    protected void sendHead(){

        totalMoved = (transform.position - transform.parent.position).magnitude;
        Vector2 translation = transform.up  * grappleSpeed;
        rb.AddForce(translation, ForceMode2D.Force);

    }


// can i make a general function for moving and when the it reaches the end make the speed *-1?
    protected void sendGrappleBack(){
        // find the parent position to return the grapple to
        dir = transform.position - transform.parent.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        Vector2 translation = transform.up  * grappleSpeed * -1;
        rb.velocity = translation;
        totalMoved -= grappleSpeed * Time.deltaTime;
    }

    public void resetStates(){
        // set state to idle/default
        totalMoved = 0;
        rb.velocity = Vector2.zero;
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
    }

    void calculateMoveDirection(){
            moveDir = transform.position - transform.parent.position;
            moveDir.Normalize();
    }

    protected void applyMove(){
        calculateMoveDirection();
        Rigidbody2D playerRb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        playerRb.AddForce(moveDir * currSpeed, ForceMode2D.Force);
    }


    protected void drawLine(){
        if(lineRenderer == null) return;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.parent.position);
        lineRenderer.SetPosition(1, transform.position);
    }


    protected void keepHeadInPlace(bool apply){
        if(apply){
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
            return;
        }
        rb.constraints = RigidbodyConstraints2D.None;


    }
    void OnTriggerStay2D(Collider2D collider){
        // Debug.Log(collider.tag);
        if(collider.gameObject.layer == LayerMask.NameToLayer("Floor") || collider.tag == "Light"){
            keepHeadInPlace(true);
            GetComponent<BoxCollider2D>().enabled = false;
            currState = grapplerState.Attached;
            currSpeed = speedBoost;
        }
    }
    
    public Vector2 getDir(){
        return dir;
    }
    public grapplerState getState(){
        return currState;
    }
}