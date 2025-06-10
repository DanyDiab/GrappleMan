using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AirTime : MonoBehaviour
{
    float startTime;
    float endTime;
    float totalTime;
    bool startedAir;

    public void startAir()
    {
        if (startedAir) return;        
        startedAir = true;
        startTime = Time.time;
    }

    public void endAir()
    {
        if (!startedAir) return;

        endTime = Time.time;
        totalTime = endTime - startTime;
        startedAir = false;        
        
    }

    public float getAirTime()
    {
        return totalTime;
    }
    public bool getStartedAir()
    {
        return startedAir;
    }

    public void setStartedAir(bool startedAir)
    {
        this.startedAir = startedAir;
    }



}