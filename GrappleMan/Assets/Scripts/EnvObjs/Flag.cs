using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public enum FlagState
{
    AwaitingPickup,
    InInventory,
    Deployed

}
public class Flag : MonoBehaviour
{

    static Flag currentFlag;

    Vector3 originalPos;
    bool deployed;
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
    public Image image;
    public TextMeshProUGUI text;
    Hover hover;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        floatingParticles = GetComponentInChildren<ParticleSystem>();
        originalPos = transform.position;
        currState = FlagState.AwaitingPickup;
        confirmedTime = 1f;
        player = FindAnyObjectByType<Player>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        hover = GetComponent<Hover>();
    }


    void Update()
    {
        switch (currState)
        {
            case FlagState.AwaitingPickup:
                floatingSprite();
                return;
            case FlagState.InInventory:
                drawUI("Place Flag?");
                rb.bodyType = RigidbodyType2D.Static;
                if (player.getIsMoving()) return;

                checkForInput();
                if (interacted)
                {
                    interacted = false;
                    deploy();
                }
                return;
            case FlagState.Deployed:
                drawUI("Teleport To Flag?");
                if (player.getIsMoving()) return;

                checkForInput();
                if (interacted)
                {
                    interacted = false;
                    teleportPlayer();
                }

                return;
        }
    }

    void floatingSprite()
    {
        hover.startFloat();
    }

    void checkForInput()
    {
        // hold key for 1 second
        if (Input.GetKey(KeyCode.R))
        {
            pressedTime += Time.deltaTime;
        }
        else
        {
            pressedTime = 0;
        }

        if (pressedTime >= confirmedTime)
        {
            pressedTime = 0;
            interacted = true;
            return;
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
        player.transform.position = currentFlag.transform.position;
    }

    void drawUI(string textToDisplay)
    {
        float percentFill = Mathf.Clamp(pressedTime / confirmedTime, 0, 1);
        image.fillAmount = percentFill;
        text.alpha = percentFill;
        text.text = textToDisplay;
    }


    void OnTriggerStay2D(Collider2D collider)
    {
        if ((collider.CompareTag("Grappler") || collider.CompareTag("Player")) && currState != FlagState.InInventory)
        {
            currentFlag = this;
            currState = FlagState.InInventory;
            hover.endFloat();
            floatingParticles.Stop();
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    

}