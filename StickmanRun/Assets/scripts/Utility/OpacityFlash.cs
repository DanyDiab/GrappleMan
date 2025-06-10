using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpacityFlash : MonoBehaviour
{
    float totalTime;
    float startTime;
    float lastFlashTime;
    float flashingSpeed;
    float opacity;
    float[] ogOpacities;
    bool flashing;
    SpriteRenderer[] spriteRenderers;
    // Start is called before the first frame update
    void Start()
    {
        flashingSpeed = .2f;
    }

    // Update is called once per frame
    void Update()
    {
        if(flashing){
            flash();
        }
    }


    public void startFlash(SpriteRenderer[] sprites, float flashTime, float opacity){
        flashing = true;
        totalTime = flashTime;
        spriteRenderers = sprites;
        startTime = Time.time;
        lastFlashTime = 0f;
        ogOpacities = new float[sprites.Length];
        this.opacity = opacity;
    }

    public void flash(){
        if(Time.time - startTime > totalTime){
            flipSpriteOpacity(true);
            flashing = false;
            return;
        }
        if(Time.time - lastFlashTime > flashingSpeed){

            flipSpriteOpacity(false);
            lastFlashTime = Time.time;
        }

    }

    void flipSpriteOpacity(bool reset){
        for(int i = 0; i < spriteRenderers.Length; i++){
            Color curr = spriteRenderers[i].color;
            if(curr.a != opacity && !reset){
                ogOpacities[i] = curr.a;
                spriteRenderers[i].color = new Color(curr.r,curr.g,curr.b,opacity);
            }
            else{
                spriteRenderers[i].color = new Color(curr.r,curr.g,curr.b,ogOpacities[i]);
            }
        }
    }

    public bool isFlashing(){
        return flashing;
    }
}
