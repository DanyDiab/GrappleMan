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

    // increase and decrease speed with d and s?
    void Start()
    {
        jumpFrames = 2;
        jumpForce = 20f;
        grappler = FindFirstObjectByType<Grapple>();
        rb = GetComponent<Rigidbody2D>();
        jumpDir = new Vector2(.2f,1);
    }

    void FixedUpdate(){
        // if in air, preserve momentum
        preserveAir();
    }



    void preserveAir(){
        if(!onFloor && grappler.getState() != grapplerState.PullingPlayer){
            if(airX == 0){
                airX = rb.velocity.x;
            }
            rb.velocity = new Vector2(airX, rb.velocity.y);
            return;
        }
        airX = 0;
        }



    void OnCollisionStay2D(Collision2D collision){
        if(collision.gameObject.layer == LayerMask.NameToLayer("Floor")){
            onFloor = true;
            return;
        }
    }

        void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.layer == LayerMask.NameToLayer("Floor")){
            onFloor = false;
            return;
        }
    }


}
