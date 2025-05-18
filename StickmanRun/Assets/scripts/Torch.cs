using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum torchState
{
    Increasing,
    Decreasing
}

public class Torch : MonoBehaviour
{
    Light2D light;
    float minIntense;
    float maxIntense;
    float flickerSpeed;
    torchState currState;
    float targetIntense;
    // Start is called before the first frame update
    void Start()
    {
        currState = torchState.Increasing;
        minIntense = 1.4f;
        maxIntense = 3f;
        targetIntense = maxIntense;
        light = GetComponentInChildren<Light2D>();
        flickerSpeed = 2f;
        light.intensity = minIntense;

    }

    // Update is called once per frame
    void Update(){
        switch (currState)
        {
            case torchState.Increasing:
                flickerLight(true);
                if (light.intensity >= targetIntense)
                {
                    pickNewTarget();
                }
                break;
            case torchState.Decreasing:
                flickerLight(false);
                if (light.intensity <= targetIntense)
                {
                    pickNewTarget();
                }
                break;
        }
    }

    void pickNewTarget()
    {
        targetIntense = Random.Range(minIntense, maxIntense);
        if (targetIntense >= light.intensity){
            currState = torchState.Increasing;
            return;
        }
        currState = torchState.Decreasing;
        return;
    }

    


    void flickerLight(bool increase)
    {
        if (increase)
        {
            light.intensity += flickerSpeed * Time.deltaTime;
            return;
        }
        light.intensity -= flickerSpeed * Time.deltaTime;
    }
}
