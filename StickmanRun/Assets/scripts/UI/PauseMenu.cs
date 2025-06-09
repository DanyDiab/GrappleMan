
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif


public enum PauseState{
    Playing, 
    Paused,
    Settings,
    Quit
}
public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button resume;
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
        resume.onClick.AddListener(resumeClicked);
        settings.onClick.AddListener(settingsClicked);
        quit.onClick.AddListener(quitClicked);
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
                if(menuInteract) currState = PauseState.Playing;
                Debug.Log("settings");
                break;
            case PauseState.Quit:
                #if UNITY_EDITOR
                    EditorApplication.isPlaying = false;
                #else
        
                    Application.Quit();
                #endif
                break;
        }
    }
    void toggleMenu(bool enable){
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(enable);
        }
    }

    void OnMouseDown(){

    }

    void resumeClicked(){
        if(currState == PauseState.Paused){
            currState = PauseState.Playing;
        }
    }

    void settingsClicked(){
        if(currState == PauseState.Paused){
            currState = PauseState.Settings;
        }
        else if(currState == PauseState.Settings){
            currState = PauseState.Paused;
        }
    }

    void quitClicked(){
        if(currState == PauseState.Paused){
            currState = PauseState.Quit;
        }
    }




}
