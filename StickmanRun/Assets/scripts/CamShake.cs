using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CamShake : MonoBehaviour
{

    Camera cam;
    float shakeDuration;
    float shakeMagnitude;
    Grapple grapple;
    Vector3 originalPos;

    // Start is called before the first frame update

    void Start()
    {
        cam = GetComponent<Camera>();
        shakeMagnitude = .1f;
        shakeDuration = .2f;
        grapple = FindAnyObjectByType<Grapple>();
        grapple.OnGrapple += camShake;
        
        
    }
    // Update is called once per frame

    public void camShake()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float timeElapsed = 0f;
        while (timeElapsed < shakeDuration)
        {
            
            float randX = Random.Range(-1, 1) * shakeMagnitude;
            float randY = Random.Range(-1, 1) * shakeMagnitude;
            float camX = randX + originalPos.x;
            float camY = randY + originalPos.y;
            timeElapsed += Time.deltaTime;
            transform.position = new Vector3(camX, camY, -1);
            yield return null;
        }
    }
}
