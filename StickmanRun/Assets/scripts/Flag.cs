using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum FlagState
{
    AwaitingPickup,
    InInventory,
    Deployed

}
public class Flag : MonoBehaviour
{

    Vector3 originalPos;
    bool deployed;
    static Vector3 checkpoint;
    static int flagCount;
    FlagState currState;
    ParticleSystem floatingParticles;
    Rigidbody2D rb;
    float floatingSpeed;
    float floatingAmplitude;
    float confirmedTime;
    float pressedTime;
    Player player;
    bool interacted;
    SpriteRenderer spriteRenderer;

    void Start()
    {
        floatingSpeed = 15f;
        floatingAmplitude = .1f;
        rb = GetComponentInParent<Rigidbody2D>();
        floatingParticles = GetComponentInChildren<ParticleSystem>();
        originalPos = transform.position;
        currState = FlagState.AwaitingPickup;
        confirmedTime = 1f;
        player = FindAnyObjectByType<Player>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }


    void Update()
    {
        switch (currState)
        {
            case FlagState.AwaitingPickup:
                floatingSprite();
                break;
            case FlagState.InInventory:
                rb.bodyType = RigidbodyType2D.Static;
                checkForInput();
                if (interacted)
                {
                    interacted = false;
                    deploy();
                }
                break;
            case FlagState.Deployed:
                checkForInput();
                if (interacted)
                {
                    interacted = false;
                    teleportPlayer();
                }

                break;
        }
    }

    void floatingSprite()
    {
        float sineValue = Mathf.Sin(Time.time * floatingSpeed) * floatingAmplitude;
        transform.position = new Vector3(0, sineValue, 0) + originalPos;
        transform.rotation = Quaternion.Euler(0, 0, 0);

    }

    void checkForInput()
    {
        // hold key for 1 second
        if (Input.GetKey(KeyCode.R))
        {
            pressedTime += Time.deltaTime;
        }
        if (pressedTime >= confirmedTime)
        {
            pressedTime = 0;
            interacted = true;
        }
    }

    void deploy()
    {
        spriteRenderer.gameObject.SetActive(true);
        currState = FlagState.Deployed;
        transform.position = player.transform.position;
    }

    void teleportPlayer()
    {
        player.transform.position = transform.position;
    }


    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == "Grappler" && currState == FlagState.AwaitingPickup)
        {
            currState = FlagState.InInventory;
            floatingParticles.Stop();
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    

}