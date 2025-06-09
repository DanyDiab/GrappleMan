using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    static bool checkInputs;
    static float currTime;
    static bool timerStarted;
    static bool enable;
    static float time;
    // Start is called before the first frame update
    void Start()
    {
        checkInputs = true;   
    }

    void Update(){
        if(timerStarted){
            toggleInput(enable,time);
        }
    }


    public bool getKeyDown(KeyCode keyToCheck){
        return Input.GetKey(keyToCheck) && checkInputs;
    }
    public bool getMouseDown(int mouseToCheck){
        return Input.GetMouseButton(mouseToCheck) && checkInputs;
    }
    public bool getKeyPressed(KeyCode keyToCheck){
        return Input.GetKeyDown(keyToCheck) && checkInputs;
    }
    
    public bool getMousePressed(int mouseToCheck){
        return Input.GetMouseButtonDown(mouseToCheck) && checkInputs;
    }


    public static void toggleInput(bool enabled){
        checkInputs = enabled;
    }

    public static void toggleInput(bool enabled, float time){
        timerStarted = true;
        enable = enabled;
        Inputs.time = time;
        checkInputs = enabled;
        if(currTime == 0f){
            currTime = Time.time;
        }
        if(Time.time - currTime >= time){
            currTime = 0f;
            timerStarted = false;
            checkInputs = !enabled;
        }
    }
}
