using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move : MonoBehaviour
{
    private int speed;
    // Start is called before the first frame update
    void Start()
    {
        speed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        // move to the right by default
        float AdjustedSpeed = speed * Time.deltaTime;
        Vector3 newPos = new Vector3(transform.position.x + AdjustedSpeed,transform.position.y,1); 
        transform.position = newPos;


        // check if the player is jumping
        if(Input.GetKeyDown(KeyCode.Space)){

        }


        // check if the player is on the ground

        // if()
    }

     
}
