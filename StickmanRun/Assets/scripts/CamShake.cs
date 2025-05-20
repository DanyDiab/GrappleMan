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

    // Start is called before the first frame update

    void Start()
    {
        cam = GetComponent<Camera>();
        shakeMagnitude = .02f;
        shakeDuration = .2f;
        grapple = FindAnyObjectByType<Grapple>();
        grapple.OnGrapple += camShake;
        
        
    }
    // Update is called once per frame

    void camShake()
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
            float camX = randX + transform.position.x;
            float camY = randY + transform.position.y;
            timeElapsed += Time.deltaTime;
            transform.position = new Vector3(camX, camY, -1);
            yield return null;
        }
    }
}
