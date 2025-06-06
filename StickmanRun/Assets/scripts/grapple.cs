using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;


public enum grapplerState
{
    Idle,
    Casting,
    Attached,
    PullingPlayer,
    PullingObject,
    Swinging,
    Retracting,
    Exploding,
}

public class Grapple : MonoBehaviour
{
    
    // state
    grapplerState currState;

    // vectors
    Vector2 dir;
    Vector2 moveDir;
    Vector2 lastPos;


// float
    float grappleSpeed;
    float returnSpeed;
    float length;

    public float speedBoost;

    float distance;
    float pushForce;

// inputs
    bool leftClick;
    bool aPressed;
    bool dPressed;
    bool wPressed;
    bool reverseDir;

    // rigidbodies
    Rigidbody2D rb;
    Rigidbody2D attachedRigidBody;
    Rigidbody2D parentRb;


// collisions
    Pullable pullObject;
    public LayerMask collisionMask;


// drawing line stuff
    GrappleHandPosition grappleHandPosition;
    LineRenderer lineRenderer;

    // particle
    public ParticleSystem explosion;
    bool startParticles;
    float playTime;
    float currTime; 

    // audio
    public AudioClip grappleAttach;
    SoundManager soundManager;


    public event OnGrappleAttach OnGrapple;


    protected void Start()
    {
        explosion.Stop();
        startParticles = false;

        parentRb = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        grappleHandPosition = transform.parent.gameObject.GetComponent<GrappleHandPosition>();
        currState = grapplerState.Idle;
        length = 16;
        grappleSpeed = 25;
        returnSpeed = 40;
        lineRenderer = GetComponentInChildren<LineRenderer>();
        rb = GetComponent<Rigidbody2D>();
        reverseDir = false;
        lastPos = transform.position;
        speedBoost = 350;
        pushForce = 75;
        playTime = .6f;

        soundManager = FindAnyObjectByType<SoundManager>();
    }

    void FixedUpdate()
    {
        distance = Vector2.Distance(parentRb.position, transform.position);
        switch (currState)
        {
            case grapplerState.Idle:
                resetStates();
                if (leftClick)
                {
                    cast();
                    currState = grapplerState.Casting;
                }
                break;
            case grapplerState.Casting:
                moveGrappler(true, grappleSpeed);
                break;
            case grapplerState.Attached:
                determinePullOrPush();
                keepHeadInPlace(true);
                break;
            case grapplerState.Retracting:
                retractGrappler();
                keepHeadInPlace(false);
                if (pullObject != null)
                {
                    pullObject.setIsPulled(false);
                }
                break;
            case grapplerState.PullingPlayer:
                determinePullOrPush();
                keepHeadInPlace(true);
                calculateMoveDirection();
                applyMove();
                break;
            case grapplerState.PullingObject:
                keepHeadInPlace(true);
                determinePullOrPush();
                // pulling function
                if (pullObject != null)
                {
                    // pull object
                    pullObject.getRb().velocity = rb.velocity;
                    pullObject.setIsPulled(true);
                    retractGrappler();
                }
                break;
            case grapplerState.Exploding:
                startParticles = true;
                calculateMoveDirection();
                moveDir *= -1;
                capMoveDir();
                parentRb.AddForce(moveDir * pushForce, ForceMode2D.Impulse);
                currState = grapplerState.Retracting;
                // applyMove();
                break;
                // if not pulling object input
        }
    }

    void Update()
    {
        aPressed = Input.GetKey(KeyCode.A);
        dPressed = Input.GetKey(KeyCode.D);
        wPressed = Input.GetKey(KeyCode.W);
        leftClick = Input.GetMouseButton(0);
        if (currState == grapplerState.Casting)
        {
            rayCastCollide();
        }
        if (distance > length || ((currState != grapplerState.Idle) && !leftClick))
        {
            currState = grapplerState.Retracting;
        }
        if (startParticles)
        {
            explosion.gameObject.transform.parent = null;
            currTime += Time.deltaTime;
            explosion.Play();
            if (currTime > playTime)
            {
                currTime = 0f;
                startParticles = false;

            }
        }
        else
        {
            explosion.Stop();
            explosion.transform.localPosition = Vector3.zero;
            explosion.gameObject.transform.parent = transform;

        }
    }

    void LateUpdate()
    {
        drawLine();
    }


