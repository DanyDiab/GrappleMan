using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : MonoBehaviour
{
    float checkTimer = 0f;
    float checkInterval = 0.1f;
    Player player;
    static bool foundPlayer;
    bool wasOnSlime;
    // Start is called before the first frame update
    void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    // Update is called once per frame
 void Update() {
        checkTimer += Time.deltaTime;
        if(checkTimer >= checkInterval) {
            checkTimer = 0f;
            
            Collider2D myCollider = GetComponentInChildren<Collider2D>();
            ContactFilter2D filter = new ContactFilter2D();
            List<Collider2D> results = new List<Collider2D>();
            
            int count = myCollider.OverlapCollider(filter, results);
            
            bool currentlyOnSlime = false;
            foreach(var col in results) {
                if(col.CompareTag("Player")) {
                    currentlyOnSlime = true;
                    break;
                }
            }
            
            // Only update player state when this slime's state changes
            if(currentlyOnSlime && !wasOnSlime) {
                player.addSlimeContact(); 
            }
            else if(!currentlyOnSlime && wasOnSlime) {
                player.removeSlimeContact();
            }
            
            wasOnSlime = currentlyOnSlime;
        }
    }
}
