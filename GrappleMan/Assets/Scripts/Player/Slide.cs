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
    public LayerMask floorLayer;
    float slopeC;
    Inputs inputs;


    void Start()
    {
        startedSlide = false;
        dragWhileSlide = 5f;
        player = GetComponent<Player>();
        rb = player.GetComponent<Rigidbody2D>();
        ogDrag = rb.drag;
        slopeC = 5f;
        inputs = GetComponent<Inputs>();
    }
    void Update()
    {
        sPressed = inputs.getKeyDown(KeyCode.S);
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
            // addSlopeAccel();
            player.setSliding(true);
            return;
        }
        rb.drag = ogDrag;
        player.setSliding(false);
        startedSlide = false;
    }

    void preventUpwardsVelocity()
    {

        if (rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
    }


// WIP 5/26/25
    void addSlopeAccel()
    {

        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.down, 6f, floorLayer);
        if (hit)
        {

            // Vector2 slope = hit.normal;
            // Vector2 perp = Vector2.Perpendicular(slope) * -1;
            // float angle = Vector2.Angle(Vector2.down, slope);
            // float adjustedAngle = 180 - angle;
            // float speed = adjustedAngle * slopeC;
            float speed = 200;
            Vector2 slopeDir = transform.up;
            Vector2 perp = Vector2.Perpendicular(slopeDir);
            // Vector2 perp = new Vector2(-slopeDir.y, slopeDir.x);
            rb.AddForce(perp * speed, ForceMode2D.Force);

        }
        // gravity * sin(angle) 
        // find the angle by casting a ray downwards
    }
}