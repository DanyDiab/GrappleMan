using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif


public class MainMenu : MonoBehaviour
{
    public Button play;
    public Button tutorial;
    public Button settings;
    public Button quit;
    // Start is called before the first frame update
    void Start()
    {
        play.onClick.AddListener(playClicked);
        tutorial.onClick.AddListener(tutorialClicked);
        settings.onClick.AddListener(settingsClicked);
        quit.onClick.AddListener(quitClicked);

    }

    void playClicked(){
        SceneManager.LoadScene("Game");   
    }

    void tutorialClicked(){
        Debug.Log("tutorial");
    }
    void settingsClicked(){
        Debug.Log("settings");
    }

    void quitClicked(){
        #if UNITY_EDITOR
            EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
