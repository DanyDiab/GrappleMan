using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour{

    // how far the grappler can reach
    float length;
    Vector3 dir;

    Vector3 attachPos;
    float grappleSpeed;

    float totalMoved;
    bool isCast;

    bool startReturn;
    bool isAttached;
    public event Action<float> OnAttach;

    LineRenderer lineRenderer;
    int successCounter;
    float speedBoost;
    bool addNewSuccess;


    void Start(){
        speedBoost = 15;
        length = 15;
        grappleSpeed = 25;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        addNewSuccess = true;
    }

    void FixedUpdate(){
        if(Input.GetMouseButton(0) && !isCast){
            cast();
        }

        else if(isAttached){
            resetStates();
            keepHeadInPlace();
        }
        else if(startReturn){
            sendGrappleBack();
        }
        else if(isCast){
            sendHead();
        }
        drawLine();
    }
    


    public void cast(){
        Vector3 startPos = transform.parent.position;
        Debug.Log(transform.parent.gameObject.tag);
        transform.position = startPos;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        dir = worldMousePos - transform.position;
        dir.Normalize();
        if(dir.x > 0){
            foreach(Transform child in transform){
                child.gameObject.SetActive(true);
            }
            GetComponent<BoxCollider2D>().enabled = true;

            isCast = true;
        }
    }

    
    private void sendHead(){
        // back and -dir since i drew the head upside down
        transform.rotation = Quaternion.LookRotation(Vector3.forward, dir);
        totalMoved += grappleSpeed * Time.deltaTime;
        Vector3 translation = Vector2.up * grappleSpeed * Time.deltaTime;
        transform.Translate(translation);
        if(totalMoved > length){
            startReturn = true;
        }
    }


// can i make a general function for moving and when the it reaches the end make the speed *-1?
    private void sendGrappleBack(){
            // find the camera position to return the grapple to

            Vector3 translation = Vector2.down * grappleSpeed * Time.deltaTime;
            transform.Translate(translation);
            totalMoved -= grappleSpeed * Time.deltaTime;
            resetStates();
    }

    private void resetStates(){
        bool isBehindPlayer = transform.position.x - transform.parent.position.x < 1;
        if(totalMoved <= 0 || isBehindPlayer){
            foreach(Transform child in transform){
                child.gameObject.SetActive(false);
            }
            isAttached = false;
            startReturn = false;
            isCast = false;
            totalMoved = 0;
        }
        
    }

    private void drawLine(){
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.parent.position);
        lineRenderer.SetPosition(1, transform.position);
        
        transform.rotation = Quaternion.LookRotation(Vector3.forward, transform.position - transform.parent.position);
    }


    private void keepHeadInPlace(){
        transform.position = attachPos;
    }
    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.layer == LayerMask.NameToLayer("Floor")){
            GetComponent<BoxCollider2D>().enabled = false;
            isAttached = true;
            if(addNewSuccess){
                successCounter++;
                OnAttach?.Invoke(successCounter * .5f * speedBoost);
                addNewSuccess = false;
            }
            // move the player
            // attached();
            attachPos = transform.position;
            return;
        }
        else{
            resetStates();
            return;
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.layer == LayerMask.NameToLayer("Floor")){
            addNewSuccess = true;

        }
    }

    public bool getIsAttached(){
        return isAttached;
    }
    
    public Vector3 getDir(){
        return dir;
    }

    public void resetSuccessCounter(){
        successCounter = 0;
    }
}