using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    Grapple grapple;
    bool grappleAttached;
    bool checkForLeave;
    Coroutine checkGrapplecoRoutine;

    void Update(){
        if(grappleAttached && grapple != null){
            if(grapple.getState() == grapplerState.PullingObject){
                Instantiate(prefabToSpawn, grapple.transform.position, grapple.transform.rotation);
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

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Grappler")){
            grapple = collision.GetComponentInParent<Grapple>();
            grappleAttached = true;
        }    
    }


}
