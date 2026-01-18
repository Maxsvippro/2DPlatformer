using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR;

public class Trap_FallingPlatform : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private BoxCollider2D[] colliders;
    [SerializeField] private float speed = 0.75f;
    [SerializeField] private float travelDistance;
    public Vector3[] wayPoints;
    private int wayPointsIndex;
    private bool canMove = false;

    [Header("PlatformFall Details")]
    [SerializeField] private float impactSpeed = 3;
    [SerializeField] private float impactDuration = 0.1f;
    private float impactTimer;
    private bool impactHappened;
    [Space]
    [SerializeField] private float fallDelay = 0.5f;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        colliders = GetComponents<BoxCollider2D>();
    }

    private void Start()
    {
        wayPointsIndex = 0;
        SetupWayPoint();
        Invoke(nameof(ActivatePlatform), Random.Range(0f, 0.5f));
    }

    private void ActivatePlatform()
    {
        canMove = true;
    }
    private void SetupWayPoint()
    {
        wayPoints = new  Vector3[2];
        float yOffset = travelDistance / 2;
        wayPoints[0] = transform.position + new Vector3(0, yOffset, 0);
        wayPoints[1] = transform.position + new Vector3(0, -yOffset, 0);
    }

    private void Update()
    {
        HandleMovement();
        HandleImpact();
    }

    private void HandleMovement()
    {
        if (canMove == false) return;

        transform.position = Vector2.MoveTowards(transform.position, wayPoints[wayPointsIndex], speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPoints[wayPointsIndex]) < 0.1f)
        {
            wayPointsIndex++;
            if (wayPointsIndex >= wayPoints.Length)
            {
                wayPointsIndex = 0;
            }
        }
        // if (canMove == false) return;
        // if (wayPoints == null || wayPoints.Length == 0) return;
        // if (wayPointsIndex < 0 || wayPointsIndex >= wayPoints.Length) return;

        // transform.position = Vector2.MoveTowards(transform.position,wayPoints[wayPointsIndex],speed * Time.deltaTime);

        // if (Vector2.Distance(transform.position, wayPoints[wayPointsIndex]) < 0.1f)
        // {
        //     wayPointsIndex++;
        //     if (wayPointsIndex >= wayPoints.Length)
        //        { 
        //         wayPointsIndex = 0;
        //        }
        // }
    }

    private void HandleImpact()
    {
        if (impactTimer < 0)
            return;
        impactTimer -= Time.deltaTime;
        Debug.Log("Move platform");
        transform.position = Vector2.MoveTowards(transform.position, transform.position + (Vector3.down *10), impactSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (impactHappened)
            return;
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Invoke(nameof(FallPlatform), fallDelay);
            impactTimer = impactDuration;
            impactHappened = true;
        }
    }

    private void FallPlatform()
    {
       anim.SetTrigger("deactivate");
       
       canMove = false;
       
       rb.bodyType = RigidbodyType2D.Dynamic;
       rb.gravityScale = 3.5f;
       rb.linearDamping = 0.5f;

         foreach (BoxCollider2D collider in colliders)
         {
              collider.enabled = false;
         }
    }
}
