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

    [Header("Slope Sliding Settings")]
    public float maxSlopeForce = 15f;        // Maximum force applied on steep slopes
    public float minSlopeAngle = 10f;        // Minimum angle to start applying slope force
    public float maxSlopeAngle = 60f;        // Angle at which maximum force is applied
    public float raycastDistance = .3f;     // Distance to cast ray for slope detection
    
    private Vector2 slopeDirection;          // Direction to apply slope force
    private float currentSlopeAngle;         // Current slope angle


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
            player.setSliding(true);
            addSlopeAccelAverage();
            return;
        }
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
  void addSlopeAccelAverage()
    {
        RaycastHit2D[] validHits = player.GetValidFloorHits();
        
        if (validHits.Length > 0)
        {
            Vector2 averageSlopeDirection = Vector2.zero;
            float averageAngle = 0f;
            int validSlopeCount = 0;
            
            foreach (RaycastHit2D hit in validHits)
            {
                Vector2 surfaceNormal = hit.normal;
                float slopeAngle = Vector2.Angle(surfaceNormal, Vector2.up);
                
                if (slopeAngle >= minSlopeAngle)
                {
                    Vector2 hitSlopeDirection = Vector2.Perpendicular(surfaceNormal);
                    if (hitSlopeDirection.y > 0)
                        hitSlopeDirection = -hitSlopeDirection;
                    
                    averageSlopeDirection += hitSlopeDirection;
                    averageAngle += slopeAngle;
                    validSlopeCount++;
                }
            }
            
            if (validSlopeCount > 0)
            {
                averageSlopeDirection /= validSlopeCount;
                currentSlopeAngle = averageAngle / validSlopeCount;
                slopeDirection = averageSlopeDirection.normalized;
                
                float slopeForceMultiplier = Mathf.InverseLerp(minSlopeAngle, maxSlopeAngle, currentSlopeAngle);
                float appliedForce = maxSlopeForce * slopeForceMultiplier;
                rb.AddForce(slopeDirection * appliedForce, ForceMode2D.Force);
                return;
            }
        }

    }
        
}