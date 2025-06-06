using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    float minPitch;
    float maxPitch;
    float currTime;
    bool startedSound;
    float maxTime;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        minPitch = .9f;
        maxPitch = 1.1f;
    }

    void Update(){
        if(startedSound){
            stopLoopedSound();
        }
    }

    // Update is called once per frame
    public void playSound(AudioClip clip){
        float randPitch = Random.Range(minPitch,maxPitch);
        audioSource.pitch = randPitch;
        audioSource.PlayOneShot(clip);
    }
    public void endSounds(){
        audioSource.Stop();
    }


    // play the sound for a given time
    public void loopSound(AudioClip clip, float time){
        startedSound = true;
        maxTime = time;
        float randPitch = Random.Range(minPitch,maxPitch);
        audioSource.pitch = randPitch;
        audioSource.clip = clip;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void stopLoopedSound(){
        currTime += Time.deltaTime;
        if(currTime > maxTime){
            startedSound = false;
            currTime = 0f;
            audioSource.Stop();
            audioSource.loop = false;
            audioSource.pitch = 1f;
        }
    }
}
