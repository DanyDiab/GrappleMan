using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    float jumpBoost;
    Rigidbody2D jumpedRb;
    // Start is called before the first frame update
    void Start()
    {
        jumpBoost = 25f;
    }

    // Update is called once per frame

    void OnTriggerEnter2D(Collider2D other)
    {
        jumpedRb = other.attachedRigidbody;
        if (jumpedRb != null)
        {
            jump();
        }
    }

    void jump()
    {
        float currYVelocity = jumpedRb.velocity.y;
        // if y velocity is negative make it positive
        if (currYVelocity < 0)
        {
            currYVelocity *= -1;
        }
        jumpedRb.AddForce(Vector2.up * (currYVelocity + jumpBoost), ForceMode2D.Impulse);
        jumpedRb = null;
    }
}
