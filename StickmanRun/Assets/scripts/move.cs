using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class move : MonoBehaviour
{

    public float jumpForce; 
    private bool onFloor;
    private Grapple grappler;
    Rigidbody2D rb;
    float airX;
    public int jumpFrames;
    public Vector2 jumpDir;
    bool initG;
    float ogGScale;
    float accelForce;

    // increase and decrease speed with d and s?
    void Start()
    {
        jumpFrames = 2;
        jumpForce = 20f;
        grappler = FindFirstObjectByType<Grapple>();
        rb = GetComponent<Rigidbody2D>();
        jumpDir = new Vector2(.2f,1);
        accelForce = 6f;
    }

    void FixedUpdate(){
        // if in air, preserve momentum
        preserveAir();
        addAccelerationToGravity();
    }



    void preserveAir(){
        if(InAir() && rb.velocity.x != 0){
            if(airX == 0){
                airX = rb.velocity.x;
            }
            rb.velocity = new Vector2(airX, rb.velocity.y);
            return;
        }
        airX = 0;
    }

    void addAccelerationToGravity(){
        if(InAir()){
            if(!initG){
                ogGScale = rb.gravityScale;
                initG = true;
            }
            rb.gravityScale += accelForce * Time.deltaTime;
        }
        else{
            initG = false;
            rb.gravityScale = ogGScale;
        }
    }

    


    void OnTriggerExit2D(Collider2D collider2D)
    {

        if(collider2D.gameObject.layer == LayerMask.NameToLayer("Floor") && tag == "Player"){
            onFloor = false;
            return;
        }
    }
    void OnTriggerStay2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.layer == LayerMask.NameToLayer("Floor")  && tag == "Player"){
            onFloor = true;
            return;
        }
    }

    bool InAir(){
        return !onFloor && grappler.getState() != grapplerState.PullingPlayer && grappler.getState() != grapplerState.PullingObject;
    }


}
