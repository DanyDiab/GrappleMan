using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private float speed;
    private float initSpeed;

    private float gconstant;
    private float gforce;
    private float initGForce;
    private float jumpForce; 
    private bool onFloor;
    private bool jumped;
    private Grapple grappler;
    private bool grappling;
    private float grappleSpeedCombo;


    // Start is called before the first frame update

    // increase and decrease speed with d and s?
    void Start()
    {
        initSpeed = 4;
        speed = initSpeed;
        gconstant = .18f;
        gforce = 0;
        jumpForce = .15f;
        initGForce = .2f;
        grappler = FindFirstObjectByType<Grapple>();
        grappler.OnAttach += grappleAttach;
    }

    void FixedUpdate(){
        applyGravity();
        // move to the right by default
        if(Input.GetKey(KeyCode.Space) && onFloor){
            jump();
        }

        Vector3 translate = calculateCurrMove();
        transform.Translate(translate);
    }


    void applyGravity(){
        if(onFloor){
            // resetSpeed()
            jumped = false;
            gforce = 0;
            if(!grappler.getIsAttached()){
                resetSpeed();
            }
            return;
        }
        else if(gforce < 2 && !grappler.getIsAttached()){
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


    // this is a function is called by an action
    void grappleAttach(float newSpeed){
        grappling = true;
        // keep calling funciton if the grapple attached
        speed = newSpeed;
        // when the grapple is not attached check if in the air, if in the air keep boost until floor
        // transform.Translate(dir * speed * Time.deltaTime);
    }

    Vector3 calculateCurrMove(){
        float AdjustedSpeed = speed * Time.deltaTime;

        Vector3 translate = new Vector3(AdjustedSpeed,-gforce,0);
        // if grappling apply different physics
        if(grappling){
            // grapple speed combo is reseting after each grapple
            translate = grappler.getDir() * AdjustedSpeed;
            // only take into account the grappler y value if hes not on the floor/didnt jump
            if(!onFloor && !jumped){
                translate.y = grappler.getDir().y;
            }
            translate.y += -gforce;
        //    translate *= AdjustedSpeed;
        }
        return translate;
    }

    void resetSpeed(){
        grappling = false;
        grappler.resetSuccessCounter();
        speed = initSpeed;
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
