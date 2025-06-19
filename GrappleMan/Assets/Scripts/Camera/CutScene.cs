using UnityEngine;


public enum CutSceneState{
    Idle,
    InProgress,
    Return
}
public class CutScene : MonoBehaviour
{
    CamFollow camFollow;
    bool start;
    CutSceneState currState;
    Vector2 targetPos;
    float currTime;
    float totalTime;
    Transform og;
    // Start is called before the first frame update
    void Start()
    {
        currState = CutSceneState.Idle;
        camFollow = GetComponent<CamFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currState){
            case CutSceneState.Idle:
                if(start){
                    currState = CutSceneState.InProgress;
                }
                break;
            case CutSceneState.InProgress:
                transform.position = targetPos;
                camFollow.setTarget(transform);
                currTime += Time.deltaTime;
                if(currTime > totalTime){
                    currState = CutSceneState.Return;
                }
                break;
            case CutSceneState.Return:
                camFollow.setTarget(og);
                start = false;
                break;
            
        }
    }

    public void startCutScene(Vector3 pos, float time){
        pos.z = 1;
        start = true;
        targetPos = pos;
        totalTime = time;
        og = camFollow.getTarget();
    }
}
