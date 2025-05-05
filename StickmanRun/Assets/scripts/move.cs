using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class move : MonoBehaviour
{
    private float speed;
    private float initSpeed;

    public float jumpForce; 
    private bool onFloor;
    private bool jumped;
    private Grapple grappler;
    Vector3 currMove;
    Vector3 grappleDir;
    Rigidbody2D rb;
    float airX;
    float airSpeed;
    Vector3 initAirPos;
    public int jumpFrames;
    int currFrames;
    public Vector2 jumpDir;


    // Start is called before the first frame update

    // increase and decrease speed with d and s?
    void Start()
    {
        jumpFrames = 2;
        initSpeed = 10;
        speed = initSpeed;
        jumpForce = 20f;
        grappler = FindFirstObjectByType<Grapple>();
        rb = GetComponent<Rigidbody2D>();
        jumpDir = new Vector2(.2f,1);
    }

    void FixedUpdate(){
        Debug.Log(onFloor);

        // if not grappling, preserve air momentum
        preserveAir();
        

        // move to the right by default


    }



    void preserveAir(){
        if(!onFloor && grappler.getState() != 2){
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
