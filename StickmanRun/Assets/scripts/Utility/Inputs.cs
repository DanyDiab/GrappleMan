using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inputs : MonoBehaviour
{
    static bool checkInputs;
    // Start is called before the first frame update
    void Start()
    {
        checkInputs = true;   
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


    public void toggleInput(bool enabled){
        checkInputs = enabled;
    }
}
