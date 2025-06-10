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

    public float toSpeed;
    public float backSpeed;
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
    MultiHitbox bodyHitbox;



    // Start is called before the first frame update

    void Awake(){
        bodyHitbox = GetComponentInChildren<MultiHitbox>();
        bodyRb.transform.parent = null;
    }
    void Start()
    {
        startPos = startTrans.position;
        endPos = endTrans.position;
        currState = ZipLineState.Idle;
        // rb = GetComponent<Rigidbody2D>();
        // bodyRb = Get<Rigidbody2D>();
        lastPos = bodyRb.position;
        lineRenderer = GetComponentInChildren<LineRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
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
        drawLine();
        checkForCollision();
    }

    void move(bool towards, float speed)
    {
        if (towards) moveDir = (endPos - startPos).normalized;
        else moveDir = (startPos - endPos).normalized;
        adjustedDir = moveDir * speed * Time.deltaTime;
        bodyRb.MovePosition(bodyRb.position + adjustedDir);
    }

    void rayCastCollide()
    {
        RaycastHit2D hit = Physics2D.Linecast(lastPos, bodyRb.position, collisionMask);
        if (hit.collider != null){
            if (hit.collider.CompareTag("ZipEnd") && currState == ZipLineState.To)
            {
                currState = ZipLineState.Back;
            }
            else if (hit.collider.CompareTag("ZipStart") && currState == ZipLineState.Back)
            {
                currState = ZipLineState.Idle;
            }
        }
        lastPos = bodyRb.position;
    }

    void checkForCollision(){
        if (bodyHitbox.isTriggered() && currState == ZipLineState.Idle){
            currState = ZipLineState.To;
            bodyHitbox.resetTrigger();
            return;
        }
        bodyHitbox.resetTrigger();

    }



    void drawLine(){
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);

    }
}
