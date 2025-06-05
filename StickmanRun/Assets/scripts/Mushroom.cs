using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    float jumpBoost;
    float minBoost;
    Rigidbody2D jumpedRb;
    AirTime airTime;
    bool addForces;
    float timeInAir;
    float maxAirTime;
    ObjectShake objectShake;
    float maxForce;
    bool jumped;


    // Start is called before the first frame update
    void Start()
    {
        objectShake = GetComponent<ObjectShake>();
        minBoost = 70f;
        maxForce = 25f;
        
    }

    // Update is called once per frame

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Grappler")) return;
        jumpedRb = other.attachedRigidbody;
        airTime = other.GetComponentInParent<AirTime>();
        if (jumpedRb != null && !jumped)
        {
            jumped = true;
            objectShake.objectShake();
            airTime.endAir();
            jumpBoost = calculateJumpBoost();
            Debug.Log(transform.up);
            float jumpX = transform.up.x;
            float currX = jumpedRb.velocity.x;
            if((jumpX > 0 && currX < 0) || (jumpX < 0 && currX > 0)) jumpedRb.velocity = new Vector2(jumpedRb.velocity.y,-jumpedRb.velocity.x);
            jumpedRb.AddForce(transform.up * jumpBoost, ForceMode2D.Impulse);
            maxAirTime = 1f;
            addForces = true;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (jumped)
        {
            jumped = !jumped;
        }
    }

    // Vector2 calculateDirToJump()
    // {
    //     Vector2 normal = transform.up;
    // }
    float calculateJumpBoost()
    {
        float velocityBounce = Math.Abs(jumpedRb.velocity.x) * 3.5f;
        return Mathf.Max(velocityBounce, minBoost);
    }

    void FixedUpdate()
    {
        if (addForces)
        {
            jump();
            return;
        }
    }

    void jump()
    {
        // timeInAir += Time.deltaTime;
        // if y velocity is negative make it positive
        //
        // float t = timeInAir / maxAirTime;
        // float forceScale = Mathf.Clamp01(1 - t);
        // float v = jumpBoost * forceScale * jumpedRb.gravityScale;
        // if (maxAirTime < .1f)
        // {
        // v = jumpBoost * jumpedRb.gravityScale;
        // }
        // if (v > maxForce) v = maxForce;
        // v += currYVelocity;
        
        // if (timeInAir > maxAirTime)
        // {
        //     addForces = false;
        // }
    }
}
