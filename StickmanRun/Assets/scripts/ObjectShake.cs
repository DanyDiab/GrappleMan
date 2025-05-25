using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;


public class ObjectShake : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Vector3 originalPos;
    float shakeDuration;
    float shakeMagnitude;



    void Start()
    {
        shakeMagnitude = .1f;
        shakeDuration = .2f;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    public void objectShake()
    {
        originalPos = transform.position;
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float timeElapsed = 0f;
        while (timeElapsed < shakeDuration)
        {

            float randX = Random.Range(-1, 1) * shakeMagnitude;
            float randY = Random.Range(-1, 1) * shakeMagnitude;
            float newX = randX + originalPos.x;
            float newY = randY + originalPos.y;
            timeElapsed += Time.deltaTime;
            spriteRenderer.transform.position = new Vector3(newX, newY, 0);
            yield return null;
        }

        spriteRenderer.transform.position = originalPos;
    }

}


