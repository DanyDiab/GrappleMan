using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public Door door;
    SpriteRenderer spriteRenderer;
    public Sprite notPressed;
    public Sprite pressed;


    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = notPressed;
    }

    void OnTriggerStay2D(Collider2D collider2D){
        // ignore collision with the floor
        if(collider2D.gameObject.layer == LayerMask.NameToLayer("Floor") || collider2D.gameObject.tag == "PPBody") return;
        spriteRenderer.sprite = pressed;
        door.startOpen();
    }

    void OnTriggerExit2D(Collider2D collider2D){
        // ignore collision with the floor
        if(collider2D.gameObject.layer == LayerMask.NameToLayer("Floor") || collider2D.gameObject.tag == "PPBody") return;
        spriteRenderer.sprite = notPressed;
        door.startClose();
    }
}
