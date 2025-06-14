using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    bool sliding;
    bool onFloor;
    Grapple grappler;
    [SerializeField] float onFloorDrag;
    [SerializeField] float otherDrag;
    [SerializeField] float slidingDrag;


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
    public ParticleSystem slidingParticles;
    bool isMoving;
    bool gotBounce;
    bool spiked;
    OpacityFlash opacityFlash;

    [Header("Floor Detection")]
    [SerializeField] LayerMask floorLayerMask;
    [SerializeField] float raycastDistance = .6f;

    [SerializeField] float raycastOffset = .1f;

    [SerializeField] int numRays;






    void Start()
    {
        grappler = FindFirstObjectByType<Grapple>();
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        grapple = GetComponentInChildren<Grapple>();
        opacityFlash = GetComponentInChildren<OpacityFlash>();
        movingTolerance = .1f;
    }
    void Update()
    {
        adjustDrag();

        determineIfIsMoving();
        switchPlayerSprite();
        drawHand();
        enableSlidingParticles();
        checkIfOnFloor();
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
        if (!isMoving)
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

    void enableSlidingParticles()
    {
        if (isSliding() && isMoving)
        {
            slidingParticles.Play();
            if (currSprite == left)
            {
                slidingParticles.transform.localPosition = Vector3.zero;
                slidingParticles.transform.rotation = Quaternion.Euler(0, 0, 0);

            }
            else
            {
                slidingParticles.transform.localPosition = new Vector3(-2f, -2f, 0);
                slidingParticles.transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            return;
        }
        slidingParticles.Stop();
    }


    void drawHand()
    {
        if(opacityFlash.isFlashing()) return;
        if (grapple.getState() != grapplerState.Idle)
        {
            hand.color = new Color(hand.color.r,hand.color.g,hand.color.b,0);
            return;
        }
        hand.color = new Color(hand.color.r,hand.color.g,hand.color.b,1);
        if (currSprite == left || currSprite == right)
        {
            hand.transform.position = grappleHandPosition.handStartFacing.position;
        }
        else
        {
            hand.transform.position = grappleHandPosition.handStartNormal.position;
        }
    }

    void determineIfIsMoving()
    {
        isMoving = Math.Abs(0 - rb.velocity.x) >= movingTolerance;

    }

    void adjustDrag(){
        if(sliding){
            rb.drag = slidingDrag;
            return;
        }
        else if(onFloor && !grapple.isDeployed() && !gotBounce){
            rb.drag = onFloorDrag;
        }

        else{
            rb.drag = otherDrag;
        }
    }

    void checkIfOnFloor(){
        onFloor = false;
        Bounds bounds = GetComponentInChildren<Collider2D>().bounds;
        Vector2 rayOrigin = new Vector2(bounds.center.x, bounds.min.y);
        for(int i = 0; i < numRays; i++){
            Vector2 rayStart = rayOrigin;
            if(numRays > 1){
                float offsetRange = bounds.size.x * .4f;
                float step = (offsetRange * 2) / (numRays -1);
                rayStart.x += -offsetRange + (i * step);
            }

            RaycastHit2D hit = Physics2D.Raycast(rayStart, Vector2.down,raycastDistance, floorLayerMask);
            if (hit.collider != null){
                onFloor = true;
                if(!hit.collider.CompareTag("Mushroom")) gotBounce = false; 
                break;
            }
        }
    }

    public RaycastHit2D[] GetAllFloorRaycastHits()
{
    RaycastHit2D[] hits = new RaycastHit2D[numRays];
    Bounds bounds = GetComponentInChildren<Collider2D>().bounds;
    Vector2 rayOrigin = new Vector2(bounds.center.x, bounds.min.y);
    
    for(int i = 0; i < numRays; i++)
    {
        Vector2 rayStart = rayOrigin;
        if(numRays > 1)
        {
            float offsetRange = bounds.size.x * .4f;
            float step = (offsetRange * 2) / (numRays - 1);
            rayStart.x += -offsetRange + (i * step);
        }
        
        hits[i] = Physics2D.Raycast(rayStart, Vector2.down, raycastDistance, floorLayerMask);
    }
    
    return hits;
}

public RaycastHit2D[] GetValidFloorHits()
{
    RaycastHit2D[] allHits = GetAllFloorRaycastHits();
    List<RaycastHit2D> validHits = new List<RaycastHit2D>();
    
    foreach(RaycastHit2D hit in allHits)
    {
        if(hit.collider != null)
        {
            validHits.Add(hit);
        }
    }
    
    return validHits.ToArray();
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
    public bool getIsMoving()
    {
        return isMoving;
    }
    public bool getSpiked(){
        return spiked;
    }
    public void setSpiked(bool spiked){
        this.spiked = spiked;
    }
    public void setgotBounce(bool gotBounce){
        this.gotBounce = gotBounce;
    }


}