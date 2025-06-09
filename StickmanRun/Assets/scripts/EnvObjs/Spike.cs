using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    float timePenalty;
    // Start is called before the first frame update
    void Start()
    {
        timePenalty = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Player")){
            Inputs.toggleInput(false, timePenalty);
        }
    }
}
