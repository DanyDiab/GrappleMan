using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultRock : MonoBehaviour
{
    float lifeTime;
    float currLifeTime;
    bool startLifetime;

    // Start is called before the first frame update
    void Start()
    {
        startLifetime = true;
        currLifeTime = 0f;
        lifeTime = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        if(startLifetime){
            killPastLifetime();
        }
    }

    void killPastLifetime(){
        currLifeTime += Time.deltaTime;
        if(currLifeTime > lifeTime){
            Destroy(gameObject);
        }
    }

    public void startLife(){
        startLifetime = true;
    }
    public void pauseLifeTimer(){
        startLifetime = false;
    }
    public void resetLife(){
        currLifeTime = 0f;
    }
    // add raycasting 
}
