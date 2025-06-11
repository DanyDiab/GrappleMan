using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    [SerializeField]float speed;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        speed = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.up * speed;
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("BubbleKillBox")){
            Destroy(gameObject);
        }
    }
}
