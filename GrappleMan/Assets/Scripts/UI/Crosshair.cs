using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    SpriteRenderer sprite;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        trackMouse();
    }

    void trackMouse(){
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
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
