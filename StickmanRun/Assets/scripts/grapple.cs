using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour{

    // how far the grappler can reach
    private float length;
    private Vector3 dir;
    private float grappleSpeed;
    private float speedBoost;
    private float totalMoved;
    private bool isCast;

    private bool startReturn;

    void Start(){

        length = 15;
        grappleSpeed = 20;
        speedBoost = 7;
    }

    void FixedUpdate()
    {
        if(Input.GetMouseButton(0) && !isCast){
            cast();
        }
        if(startReturn){
            resetGrapple();
            return;
        }
        if(isCast){
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

    // prob wont work for player movement
    private void attached(){
        transform.parent.transform.Translate(speedBoost * Time.deltaTime * dir);
    }

    private void resetGrapple(){
            // find the camera position to return the grapple to

            Vector3 translation = Vector2.up * grappleSpeed * Time.deltaTime;
            transform.Translate(translation);
            totalMoved -= grappleSpeed * Time.deltaTime;
            bool isBehindPlayer = transform.position.x - Camera.main.transform.position.x < 0;
            if(totalMoved <= 0 || isBehindPlayer){
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                startReturn = false;
                isCast = false;
                totalMoved = 0;
            }
            // Vector3 returnPos = new Vector3(camPos.x,camPos.y,1);
            // transform.position = returnPos;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.layer == LayerMask.NameToLayer("Floor")){
            startReturn = true;
            // move the player
            // attached();
            return;
        }
    }
}