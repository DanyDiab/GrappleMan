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
    Collider2D col;

    void Start()
    {
        spawning = true;
        interval = .5f;
        col = GetComponentInChildren<Collider2D>();
        grapple = FindFirstObjectByType<Grapple>();
        grapple.OnGrapple += checkGrappleAttached;
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
        if(grappleAttached){
            if(grapple.getState() == grapplerState.PullingObject){
                Instantiate(prefabToSpawn, grapple.transform.position, grapple.transform.rotation);
                grappleAttached = false;
                return;
            }
            if(!grapple.isDeployed()){
                grappleAttached = false;
            }
       }
    }

    void checkGrappleAttached(Collider2D col){
        if(this.col == col){
            grappleAttached = true;
        }
    }

    void autoSpawn(){
        if(Time.time - lastSpawnTime >= interval){
            float randX = Random.Range(-randomizeXPos,randomizeXPos);
            Vector2 randPos = new Vector2(parent.position.x + randX, parent.position.y);
            Instantiate(prefabToSpawn,randPos,parent.rotation);
            lastSpawnTime = Time.time;
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
