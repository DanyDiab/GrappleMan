using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaterPuzzleState{
    Idle,
    Complete
}

public class WaterPuzzle : MonoBehaviour
{
    public Transform head;
    CutScene cutScene;
    Valve[] valves;
    WaterPuzzleState currState;
    PrefabSpawner bubbleSpawner;
    bool startedCutScene;
    // Start is called before the first frame update
    void Start()
    {
        cutScene = Camera.main.GetComponent<CutScene>();
        currState = WaterPuzzleState.Complete;
        valves = GetComponentsInChildren<Valve>();
        bubbleSpawner = GetComponentInChildren<PrefabSpawner>();

        bubbleSpawner.setSpawning(false);
    }

    // Update is called once per frame
    void Update()
    {

        switch(currState){
            case WaterPuzzleState.Idle:
                if(checkForValveCompletions()){
                    currState = WaterPuzzleState.Complete;
                }
                bubbleSpawner.setSpawning(false);
                break;
            case WaterPuzzleState.Complete:
                if(!startedCutScene){
                    startedCutScene = true;
                    cutScene.startCutScene(head.position, 2f);
                    
                }
                bubbleSpawner.setSpawning(true);
                bubbleSpawner.toggleAuto(true);
                bubbleSpawner.setAutoSpawnVars(1f,head,1);
                break;

        }
    }


    bool checkForValveCompletions(){
        for(int i = 0; i < valves.Length; i++){
            if(!valves[i].isCompleted()){
                return false;
            }
        }
        return true;
    }
}
