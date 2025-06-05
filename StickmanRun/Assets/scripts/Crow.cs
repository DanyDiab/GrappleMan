using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;


public enum CrowState{
    Idle,
    Ascending,
    Descending,
}
public class Crow : MonoBehaviour
{
    float speed;
    Rigidbody2D rb;
    public Transform start;
    public Transform mid;
    public Transform end;
    Vector2 endPos;
    Vector2 startPos;
    Vector2 midPos;
    Vector2 currPos;
    CrowState currState;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 dir;
    public LayerMask collisionMask;
    Vector2 lastPos;
    public Rigidbody2D bodyRb;
    MultiHitbox bodyHitbox;

    

    // Start is called before the first frame update


    void Awake(){
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        bodyHitbox = GetComponentInChildren<MultiHitbox>();
        rb = GetComponentInChildren<Rigidbody2D>();
        bodyRb.transform.parent = null;

    }
    void Start()
    {
        speed = .5f;
        endPos = end.transform.position;
        startPos = start.transform.position;
        currState = CrowState.Idle;
        midPos = mid.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate(){

        switch(currState){
            case CrowState.Idle:
                currPos = bodyRb.position;
                break;
            case CrowState.Ascending:
                moveTowards(midPos);
                break;
            case CrowState.Descending:
                moveTowards(endPos);
                break;
        }
    }
    void Update()
    {
        flipSprite();
        rayCastCollide();
    }

    void LateUpdate(){
        checkForCollision();
    }


    // returns true if reaches target and false otherwise
    void moveTowards(Vector2 target){
        dir = (target - bodyRb.position).normalized;
        bodyRb.position += dir * speed;     
    }

    void flipSprite(){
        if(dir.x > 0){
            spriteRenderer.flipX = false;
        }
        else{
            spriteRenderer.flipX = true;
        }
    }

    void rayCastCollide()
    {
        RaycastHit2D hit = Physics2D.Linecast(lastPos, bodyRb.position, collisionMask);
        if(hit.collider != null){
            if(hit.collider.CompareTag("CrowMid") && currState == CrowState.Ascending){
                currState = CrowState.Descending;
            }
            if(hit.collider.CompareTag("CrowEnd") && currState == CrowState.Descending){
                Transform temp = start;
                start = end;
                end = temp;
                start.tag = "CrowStart";
                end.tag = "CrowEnd";
                startPos = start.transform.position;
                endPos = end.transform.position;
                midPos = mid.transform.position;
                dir *= -1;
                animator.SetBool("isFlying", false);
                currState = CrowState.Idle;
            }
        }
        lastPos = bodyRb.position;
    }

    void checkForCollision(){
        if(bodyHitbox.isTriggered() && currState == CrowState.Idle){
            bodyHitbox.resetTrigger();
            Debug.Log("triggered!");
            currState = CrowState.Ascending;
            animator.SetBool("isFlying", true);
            return;
        } 
        bodyHitbox.resetTrigger();
    }



}
