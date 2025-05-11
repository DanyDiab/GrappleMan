using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    float distance;

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
    bool reverseDir;
    Pullable pullObject;
    
    bool attachJoint;
    float swingSpeed;
    HingeJoint2D hinge;


    protected void Start(){
        currState = grapplerState.Idle;
        speedBoost = 25;
        length = 20;
        grappleSpeed = 25;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        resetStates();
        reverseDir = false;
        attachJoint = true;
        swingSpeed = 10;
        hinge = transform.parent.gameObject.GetComponent<HingeJoint2D>();
    }


    // e = cast grappler.
    // holding left click = pull player in 
    // holding right click push player out  or pull object towards player
    // holding both = no push or pull allwing player to swing\



    // add force to move player to emulat emoving faster after a longer time
    // 

    void Update()
    {
        // Debug.Log(currState);
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
                moveGrappler(true);
                if(distance > length){
                     currState = grapplerState.Retracting;
                }
                break;
            case grapplerState.Attached:
                keepHeadInPlace(true);
                if(leftClick){
                    hinge.enabled = false;
                    currState = grapplerState.PullingPlayer;
                }
                else if(rightClick){
                    hinge.enabled = false;
                    currState = grapplerState.PullingObject;
                }
                else if(ePressed){
                    hinge.enabled = false;
                    currState = grapplerState.Retracting;
                }
                break;
            case grapplerState.Retracting:
                retractGrappler();
                break;
            case grapplerState.PullingPlayer:
                if(!leftClick) currState = grapplerState.Attached;
                calculateMoveDirection();
                applyMove();
                break;
            case grapplerState.PullingObject:
                // pulling function
                if(pullObject != null){
                    // pull object
                    pullObject.getRb().velocity = rb.velocity;
                    pullObject.setIsPulled(true);
                    retractGrappler();
                    break;
                }
                
                if(!rightClick) currState = grapplerState.Attached;
                calculateMoveDirection();
                moveDir *= -1;
                applyMove();      
                distance = Vector2.Distance(transform.parent.position, transform.position);
                if(distance > length){
                     currState = grapplerState.Retracting;
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
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

        foreach(Transform child in transform){
            child.gameObject.SetActive(true);
        }
        GetComponent<BoxCollider2D>().enabled = true;
    }

    
    protected void moveGrappler(bool forward){
        if(!forward){
            dir = (transform.parent.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if(!reverseDir){
                transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
                reverseDir = true;
            }
        }
        rb.velocity = dir * grappleSpeed;
        distance = Vector2.Distance(transform.position, transform.parent.position);
        // Debug.Log(distance);
    }

    public void resetStates(){
        // set state to idle/default
        pullObject = null;
        distance = 0;
        reverseDir = false;
        attachJoint = true;
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
        Rigidbody2D playerRb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        playerRb.AddForce(speedBoost * moveDir, ForceMode2D.Force);
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
    void retractGrappler(){
        keepHeadInPlace(false);
        moveGrappler(false);
        if(distance <= 0.5f){
            currState = grapplerState.Idle;
            if(pullObject != null){
                pullObject.setIsPulled(false);
            }
        }
        return;
    }

    void OnTriggerStay2D(Collider2D collider){
        // Debug.Log(collider.tag);
        if(collider.gameObject.layer == LayerMask.NameToLayer("Floor") || collider.tag == "Light" || collider.tag == "Pull"){
            keepHeadInPlace(true);
            GetComponent<BoxCollider2D>().enabled = false;
            currState = grapplerState.Attached;
            currSpeed = speedBoost;
        }
        if(collider.tag == "Pull"){
            pullObject = collider.GetComponent<Pullable>();
        }
        
    }
    
    public Vector2 getDir(){
        return dir;
    }
    public grapplerState getState(){
        return currState;
    }
}