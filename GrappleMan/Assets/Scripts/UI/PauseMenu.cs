
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public Button mainMenu;
    public Button quit;
    PauseState currState;
    bool menuInteract;
    Crosshair crosshair;

    
    // Start is called before the first frame update
    void Start()
    {
        currState = PauseState.Playing;
        resume.onClick.AddListener(resumeClicked);
        settings.onClick.AddListener(settingsClicked);
        quit.onClick.AddListener(quitClicked);
        mainMenu.onClick.AddListener(mainMenuClicked);
        crosshair = FindFirstObjectByType<Crosshair>();

    }

    // Update is called once per frame
    void Update()
    {
        menuInteract = Input.GetKeyDown(KeyCode.Escape);
    
        switch(currState){
            case PauseState.Playing:
                if(menuInteract) currState = PauseState.Paused;
                Time.timeScale = 1f;
                toggleMenu(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
                crosshair.gameObject.SetActive(true);
                break;
            case PauseState.Paused:
                Time.timeScale = 0f;
                if(menuInteract){
                    Inputs.toggleInput(true);
                    currState = PauseState.Playing;
                    return;
                }
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                crosshair.gameObject.SetActive(false);
                Inputs.toggleInput(false);
                toggleMenu(true);
                break;
            case PauseState.Settings:
                if(menuInteract) {
                    Inputs.toggleInput(true);
                    currState = PauseState.Playing;
                    return;
                }
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

    void resumeClicked(){
        if(currState == PauseState.Paused){
            Inputs.toggleInput(true);
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

    void mainMenuClicked(){
        if(currState == PauseState.Paused){
            SceneManager.LoadScene("MainMenu");
        }
    }




}
