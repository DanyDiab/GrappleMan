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
    Valve[] valves;
    WaterPuzzleState currState;
    PrefabSpawner bubbleSpawner;
    // Start is called before the first frame update
    void Start()
    {
        currState = WaterPuzzleState.Idle;
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
                break;
            case WaterPuzzleState.Complete:
                Debug.Log("Complete");
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
