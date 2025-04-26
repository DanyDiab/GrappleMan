using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour{

    // how far the grappler can reach
    private float length;
    private Vector3 dir;
    private float grappleSpeed;

    private float totalMoved;
    private bool isCast;

    private bool startReturn;
    private bool isAttached;
    public event Action<Vector3> OnAttach;
    private float speedBoost;

    void Start(){

        length = 15;
        grappleSpeed = 25;
        speedBoost = 10;
    }

    void FixedUpdate()
    {
        if(Input.GetMouseButton(0) && !isCast){
            cast();
        }
        else if(isAttached){
            resetStates();
            OnAttach?.Invoke(dir);
        }
        else if(startReturn){
            sendGrappleBack();
            return;
        }
        else if(isCast){
            sendHead();
        }

    }
    



    public void cast(){
        Vector3 startPos = Camera.main.transform.position;
        startPos.z = 0;
        transform.position = startPos;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        dir = worldMousePos - transform.position;
        dir.Normalize();
        if(dir.x > 0){
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            isCast = true;
        }
    }

    
    private void sendHead(){
        // back and -dir since i drew the head upside down
        transform.rotation = Quaternion.LookRotation(Vector3.forward, -dir);
        totalMoved += grappleSpeed * Time.deltaTime;
        Vector3 translation = Vector2.down * grappleSpeed * Time.deltaTime;
        transform.Translate(translation);
        if(totalMoved > length){
            startReturn = true;
        }
    }


// can i make a general function for moving and when the it reaches the end make the speed *-1?
    private void sendGrappleBack(){
            // find the camera position to return the grapple to

            Vector3 translation = Vector2.up * grappleSpeed * Time.deltaTime;
            transform.Translate(translation);
            totalMoved -= grappleSpeed * Time.deltaTime;
            resetStates();
    }

    private bool resetStates(){
        bool isBehindPlayer = transform.position.x - Camera.main.transform.position.x < 0;
        if(totalMoved <= 0 || isBehindPlayer){
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            isAttached = false;
            startReturn = false;
            isCast = false;
            totalMoved = 0;
            return true;
        }
        return false;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.layer == LayerMask.NameToLayer("Floor")){
            isAttached = true;
            // move the player
            // attached();
            return;
        }
        else{
            resetStates();
            return;
        }
    }

    public float getSpeedBoost(){
        return speedBoost;
    }

    public bool getIsAttached(){
        return isAttached;
    }
    
    public Vector3 getDir(){
        return dir;
    }
}