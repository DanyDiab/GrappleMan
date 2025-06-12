using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    Camera cameraa;
    [SerializeField] Transform target;
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
        Vector3 pos = new Vector3(target.position.x,target.position.y, -1);
        transform.position = pos;
    }
    // Update is called once per frame
    void LateUpdate(){
        Vector3 pos = new Vector3(target.position.x,target.position.y, -1);
        if(transform.position != pos){
            transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * camDamping);
        }
    }

    public Transform getTarget(){
        return target;
    }
    public void setTarget(Transform transform){
        target = transform;
    }
    
}
