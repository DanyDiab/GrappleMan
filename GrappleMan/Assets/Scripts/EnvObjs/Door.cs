using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum doorState{
  Idle,
  Opening,
  Closing,
  FullyOpen,
  FullyClosed
}

public class Door : MonoBehaviour
{
  doorState currState;
  public Collider2D BotCollider;
  public Collider2D TopCollider;
  public Collider2D bodyCollider;
  Rigidbody2D rb;
  float doorSpeed;


    void Start()
    {
        TopCollider.transform.parent = null;
        doorSpeed = 1f;
        rb = GetComponent<Rigidbody2D>();
    }



    void Update()
    {
      switch(currState){
        case doorState.Opening:
          open();
          break;
        case doorState.Closing:
          close();
          break;
        case doorState.FullyOpen:
          rb.velocity = Vector2.zero;
          break;
        case doorState.FullyClosed:
          rb.velocity = Vector2.zero;
          break;
        case doorState.Idle:
          rb.velocity = Vector2.zero;
          break;
      }
    }
    public void startOpen(){
    currState = doorState.Opening;
  }

  public void startClose(){
    if(currState != doorState.FullyClosed){
      currState = doorState.Closing;
    }
  }

  public void open(){
    rb.velocity = Vector2.up * doorSpeed;
  }
  public void close(){
    bodyCollider.enabled = true;
    rb.velocity = Vector2.down * doorSpeed;
  }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(collider2D.gameObject.layer == LayerMask.NameToLayer("Floor") && currState == doorState.Closing){
          currState = doorState.Idle;
        }
        else if(collider2D == BotCollider && currState == doorState.Closing){
          currState = doorState.FullyClosed;
        }
        else if(collider2D == TopCollider && currState == doorState.Opening){
          currState = doorState.FullyOpen;
        }
    }


}
