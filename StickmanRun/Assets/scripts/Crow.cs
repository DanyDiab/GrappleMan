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

    // Start is called before the first frame update
    void Start()
    {
        speed = .5f;
        endPos = end.position;
        Debug.Log("end" + endPos);
        startPos = start.position;
        Debug.Log("start" + startPos);
        currState = CrowState.Idle;
        rb = GetComponent<Rigidbody2D>();
        midPos = mid.position;
    }

    // Update is called once per frame
    void FixedUpdate(){
        Debug.Log(currState);
        switch(currState){
            case CrowState.Idle:
                currPos = rb.position;
                break;
            case CrowState.Ascending:
                if(moveTowards(midPos)){
                    currState = CrowState.Descending;
                }
                break;
            case CrowState.Descending:
                if(moveTowards(endPos)){
                    Vector2 startTemp = startPos;
                    startPos = endPos;
                    endPos = startTemp;
                    currState = CrowState.Idle;
                }
                break;
        }
    }


// returns true if reaches target and false otherwise
    bool moveTowards(Vector2 target){
        Vector2 dir = (target - rb.position).normalized;
        Debug.Log("end" + target);
        Debug.Log("at" + rb.position);
        rb.position += dir * speed;
        if((rb.position - target).magnitude < .5f){

            return true;
        }
        return false;
                
    }


    void OnTriggerEnter2D(Collider2D collision){
        if((collision.tag == "Grappler" || collision.tag == "Player") && currState == CrowState.Idle){
            currState = CrowState.Ascending;
        }
        
    }
}
