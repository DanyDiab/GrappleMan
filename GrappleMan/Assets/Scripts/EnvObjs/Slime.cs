using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D other){
        if(other.CompareTag("Player")){
            Player player = other.GetComponentInParent<Player>();
            player.setOnSlime(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player")){
            Player player = other.GetComponentInParent<Player>();
            player.setOnSlime(false);

        }
    }
}
