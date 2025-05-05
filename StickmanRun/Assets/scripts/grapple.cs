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
    float successBoost;
    bool addNewSuccess;
    Vector3 currMove;
    float currSpeed;
    Rigidbody2D rb;
    Vector2 moveDir;
    bool addMove;
    Vector3 lastPos; 
    float totalPlayerDisplacement;
    float totalGrapplerDisplacement;
    Vector3 originalPlayerPos;
    int state; // current state of the grappler


    void Start(){
        speedBoost = 300;
        length = 7;
        grappleSpeed = 50;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        addNewSuccess = true;
        rb = GetComponent<Rigidbody2D>();
        successBoost = 20;
        resetStates();
    }

    void FixedUpdate(){



        // while the player holds down left click do normal grappler.
        // else send grapple back or reset it. 

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
                // idle
            // default:
            //     resetStates();
            //     break;

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
        // back and -dir since i drew the head upside down

        totalMoved = (transform.position - transform.parent.position).magnitude;
        Vector2 translation = transform.up  * grappleSpeed;
        rb.AddForce(translation, ForceMode2D.Force);
        if(totalMoved > length){
            state = 3;
        }
    }


// can i make a general function for moving and when the it reaches the end make the speed *-1?
    private void sendGrappleBack(){
        // find the camera position to return the grapple to
        dir = transform.position - transform.parent.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        Vector2 translation = transform.up  * grappleSpeed * -1;
        rb.AddForce(translation);
    }

    public void resetStates(){

        // set the transform back to 0,0,1

        // set state to idle/default
        state = 0;
        totalMoved = 0;
        totalPlayerDisplacement = 0;
        totalGrapplerDisplacement = 0;
        rb.velocity = Vector2.zero;
        // transform.position = Vector3.forward;
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
    }

    void calculateMoveDirection(){
            // if the grapple is too close dont change the direction
        // moveDir = Vector2.zero;
        // if((transform.position - transform.parent.position).magnitude > .1){
            moveDir = transform.position - transform.parent.position;
            moveDir.Normalize();
            addMove = true;
        // }
    }

    private void applyMove(){
        calculateMoveDirection();
        Rigidbody2D playerRb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        if(totalGrapplerDisplacement == 0){
            originalPlayerPos = transform.parent.position;
            totalGrapplerDisplacement = (transform.parent.position - transform.position).magnitude;
        }
        totalPlayerDisplacement = (transform.parent.position - originalPlayerPos).magnitude;
        playerRb.AddForce(moveDir * currSpeed, ForceMode2D.Force);
// if displacement of player is geater than displacement of grappler
        // if(totalPlayerDisplacement >= totalGrapplerDisplacement){
        //     resetStates();
        // }
    }


    private void drawLine(){
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.parent.position);
        lineRenderer.SetPosition(1, transform.position);
        
        // transform.rotation = Quaternion.LookRotation(Vector3.forward, transform.position - transform.parent.position);
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
            if(addNewSuccess){
                // currSpeed = successCounter * successBoost + speedBoost;
                currSpeed = speedBoost;
                addNewSuccess = false;
                successCounter++;
            }

        }


    }

    void OnTriggerExit2D(Collider2D collider){

        if(collider.gameObject.layer == LayerMask.NameToLayer("Floor") || collider.tag == "Light"){
            addNewSuccess = true;
            isAttached = false;
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