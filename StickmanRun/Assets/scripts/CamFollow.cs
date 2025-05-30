using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    Camera cameraa;
    public Transform target;
    Vector3 lastFrame;
    float camSize;
    float camDamping;
    float zoomSpeed;
    float minZoom;
    float maxZoom;
    float minSpeed;
    float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        cameraa = GetComponent<Camera>();
        zoomSpeed = 6f;
        camDamping = 3f;
        minZoom = 10f;
        maxZoom = 20f;
        minSpeed = 0f;
        maxSpeed = 100f;
    }
    // Update is called once per frame
    void LateUpdate(){
        Vector3 pos = new Vector3(target.position.x,target.position.y, -1);
        if(transform.position != pos){
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * camDamping);
        }
        // updateCamZoom();
    }



    // .08 = 10
    // .1 = 20

    void updateCamZoom(){
        float moved = (lastFrame - target.transform.position).magnitude;
        lastFrame = target.transform.position;
        // camSize = Mathf.Clamp();
        updateCamSize();
    }

    void updateCamSize(){
        Debug.Log(camSize);
        if(cameraa.orthographicSize < camSize){
            cameraa.orthographicSize += .02f;
        }
        else if(cameraa.orthographicSize > camSize){
            cameraa.orthographicSize -= .02f;
        }
    }
    
}
