using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHitbox : MonoBehaviour
{
    bool triggered;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Grappler")){
            triggered = true;
        }   
    }


    public bool isTriggered(){
        return triggered;
    }
    public void resetTrigger(){
        triggered = false;
    }
}
