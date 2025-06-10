using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    ParticleSystem[] particleSystems;
    // Start is called before the first frame update
    void Start()
    {
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
                break;
        }
    }



    void checkForGrapplePull(){
        if(grappleAttached && grapple != null){
            if(grapple.getState() == grapplerState.PullingObject){
                currState = ValveState.Turning;
                grapple = null;
                grappleAttached = false;
            }
            if(grapple != null){
                if(!grapple.isDeployed()){
                    grapple = null;
                    grappleAttached = false;
                }
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

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Grappler")){
            grapple = collision.GetComponentInParent<Grapple>();
            grappleAttached = true;
        }    
    }

    public bool isCompleted(){
        return currState == ValveState.Complete;
    }



}
