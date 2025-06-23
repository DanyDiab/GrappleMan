using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pullable : MonoBehaviour
{
    bool isPulled;
    public bool isStuck;
    Rigidbody2D rb;
    Grapple grapple;
    Rigidbody2D grappleRB;
    Vector2 posRelativeToGrapple;
    SpringJoint2D springJoint;


    [SerializeField] float springForce = 1000f;
    [SerializeField] float dampingRatio = 0.7f;
    [SerializeField] float maxDistance = 5f;
    // Start is called before the first frame update


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 2;
        posRelativeToGrapple = Vector2.zero;
        grapple = FindAnyObjectByType<Grapple>();
        grappleRB = grapple.getRB();
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

    void Update(){
        if(isPulled){
            if(posRelativeToGrapple == Vector2.zero){
                startPull();
            }
            Vector2 newPos = grappleRB.position + posRelativeToGrapple;
            rb.MovePosition(newPos);
            return;
        }
        stopPull();
    }


    public void startPull(){
        posRelativeToGrapple = grappleRB.position - rb.position;
        springJoint = gameObject.AddComponent<SpringJoint2D>();
        springJoint.connectedBody = grappleRB;
        springJoint.autoConfigureDistance = false;
        springJoint.distance = Vector2.Distance(transform.position, grappleRB.position);
        springJoint.frequency = springForce;
        springJoint.dampingRatio = dampingRatio;
        springJoint.breakForce = Mathf.Infinity;
    }

    public void stopPull(){
        posRelativeToGrapple = Vector2.zero;
        if (springJoint != null)
        {
            Destroy(springJoint);
            springJoint = null;
        }
    }

    public void setIsPulled(bool pulled)
    {
        isPulled = pulled;
    }

    public Rigidbody2D getRb(){
        return rb;
    }
}
