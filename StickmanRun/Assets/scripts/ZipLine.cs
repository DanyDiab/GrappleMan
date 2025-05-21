using Unity.VisualScripting;
using UnityEngine;

public enum ZipLineState
{
    Idle,
    To,
    Back,

}

public class ZipLine : MonoBehaviour
{

    float toSpeed;
    float backSpeed;
    public Transform startTrans;
    public Transform endTrans;
    Vector3 startPos;
    Vector3 endPos;
    Vector2 moveDir;
    Vector2 adjustedDir;
    Rigidbody2D rb;
    public Rigidbody2D bodyRb;
    ZipLineState currState;
    public LayerMask collisionMask;
    Vector2 lastPos;
    LineRenderer lineRenderer;



    // Start is called before the first frame update
    void Start()
    {
        startPos = startTrans.position;
        endPos = endTrans.position;
        currState = ZipLineState.Idle;
        // rb = GetComponent<Rigidbody2D>();
        // bodyRb = Get<Rigidbody2D>();
        bodyRb.transform.parent = null;
        toSpeed = 15f;
        backSpeed = 5f;
        lastPos = bodyRb.position;
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(currState);
        switch (currState)
        {
            case ZipLineState.To:
                move(true, toSpeed);
                break;
            case ZipLineState.Back:
                move(false, backSpeed);
                break;
            default:
                break;
        }
    }
    void Update()
    {
        rayCastCollide();
        // drawLine();
    }

    void move(bool towards, float speed)
    {
        if (towards) moveDir = (endPos - startPos).normalized;
        else moveDir = (startPos - endPos).normalized;
        adjustedDir = moveDir * speed * Time.deltaTime;
        bodyRb.MovePosition(bodyRb.position + adjustedDir);
    }


    // 5/21/25 known bug: 
    // if grappler is attached, other hitboxes arent triggered allowing for the zipline to continue moving passed the end 
    void rayCastCollide()
    {
        RaycastHit2D hit = Physics2D.Linecast(lastPos, bodyRb.position, collisionMask);
        if (hit.collider != null)
        {
            Debug.Log(hit.collider.tag);
            if (hit.collider.tag == "Grappler" && currState == ZipLineState.Idle)
            {
                currState = ZipLineState.To;
            }
            else if (hit.collider.tag == "ZipEnd" && currState == ZipLineState.To)
            {
                currState = ZipLineState.Back;
            }
            else if (hit.collider.tag == "ZipStart" && currState == ZipLineState.Back)
            {
                currState = ZipLineState.Idle;
            }
        }
        lastPos = bodyRb.position;
    }



    void drawLine()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

    }
}
