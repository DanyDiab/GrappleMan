using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    bool isPulled;
    public bool isStuck;
    Rigidbody2D rb;
    float gScale;
    // Start is called before the first frame update


    void Start()
    {
        gScale = 2;
        rb = GetComponent<Rigidbody2D>();
    }
    void LateUpdate()
    {
        if (isPulled && isStuck)
        {
            isStuck = false;
            rb.gravityScale = gScale;
            return;
        }
        if (isStuck)
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        } 
    }

    public void setIsPulled(bool pulled)
    {
        isPulled = pulled;
    }

    public Rigidbody2D getRb(){
        return rb;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // if the box collides with the player transfer momentum? could be a fun idea
        if(collision.gameObject.layer == LayerMask.NameToLayer("Floor")){
            if(!isPulled){
                rb.velocity = Vector2.zero;
            }
        }
    }
}
