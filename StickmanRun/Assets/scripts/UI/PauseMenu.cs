using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public enum PauseState{
    Playing, 
    Paused,
    Settings,
    Quit
}
public class PauseMenu : MonoBehaviour
{
    public Button resume;
    public Button settings;
    public Button quit;
    PauseState currState;
    bool menuInteract;
    Inputs inputs;

    
    // Start is called before the first frame update
    void Start()
    {
        inputs = GetComponent<Inputs>();
        currState = PauseState.Playing;
    }

    // Update is called once per frame
    void Update()
    {
        menuInteract = Input.GetKeyDown(KeyCode.Escape);
    
        switch(currState){
            case PauseState.Playing:
                if(menuInteract) currState = PauseState.Paused;

                inputs.toggleInput(true);
                toggleMenu(false);
                break;
            case PauseState.Paused:
                if(menuInteract) currState = PauseState.Playing;
                inputs.toggleInput(false);
                toggleMenu(true);
                break;
            case PauseState.Settings:
                break;
            case PauseState.Quit:
                break;
        }
    }
    void toggleMenu(bool enable){
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(enable);
        }
    }

}