    void determinePullOrPush()
    {
        if (wPressed)
        {
            currState = grapplerState.Exploding;
            return;
        }
        if (parentRb.position.x - rb.position.x > 0)
        {
            if (aPressed)
            {
                currState = grapplerState.PullingPlayer;
                return;
            }

            else if (dPressed)
            {
                currState = grapplerState.PullingObject;
                return;
            }
            currState = grapplerState.Attached;
            return;
        }
        if (aPressed)
        {
            currState = grapplerState.PullingObject;
            return;
        }
        else if (dPressed)
        {
            currState = grapplerState.PullingPlayer;
            return;
        }
        currState = grapplerState.Attached;
        return;   
    
    }

    void capMoveDir()
    {
        if (moveDir.x > .5f) moveDir.x = .5f;
        else if (moveDir.x < -.5f) moveDir.x = -.5f;
    }

    public void cast()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        // Vector3 startPos = new Vector3(transform.parent.position.x,transform.position.y + 2,1);
        Vector3 startPos = parentRb.position;
        transform.position = startPos;
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 1;
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);
        dir = worldMousePos - transform.position;
        dir.Normalize();
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);


        // GetComponent<BoxCollider2D>().enabled = true;
    }


    protected void moveGrappler(bool forward, float speed)
    {
        if (!forward)
        {
            
            dir = (parentRb.position - rb.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (!reverseDir)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, angle + 90f);
                reverseDir = true;
            }
        }
        rb.velocity = dir * speed;
        distance = Vector2.Distance(rb.position, parentRb.position);
        // Debug.Log(distance);
    }

    public void resetStates(){
        // set state to idle/default
        transform.parent = parentRb.transform;
        transform.localPosition = Vector3.zero;
        pullObject = null;
        distance = 0;
        rb.velocity = Vector2.zero;
        lastPos = transform.position;
        attachedRigidBody = null;
        // transform.position = parent.transform.position;
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void calculateMoveDirection()
    {
        moveDir = rb.position - parentRb.position;
        moveDir.Normalize();
    }

    protected void applyMove()
    {
        parentRb.AddForce(speedBoost * moveDir, ForceMode2D.Force);
    }




    protected void drawLine()
    {
        if (lineRenderer == null) return;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, grappleHandPosition.getCurrPosition().position);
        lineRenderer.SetPosition(1, transform.position);
        lineRenderer.sortingOrder = grappleHandPosition.getDrawingLayer();
    }


    protected void keepHeadInPlace(bool apply)
    {
        if (apply)
        {
            if (attachedRigidBody != null && attachedRigidBody.bodyType != RigidbodyType2D.Static)
            {

                rb.constraints = RigidbodyConstraints2D.None;
                rb.position = attachedRigidBody.position;
                return;
            }
            rb.bodyType = RigidbodyType2D.Static;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
        }
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.None;
    }
    void retractGrappler()
    {
        keepHeadInPlace(false);
        moveGrappler(false, returnSpeed);
        if (distance <= 0.5f)
        {
            currState = grapplerState.Idle;
        }
        return;
    }

    void rayCastCollide()
    {
        float distance = Vector2.Distance(transform.position, lastPos);
        float buffer = .3f;
        Vector2 extendedPos = lastPos + dir * (distance + buffer);
        RaycastHit2D hit = Physics2D.Linecast(lastPos, extendedPos, collisionMask);
        if (hit.collider != null)
        {
            grapplerAttach(hit.collider, hit.point);
        }
        lastPos = transform.position;
    }

    void grapplerAttach(Collider2D collider, Vector2 attachPoint)
    {
        // set the grappler position to attachedPoint
        soundManager.playSound(grappleAttach);
        transform.position = attachPoint;
        
        parentRb = transform.parent.GetComponent<Rigidbody2D>();
        grappleHandPosition = transform.parent.GetComponent<GrappleHandPosition>();
        transform.parent = null;
        OnGrapple?.Invoke();
        currState = grapplerState.Attached;
        attachedRigidBody = collider.attachedRigidbody;

        if (collider.gameObject.layer != LayerMask.NameToLayer("Floor"))
        {
            attachedRigidBody = collider.attachedRigidbody;        
        }
        if (collider.gameObject.layer == LayerMask.NameToLayer("Pullables"))
        {
            pullObject = collider.GetComponentInParent<Pullable>();

        }
    }

    public delegate void OnGrappleAttach();

    public Vector2 getDir()
    {
        return dir;
    }
    public grapplerState getState()
    {
        return currState;
    }

    public bool isActive()
    {
        return currState == grapplerState.PullingObject || currState == grapplerState.PullingPlayer;
    }
    
        public bool isDeployed()
    {
        return currState != grapplerState.Idle && currState != grapplerState.Retracting;
    }
}