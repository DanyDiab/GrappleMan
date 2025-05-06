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
    public event Action<float> OnAttach;

    LineRenderer lineRenderer;
    int successCounter;
    public float speedBoost;
    Vector3 currMove;
    float currSpeed;
    Rigidbody2D rb;
    Vector2 moveDir;
    bool addMove;
    float totalGrapplerDisplacement;
    Vector3 originalPlayerPos;
    int state; // current state of the grappler


    void Start(){
        speedBoost = 300;
        length = 7;
        grappleSpeed = 75;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        resetStates();
    }

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

    
    private void sendHead(){

        totalMoved = (transform.position - transform.parent.position).magnitude;
        Vector2 translation = transform.up  * grappleSpeed;
        rb.AddForce(translation, ForceMode2D.Force);
        if(totalMoved > length){
            state = 3;
        }
    }


// can i make a general function for moving and when the it reaches the end make the speed *-1?
    private void sendGrappleBack(){
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

    private void applyMove(){
        calculateMoveDirection();
        Rigidbody2D playerRb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        if(totalGrapplerDisplacement == 0){
            originalPlayerPos = transform.parent.position;
            totalGrapplerDisplacement = (transform.parent.position - transform.position).magnitude;
        }
        playerRb.AddForce(moveDir * currSpeed, ForceMode2D.Force);
    }


    private void drawLine(){
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.parent.position);
        lineRenderer.SetPosition(1, transform.position);
    }


    private void keepHeadInPlace(bool apply){
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

    public void resetSuccessCounter(){
        successCounter = 0;
    }
    public int getState(){
        return state;
    }
}