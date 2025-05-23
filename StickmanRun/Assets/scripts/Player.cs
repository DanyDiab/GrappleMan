using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public Sprite left;
    public Sprite right;
    public Sprite normal;
    public GrappleHandPosition grappleHandPosition;
    SpriteRenderer spriteRenderer;
    public SpriteRenderer hand;
    Sprite currSprite;
    Grapple grapple;
    


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        grapple = GetComponentInChildren<Grapple>();
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
        if (rb.velocity.x < 0)
        {
            currSprite = left;
        }
        else if (rb.velocity.x > 0)
        {
            currSprite = right;
        }
        else
        {
            currSprite = normal;
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


}