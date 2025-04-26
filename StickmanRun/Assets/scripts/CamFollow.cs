using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        Vector3 pos = new Vector3(target.position.x,1.5f, -1);
        if(transform.position.x != pos.x){
            transform.position = pos;
        }
    }
}
