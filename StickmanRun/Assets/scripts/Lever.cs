using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;

public enum LeverState{
    NotActive,
    Off,
    On,
}
public class Lever : MonoBehaviour
{
    LeverState currState;
    SpriteRenderer spriteRenderer;
    public Sprite notActiveSprite;
    public Sprite activeSprite;
    public Tilemap tileMapToActivate;
    // variable to keep track of when we initalliy change states to set run intial methods 
    Grapple grapple;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currState = LeverState.NotActive;
        grapple = FindAnyObjectByType<Grapple>();
    }


    void Update()
    {
        switch (currState)
        {
            // wait for the stick to be attacghed to set the state to off
            case LeverState.NotActive:
                spriteRenderer.sprite = notActiveSprite;
                break;
            // 
            case LeverState.Off:
                spriteRenderer.sprite = activeSprite;
                Debug.Log(grapple);
                // Debug.Log(grapple.getState());

                if (grapple != null && grapple.getState() == grapplerState.PullingObject)
                {
                    grapple.resetStates();
                    BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>(true);
                    foreach (BoxCollider2D col in colliders)
                    {
                        col.enabled = false;
                        currState = LeverState.On;

                    }
                    grapple = null;
                }
                break;
            case LeverState.On:
                spriteRenderer.flipX = true;
                tileMapToActivate.gameObject.SetActive(true);
                break;
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Stick" && currState == LeverState.NotActive)
        {

            Destroy(collision.gameObject);
            BoxCollider2D[] colliders = GetComponentsInChildren<BoxCollider2D>(true);
            foreach (BoxCollider2D col in colliders)
            {
                col.enabled = true;
            }
            currState = LeverState.Off;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Grappler")
        {
            grapple = collision.GetComponentInChildren<Grapple>();
        }
    }



}