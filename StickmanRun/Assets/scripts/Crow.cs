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
    public Transform end;
    public Transform start;
    public Transform mid;
    Vector2 endPos;
    Vector2 startPos;
    Vector2 midPos;
    Vector2 currPos;
    CrowState currState;
    Animator animator;
    Hover hover;
    

    // Start is called before the first frame update
    void Start()
    {
        speed = .5f;
        endPos = end.position;
        startPos = start.position;
        currState = CrowState.Idle;
        rb = GetComponent<Rigidbody2D>();
        midPos = mid.position;
        animator = GetComponentInChildren<Animator>();
        hover = GetComponent<Hover>();
        hover.endFloat();
    }

    // Update is called once per frame
    void FixedUpdate(){
        switch(currState){
            case CrowState.Idle:
                currPos = rb.position;
                break;
            case CrowState.Ascending:
                hover.startFloat();
                animator.SetBool("isFlying", true);
                if(moveTowards(midPos)){
                    currState = CrowState.Descending;
                }
                break;
            case CrowState.Descending:
                if(moveTowards(endPos)){
                    hover.endFloat();
                    Vector2 startTemp = startPos;
                    startPos = endPos;
                    endPos = startTemp;
                    animator.SetBool("isFlying", false);
                    currState = CrowState.Idle;
                }
                break;
        }
    }


// returns true if reaches target and false otherwise
    bool moveTowards(Vector2 target){
        Vector2 dir = (target - rb.position).normalized;
        rb.position += dir * speed;
        if((rb.position - target).magnitude < .5f){

            return true;
        }
        return false;
                
    }


    void OnTriggerEnter2D(Collider2D collision){
        Debug.Log(tag);
        if((collision.CompareTag("Grappler") || collision.CompareTag("Player")) && currState == CrowState.Idle)
        {
            currState = CrowState.Ascending;
        }
        
    }
}
