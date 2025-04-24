using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private int speed;
    private float gconstant;
    private float gforce;
    private bool jumped; 
    private bool onFloor;

    // Start is called before the first frame update
    void Start()
    {
        speed = 2;
        gconstant = .04f;
        gforce = 0;
    }
    void FixedUpdate(){
        applyGravity();
        // move to the right by default
        if(Input.GetKey(KeyCode.Space) && onFloor){
            jump();
        }
        float AdjustedSpeed = speed * Time.deltaTime;
        Vector3 newPos = new Vector3(transform.position.x + AdjustedSpeed,transform.position.y - gforce,1); 
        transform.position = newPos;
    }


    void applyGravity(){
        if(onFloor){
            gforce = 0;
            return;
        }
        else if(gforce < 2){
            // add an initial gravity force
            if(gforce == 0){
                gforce = .04f;
            }
            // continue adding to the gravity force
            gforce += gconstant * Time.deltaTime;
        }
    }
    void jump(){
        gforce -= .04f;
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
