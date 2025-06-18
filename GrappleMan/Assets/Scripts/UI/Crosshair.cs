using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    SpriteRenderer sprite;
    Inputs inputs;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        inputs = GetComponent<Inputs>();
        animator = GetComponentInChildren<Animator>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        trackMouse();
        expandShrinkCrosshair();

    }

    void trackMouse(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }
    void expandShrinkCrosshair(){
        if(inputs.getMouseUp(0)){
            animator.ResetTrigger("Shrink");
            animator.SetTrigger("Expand");
        }
        else if(inputs.getMousePressed(0)){
            animator.ResetTrigger("Expand");
            animator.SetTrigger("Shrink");
        }
    }


    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

}
