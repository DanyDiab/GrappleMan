using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    bool isPulled;
    public bool isStuck;
    Rigidbody2D rb;
    // Start is called before the first frame update


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;
        
    }
    void LateUpdate(){
        if (isStuck){
            if (isPulled){
                rb.constraints = RigidbodyConstraints2D.None;
                isStuck = false;
                return;
            }
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            return;
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
