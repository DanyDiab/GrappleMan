using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    float jumpBoost;
    Rigidbody2D jumpedRb;
    AirTime airTime;
    bool addForces;
    float timeInAir;
    float maxAirTime;
    CamShake camShake;

    // Start is called before the first frame update
    void Start()
    {
        camShake = Camera.main.GetComponent<CamShake>();
        jumpBoost = 3f;
    }

    // Update is called once per frame

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Grappler") return;
        jumpedRb = other.attachedRigidbody;
        airTime = other.GetComponentInParent<AirTime>();
        if (jumpedRb != null && airTime != null)
        {
            camShake.camShake();
            airTime.endAir();
            maxAirTime = airTime.getAirTime();
            addForces = true;
        }
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
        timeInAir += Time.deltaTime;


        // if y velocity is negative make it positive
        //

        float t = timeInAir / maxAirTime;
        float forceScale = Mathf.Clamp01(1 - t);
        float v = jumpBoost * forceScale * jumpedRb.gravityScale;
        if (maxAirTime < .1f)
        {
            v = jumpBoost * jumpedRb.gravityScale;
        }
        // v += currYVelocity;
        jumpedRb.AddForce(Vector2.up * v, ForceMode2D.Impulse);
        if (timeInAir > maxAirTime)
        {
            addForces = false;
        }
    }
}
