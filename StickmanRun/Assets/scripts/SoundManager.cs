using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    float minPitch;
    float maxPitch;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        minPitch = .9f;
        maxPitch = 1.1f;
    }

    // Update is called once per frame
    public void playSound(AudioClip clip){
        float randPitch = Random.Range(minPitch,maxPitch);
        audioSource.pitch = randPitch;
        audioSource.PlayOneShot(clip);
    }
}
