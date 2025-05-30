using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    float jumpBoost;
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
        jumpBoost = 70f;
        maxForce = 25f;
        
    }

    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Grappler") return;
        jumpedRb = other.attachedRigidbody;
        airTime = other.GetComponentInParent<AirTime>();
        if (jumpedRb != null && !jumped)
        {
            jumped = true;
            // jumpedRb.gravityScale = jumpedRb.gravityScale / 2;
            objectShake.objectShake();
            airTime.endAir();
            // maxAirTime = airTime.getAirTime();
            // if (maxAirTime < 1f) maxAirTime = 1f;
            jumpedRb.AddForce(Vector2.up * jumpBoost, ForceMode2D.Impulse);
            maxAirTime = 1f;
            addForces = true;
            // Vector2 launchVector = calculateDirToJump();
            // jumpedRb.gravityScale = jumpedRb.gravityScale * 2;

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (jumped)
        {
            jumped = !jumped;
        }
    }

    Vector2 calculateDirToJump()
    {
        float angle = transform.rotation.z - 180;
        float xUp = Mathf.Cos(angle);
        float yUp = Mathf.Sin(angle);

        Vector2 perp = new Vector2(xUp, yUp);
        return perp;
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
