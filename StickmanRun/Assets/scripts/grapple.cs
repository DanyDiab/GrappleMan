using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour{

    // how far the grappler can reach
    float length;
    Vector2 dir;
    Vector2 attachPos;
    float grappleSpeed;

    float totalMoved;
    bool isCast;

    bool startReturn;
    bool isAttached;

    protected LineRenderer lineRenderer;
    public float speedBoost;
    float currSpeed;
    protected Rigidbody2D rb;
    Vector2 moveDir;
    bool addMove;
    float totalGrapplerDisplacement;
    protected int state; // current state of the grappler
    static bool didDrawLine;


    protected void Start(){
        speedBoost = 300;
        length = 7;
        grappleSpeed = 75;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        resetStates();
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
       state = 1;
    }

    
    protected void sendHead(){

        totalMoved = (transform.position - transform.parent.position).magnitude;
        Vector2 translation = transform.up  * grappleSpeed;
        rb.AddForce(translation, ForceMode2D.Force);
        if(totalMoved > length){
            state = 3;
        }
    }


// can i make a general function for moving and when the it reaches the end make the speed *-1?
    protected void sendGrappleBack(){
        // find the parent position to return the grapple to
        dir = transform.position - transform.parent.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        Vector2 translation = transform.up  * grappleSpeed * -1;
        rb.AddForce(translation);
    }

    public void resetStates(){
        // set state to idle/default
        state = 0;
        totalMoved = 0;
        rb.velocity = Vector2.zero;
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
    }

    void calculateMoveDirection(){
            moveDir = transform.position - transform.parent.position;
            moveDir.Normalize();
            addMove = true;
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
        if(collider.tag == "Player" && (state == 2 || state == 3)){
            resetStates();
            return;
        }
        if(collider.gameObject.layer == LayerMask.NameToLayer("Floor") || collider.tag == "Light"){
            keepHeadInPlace(true);
            GetComponent<BoxCollider2D>().enabled = false;
            isAttached = true;
            state = 2;
            currSpeed = speedBoost;

        }


    }

    public bool getIsAttached(){
        return isAttached;
    }
    
    public Vector2 getDir(){
        return dir;
    }
    public int getState(){
        return state;
    }
}