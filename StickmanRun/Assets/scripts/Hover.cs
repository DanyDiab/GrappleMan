using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{


    public float magnitude;
    public float speed;
    bool floating;
    Rigidbody2D rb;
    Vector3 originalPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPos = rb.position;
        floating = true;
    }


    void Update()
    {
        if (floating){
            float sineValue = Mathf.Sin(Time.time * speed) * magnitude;
            transform.position = new Vector3(0, sineValue, 0) + originalPos;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void startFloat()
    {
        floating = true;
    }


    public void endFloat()
    {
        floating = false;
    }
}