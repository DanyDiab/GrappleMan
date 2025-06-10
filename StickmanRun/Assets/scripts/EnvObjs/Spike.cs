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
            OpacityFlash opacityFlash = other.gameObject.GetComponentInChildren<OpacityFlash>();
            SpriteRenderer[] spriteRenderers = other.gameObject.transform.GetComponentsInChildren<SpriteRenderer>();
            opacityFlash.startFlash(spriteRenderers,2f,0);
            Inputs.toggleInput(false, timePenalty);
        }
    }
}
