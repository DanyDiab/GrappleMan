using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum ValveState{
    Idle,
    Turning,
    Complete
}
public class Valve : MonoBehaviour
{
    Grapple grapple;
    bool grappleAttached;
    ValveState currState;
    float startRotation;
    float targetRotation;
    float rotationSpeed;
    Collider2D myCol;
    ParticleSystem[] particleSystems;
    Light2D light;
    // Start is called before the first frame update
    void Start()
    {
        myCol = GetComponentInChildren<Collider2D>();
        grapple = FindFirstObjectByType<Grapple>();
        light = GetComponentInChildren<Light2D>();
        grapple.OnGrapple += checkGrappleAttached;
        startRotation = 0f;
        targetRotation = 270f;
        rotationSpeed = 1f;
        currState = ValveState.Idle;
        particleSystems = GetComponentsInChildren<ParticleSystem>();
        foreach (ParticleSystem item in particleSystems)
        {
            item.Stop();
        }   
    }

    // Update is called once per frame
    void Update()
    {
        switch(currState){
            case ValveState.Idle:
                checkForGrapplePull();
                break;
            case ValveState.Turning:
                if(rotateValve()){
                    currState = ValveState.Complete;
                    foreach (ParticleSystem item in particleSystems){
                        item.Play();
                    } 
                }
                break;
            case ValveState.Complete:
                light.color = Color.green;
                break;
        }
    }



    void checkForGrapplePull(){
        if(grappleAttached){
            if(grapple.getState() == grapplerState.PullingObject){
                currState = ValveState.Turning;
                grappleAttached = false;
                return;
            }
            if(!grapple.isDeployed()){
                grappleAttached = false;
            }
        }

    }

    bool rotateValve(){
        float adjustment = 0f;
        if(transform.eulerAngles.z < targetRotation) adjustment = rotationSpeed;
        else if(transform.eulerAngles.z > targetRotation) adjustment = -rotationSpeed;
        transform.rotation = Quaternion.Euler(0,0,transform.eulerAngles.z + adjustment);
        return Mathf.Abs(transform.eulerAngles.z - targetRotation) <= rotationSpeed;
    }

    void checkGrappleAttached(Collider2D col){
        if(col == myCol){
            grappleAttached = true;
        }
    }

    public bool isCompleted(){
        return currState == ValveState.Complete;
    }
}
