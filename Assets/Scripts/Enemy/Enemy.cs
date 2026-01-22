using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;

    [SerializeField]protected GameObject damageTrigger;
    [Space]

    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float idleDuration = 1.5f;
    protected float idleTimer;

    [Header("Death details")]
    [SerializeField] private float deathImpactSpeed = 5;
    [SerializeField] private float deathRotationSpeed =  150;
    private int deathRotationDirection = 1;
    protected bool isDead;


    [Header("Basic collision settings")] 
    [SerializeField] protected float groundCheckDistance = 1.1f;
    [SerializeField] protected float wallCheckDistance = 0.7f;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform groundCheck;
    protected bool isGrounded;
    protected bool isWallDetected;
    protected bool isGroundInFrontDetected;

    protected int facingDirection = -1;
    protected bool facingRight = false;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        idleTimer -= Time.deltaTime;

        if (isDead)
        {
            HandleDeathRotation();
        }
    }

    public virtual void Die()
    {
        col.enabled = false;
        damageTrigger.SetActive(false);
        anim.SetTrigger("hit");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, deathImpactSpeed);
        isDead = true;

        if (Random.Range(0, 100) < 50)
            deathRotationDirection = deathRotationDirection * -1;
    }
    private void HandleDeathRotation()
    {
        transform.Rotate(0, 0, (deathRotationSpeed * deathRotationDirection) * Time.deltaTime); 
    }
    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isGroundInFrontDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, LayerMask.GetMask("Ground"));
    }
    protected virtual void HandleFlip(float xValue)
    {
        if (Mathf.Abs(xValue) > 0)
        {
            if (xValue < 0 && facingRight || xValue > 0 && !facingRight)
            {
                Flip();
            }
        }
        else 
        { 
            if (Mathf.Abs(rb.linearVelocity.x) > 0.1f && !isWallDetected)
            {
                if (rb.linearVelocity.x < 0 && facingRight || rb.linearVelocity.x > 0 && !facingRight)
                {
                    Flip();
                }
            }
        }
    }   
    protected virtual void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0,180,0);
        facingRight = !facingRight;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDirection), transform.position.y));
    }
    
}
