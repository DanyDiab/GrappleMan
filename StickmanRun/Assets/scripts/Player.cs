using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    bool sliding;
    bool onFloor;
    Grapple grappler;


    Rigidbody2D rb;
    public Sprite left;
    public Sprite right;
    public Sprite normal;
    public GrappleHandPosition grappleHandPosition;
    SpriteRenderer spriteRenderer;
    public SpriteRenderer hand;
    Sprite currSprite;
    Grapple grapple;
    float movingTolerance;



    void Start()
    {
        grappler = FindFirstObjectByType<Grapple>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        grapple = GetComponentInChildren<Grapple>();
        movingTolerance = .1f;
    }
    void Update()
    {
        switchPlayerSprite();
        drawHand();
        spriteRenderer.sprite = currSprite;
    }

    void switchPlayerSprite()
    {
        if (grapple.isDeployed())
        {
            if (grapple.transform.position.x - transform.position.x > 0)
            {
                currSprite = right;
            }
            else if (grapple.transform.position.x - transform.position.x < 0)
            {
                currSprite = left;
            }
            return;
        }
        if (Math.Abs(0 - rb.velocity.x) <= movingTolerance)
        {
            currSprite = normal;
            return;
        }
        if (rb.velocity.x < 0)
        {
            currSprite = left;
        }
        else
        {
            currSprite = right;
        }
    }


    void drawHand()
    {
        if (grapple.getState() != grapplerState.Idle)
        {
            hand.gameObject.SetActive(false);
            return;
        }
        hand.gameObject.SetActive(true);
        if (currSprite == left || currSprite == right)
        {
            hand.transform.position = grappleHandPosition.handStartFacing.position;
        }
        else
        {
            hand.transform.position = grappleHandPosition.handStartNormal.position;
        }
    }

    void OnTriggerExit2D(Collider2D collider2D)
    {

        if (collider2D.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            onFloor = false;
            return;

        }

    }
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            onFloor = true;
            return;
        }
    }

    public bool InAir()
    {
        return !onFloor && !grappler.isActive() || sliding;
    }


    public void setSliding(bool sliding)
    {
        this.sliding = sliding;
    }
    public bool getOnFloor()
    {
        return onFloor;
    }

    public bool isSliding()
    {
        return sliding;
    }


}