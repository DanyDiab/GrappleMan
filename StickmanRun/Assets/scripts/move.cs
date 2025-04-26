using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private int speed;
    private float gconstant;
    private float gforce;
    private float initGForce;
    private float jumpForce; 
    private bool onFloor;
    private bool jumped;

    // Start is called before the first frame update

    // increase and decrease speed with d and s?
    void Start()
    {
        speed = 4;
        gconstant = .18f;
        gforce = 0;
        jumpForce = .15f;
        initGForce = .2f;
    }
    void FixedUpdate(){
        applyGravity();
        // move to the right by default
        if(Input.GetKey(KeyCode.Space) && onFloor){
            jump();
        }

        float AdjustedSpeed = speed * Time.deltaTime;
        Vector3 translate = new Vector3(AdjustedSpeed,-gforce,0);
        transform.Translate(translate);
    }


    void applyGravity(){
        if(onFloor){
            jumped = false;
            gforce = 0;
            return;
        }
        else if(gforce < 2){
            Debug.Log(gforce);
            // add an initial gravity force
            if(gforce == 0 && jumped){
                gforce = initGForce;
            }
            // continue adding to the gravity force
            gforce += gconstant * Time.deltaTime;
        }
    }
    void jump(){
        jumped = true;
        gforce -= jumpForce;
    }


    void OnCollisionEnter2D(Collision2D collision){
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
