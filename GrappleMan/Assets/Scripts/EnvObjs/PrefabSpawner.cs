using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    Grapple grapple;
    bool grappleAttached;
    bool checkForLeave;
    bool spawning;
    Coroutine checkGrapplecoRoutine;
    bool isAuto;
    float interval;
    float lastSpawnTime;
    Transform parent;
    float randomizeXPos;

    void Start()
    {
        spawning = true;
        isAuto = false;
        interval = .5f;
    }

    void Update(){
        if(!spawning) return;

        if(!isAuto){
           manualSpawn(); 
        }
        else{
            autoSpawn();
        }
    }


    // change to an event system rather than collider system

    void manualSpawn(){
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

    void autoSpawn(){
        Debug.Log("auto");
        if(Time.time - lastSpawnTime >= interval){
            float randX = Random.Range(-randomizeXPos,randomizeXPos);
            Vector2 randPos = new Vector2(parent.position.x + randX, parent.position.y);
            Instantiate(prefabToSpawn,randPos,parent.rotation);
            lastSpawnTime = Time.time;
        }
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Grappler")){
            grapple = collision.GetComponentInParent<Grapple>();
            grappleAttached = true;
        }    
    }

    public void toggleAuto(bool toggle){
        isAuto = toggle;
    }

    public void setAutoSpawnVars(float interval, Transform parent, float randomizeXPos){
        this.interval = interval;
        this.parent = parent;
        this.randomizeXPos = randomizeXPos;
    }
    public void setSpawning(bool spawn){
        spawning = spawn;
    }



}
