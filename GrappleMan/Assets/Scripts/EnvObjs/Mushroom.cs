using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    float jumpBoost;
    float minBoost;
    float jumpCooldown;
    float lastJumpTime;
    Rigidbody2D jumpedRb;
    AirTime airTime;
    bool addForces;
    float timeInAir;
    float maxAirTime;
    ObjectShake objectShake;
    float maxForce;
    bool jumped;
    SoundManager soundManager;
    public AudioClip jumpSound;
    String jumpedTag;


    // Start is called before the first frame update
    void Start()
    {
        jumpCooldown = .1f;
        objectShake = GetComponent<ObjectShake>();
        minBoost = 70f;
        maxForce = 25f;
        soundManager = FindAnyObjectByType<SoundManager>();
        
    }

    // Update is called once per frame

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Grappler")) return;
        jumpedRb = other.attachedRigidbody;
        if (jumpedRb != null && Time.time - lastJumpTime > jumpCooldown)
        {
            jumpedTag = other.tag;
            jumpBoost = calculateJumpBoost();
            reverseDirIfNeeded();
            lastJumpTime = Time.time;

            jumpedRb.AddForce(transform.up * jumpBoost, ForceMode2D.Impulse);
            soundManager.playSound(jumpSound);
            objectShake.objectShake();
        }
    }

    float calculateJumpBoost()
    {
        float velocityBounce = Math.Abs(jumpedRb.velocity.x) * 3.5f;
        return Mathf.Max(velocityBounce, minBoost);
    }


/// <summary>
/// Reverses the direction that the jumpedRb is going in if the bounce send him in an opposite direction
/// </summary>
    void reverseDirIfNeeded(){
        float jumpX = transform.up.x;
        float currX = jumpedRb.velocity.x;
        if((jumpX > 0 && currX < 0) || (jumpX < 0 && currX > 0)) jumpedRb.velocity = new Vector2(jumpedRb.velocity.y,-jumpedRb.velocity.x);
    }
}
