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


    void Start(){
        speedBoost = 10;
        length = 20;
        grappleSpeed = 25;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        addNewSuccess = true;
        rb = GetComponent<Rigidbody2D>();
        successBoost = 20;
        resetStates();
    }

    void FixedUpdate(){
        if(Input.GetMouseButton(0) && !isCast){
            keepHeadInPlace(false);
            cast();
        }
        else if(addMove){
            applyMove();
        }
        else if(startReturn){
            sendGrappleBack();
        }
        else if (isCast){
            sendHead();
            //resetStates();
        }

        // else if(startReturn){
        //     sendGrappleBack();
        // }
        // else if(isCast){
        //     sendHead();
        // }
        // if(addMove){
        //     applyMove();
        // }

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
        
        GetComponent<BoxCollider2D>().enabled = true;
        isCast = true;
        }
    }

    
    private void sendHead(){
        // back and -dir since i drew the head upside down
        totalMoved += grappleSpeed * Time.deltaTime;
        Vector2 translation = transform.up  * grappleSpeed;
        rb.AddForce(translation, ForceMode2D.Force);
        if(totalMoved > length){
            startReturn = true;
        }
    }


// can i make a general function for moving and when the it reaches the end make the speed *-1?
    private void sendGrappleBack(){
        // find the camera position to return the grapple to
        Vector2 translation = transform.up  * grappleSpeed * -1;
        rb.AddForce(translation);
    }

    public void resetStates(){
        foreach(Transform child in transform){
            child.gameObject.SetActive(false);
        }
        // set the transform back to 0,0,1
        transform.position = Vector3.forward;
        isAttached = false;
        startReturn = false;
        isCast = false;
        addMove = false;
        totalMoved = 0;
        totalPlayerDisplacement = 0;
        totalGrapplerDisplacement = 0;
    }

    void calculateMoveDirection(){
            // if the grapple is too close dont change the direction
        // moveDir = Vector2.zero;
        // if((transform.position - transform.parent.position).magnitude > .1){
            moveDir = transform.position - transform.parent.position;
            addMove = true;
            moveDir.Normalize();
        // }
    }

    private void applyMove(){
        Rigidbody2D playerRb = transform.parent.gameObject.GetComponent<Rigidbody2D>();

        if(isAttached){
            if(totalGrapplerDisplacement == 0){
                originalPlayerPos = transform.parent.position;
                totalGrapplerDisplacement = (transform.parent.position - transform.position).magnitude;
            }
            totalPlayerDisplacement = (transform.parent.position - originalPlayerPos).magnitude;
            playerRb.AddForce(moveDir * currSpeed, ForceMode2D.Impulse);
            Debug.Log("total grapple moved: " + totalMoved);
            Debug.Log("player moved" + totalPlayerDisplacement);
// if displacement of player is geater than displacement of grappler
            if(totalPlayerDisplacement >= totalGrapplerDisplacement){
                resetStates();
            }
        }
        else{
            successCounter = 0;
            addMove = false;
        }
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
        if(collider.gameObject.layer == LayerMask.NameToLayer("Floor") || collider.tag == "Light"){
            calculateMoveDirection();
            keepHeadInPlace(true);
            GetComponent<BoxCollider2D>().enabled = false;
            isAttached = true;
            addMove = true;
            if(addNewSuccess){
                // currSpeed = successCounter * successBoost + speedBoost;
                currSpeed = speedBoost;
                addNewSuccess = false;
                successCounter++;
            }

        }
        else if(collider.tag == "Player" && startReturn){
            resetStates();
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
}