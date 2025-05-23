using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slide : MonoBehaviour
{


    Player player;
    Rigidbody2D rb;
    bool sPressed;
    float dragWhileSlide;
    float ogDrag;
    bool startedSlide;


    void Start()
    {
        startedSlide = false;
        dragWhileSlide = 5f;
        player = GetComponent<Player>();
        rb = player.GetComponent<Rigidbody2D>();
        ogDrag = rb.drag;
    }
    void Update()
    {
        sPressed = Input.GetKey(KeyCode.S);
        slide();
        // if (startedSlide)
        // {
        //     preventUpwardsVelocity();
        // }
    }

    void slide()
    {
        if (sPressed && player.getOnFloor())
        {
            if (!startedSlide)
            {
                ogDrag = rb.drag;
                rb.drag = dragWhileSlide;
                startedSlide = true;
            }
            player.setSliding(true);
            return;
        }
        rb.drag = ogDrag;
        player.setSliding(false);
        startedSlide = false;
    }

    void preventUpwardsVelocity(){
        
        if (rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }
}