using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CatapultState{
    Idle, 
    AwaitingLaunch,
    Launching,
    Retracting
}

public class Catapult : MonoBehaviour
{

    public Transform rockStuckPos;
    public Transform arm;

    CatapultState currState;
    CatapultRock currRock;
    Rigidbody2D currRockRb;

    Vector2 rockStartPos;
    float timeUntilLaunch;
    float currTime;
    float startRotation;
    float endRotation;
    float sendRotationSpeed;
    float backRotationSpeed;
    bool reached;
    
    // Start is called before the first frame update
    void Start()
    {
        startRotation = 355f;
        endRotation = 280f;
        sendRotationSpeed = 5f;
        backRotationSpeed = 1f;
        currState = CatapultState.Idle;
        timeUntilLaunch = 2f;
    }

    // Update is called once per frame

    void FixedUpdate(){
        switch(currState){
            case CatapultState.Idle:
                break;
            case CatapultState.AwaitingLaunch:
                currRock.transform.position = rockStuckPos.position;
                currRock.transform.rotation = arm.rotation;
                currTime += Time.fixedDeltaTime;
                if(currTime > timeUntilLaunch){
                    currTime = 0f;
                    rockStartPos = currRockRb.position;
                    currState = CatapultState.Launching;
                }
                break;
                // when we launch the rock, set the rock rb back to dynamic and constraints to none
            case CatapultState.Launching:
                currRock.transform.position = rockStuckPos.position;
                currRock.transform.rotation = arm.rotation;
                reached = rotateArm(endRotation,sendRotationSpeed);
                if(reached){
                    currRock.startLife();
                    currRockRb.bodyType = RigidbodyType2D.Dynamic;
                    currState = CatapultState.Retracting;
                    Vector2 dirToSend = (currRockRb.position - rockStartPos).normalized;
                    Debug.Log(dirToSend);
                    currRockRb.AddForce(dirToSend * sendRotationSpeed * 5, ForceMode2D.Impulse);

                }
                break;
            case CatapultState.Retracting:
                reached = rotateArm(startRotation,backRotationSpeed);
                if(reached){
                    currState = CatapultState.Idle;
                }
                break;
        }
    }
    // returns if reached target rotation
    bool rotateArm(float targetRotation, float rotateSpeed){
        float adjustment = 0f;
        if(arm.eulerAngles.z < targetRotation) adjustment = rotateSpeed;
        else if(arm.eulerAngles.z > targetRotation) adjustment = -rotateSpeed;
        arm.rotation = Quaternion.Euler(0,0,arm.eulerAngles.z + adjustment);
        return Math.Abs(arm.eulerAngles.z - targetRotation) <= rotateSpeed;
    }


    void OnTriggerStay2D(Collider2D collision){
        if(collision.CompareTag("CatapultRock") && currState == CatapultState.Idle){
            currRock = collision.GetComponentInParent<CatapultRock>();
            currRockRb = collision.attachedRigidbody;
            currRockRb.constraints = RigidbodyConstraints2D.FreezeRotation;
            currRockRb.bodyType = RigidbodyType2D.Kinematic;
            currRock.pauseLifeTimer();
            currRock.resetLife();
            currState = CatapultState.AwaitingLaunch;
        }
    }
}
