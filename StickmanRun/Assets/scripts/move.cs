using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class move : MonoBehaviour
{

    Player player;

    public float jumpForce; 
    private bool onFloor;
    Rigidbody2D rb;
    float airX;
    public int jumpFrames;
    public Vector2 jumpDir;
    bool initG;
    float ogGScale;
    float accelForce;
    AirTime airTime;
    float maxGScale;
    float linearDecay;

    // increase and decrease speed with d and s?
    void Start()
    {
        player = GetComponent<Player>();
        jumpFrames = 2;
        jumpForce = 20f;
        rb = GetComponent<Rigidbody2D>();
        jumpDir = new Vector2(.2f, 1f);
        accelForce = 12f;
        airTime = GetComponent<AirTime>();
        maxGScale = 20f;
        linearDecay = .3f;
        
    }

    void FixedUpdate()
    {
        // if in air, preserve momentum
        preserveAir();
        calculateAirTime();
        addAccelerationToGravity();
    }



// add a linear decary to airX
    void preserveAir(){
        if(player.InAir() && rb.velocity.x != 0){
            if(airX == 0){
                airX = rb.velocity.x;
            }
            if(airX > 0) airX -= linearDecay;
            else if(airX < 0) airX += linearDecay;
            rb.velocity = new Vector2(airX, rb.velocity.y);
            return;
        }
        airX = 0;
    }

    void addAccelerationToGravity(){
        if(player.InAir()){
            if(!initG){
                ogGScale = rb.gravityScale;
                initG = true;
            }
            if (rb.gravityScale >= maxGScale)
            {
                rb.gravityScale = maxGScale;
                return;
            }
            rb.gravityScale += accelForce * Time.deltaTime;
        }
        else{
            initG = false;
            rb.gravityScale = ogGScale;
        }
    }

    void calculateAirTime()
    {
        if (player.InAir())
        {
            airTime.startAir();
            return;
        }
        airTime.endAir();
    }
}
