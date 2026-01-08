using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using NUnit.Framework;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Player : MonoBehaviour
{
   private Rigidbody2D rb;
   private Animator anim;
   
   [Header("Movement")]
   [SerializeField] private float moveSpeed;    
   [SerializeField] private float jumpForce;
   [SerializeField] private float doubleJumpForce;
   public bool canDoubleJump;

   [Header("Wall Jump")]
   [SerializeField] private float wallJumpDuration = 0.65f;
   [SerializeField] private Vector2 wallJumpForce;
   private bool isWallJumping;
   
   [Header("Buffer Jump && Coyote Jump")]
   [SerializeField] private float bufferJumpWindow = 0.25f;
   public float bufferJumpActivated = -1;
   [SerializeField] private float coyoteJumpWindow = 0.5f;
   private float coyoteJumpActivated = -1;

   [Header("Knockback")]
   [SerializeField] private float knockbackDuration = 1;
   [SerializeField] private Vector2 knockbackPower;
   private bool isKnockbacked;
   //private bool canBeKnockbacked;
   private float xInput;
   private float yInput;
   private bool facingRight = true;
   private int facingDirection = 1; 
   
   [Header("Collision")]
   [SerializeField] private float groundCheckDistance;
   [SerializeField] private float wallCheckDistance;
    private bool isGrounded;
    private bool isAirborne;
    private bool isWallDetected;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        //GameManager.Instance = this;
    }
    private void Update()
    {
        if (isKnockbacked)
            return;
        HandleInput();
        HandleColision();
        UpdateAirBorneStatus();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        HandleAnimation();
    }

    public void Knockback()
    {
        if (isKnockbacked)
            return;
        StartCoroutine(KnockbackRoutine());
        anim.SetTrigger("knockback");
        rb.linearVelocity = new Vector2(knockbackPower.x * -facingDirection, knockbackPower.y);
    }

    private IEnumerator KnockbackRoutine()
    {
        //canBeKnockbacked = false;
        isKnockbacked = true;
        yield return new WaitForSeconds(knockbackDuration);
        //canBeKnockbacked = true;
        isKnockbacked = false;
    }
    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && rb.linearVelocity.y < 0; //&& xInput * facingDirection >= 0;
        float yModifer = yInput <0 ? 1 : 0.5f;
        if (canWallSlide == false)
            return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * yModifer);
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpButton();
            RequestBufferJump();
        }
    }

    #region Buffer && Coyote Jump
    private void RequestBufferJump()
    {
        if (isAirborne)
            bufferJumpActivated = Time.time;
    }

    private void AttemptBufferJump()
    {
        if (Time.time - bufferJumpActivated <= bufferJumpWindow)
        {
            bufferJumpActivated = Time.time -1;
            Jump();
        }
    }

    private void ActivateCoyoteJump() => coyoteJumpActivated = Time.time;
    private void CanclelCoyoteJump() => coyoteJumpActivated =Time.time -1;
    #endregion
    #region  
    private void JumpButton()
    {
        bool coyoteJumpAvailable = Time.time - coyoteJumpActivated <= coyoteJumpWindow;
        if (isGrounded || coyoteJumpAvailable)
        {
            Jump();
        }
        else if (isWallDetected && !isGrounded)
        {
            WallJump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }
        CanclelCoyoteJump();
    }
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    private void DoubleJump()
    {   
        isWallJumping = false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
        canDoubleJump = false;
    }
    private void WallJump()
    {
        canDoubleJump = true;
        rb.linearVelocity = new Vector2(wallJumpForce.x * -facingDirection, wallJumpForce.y);
        Flip();
        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine());
    }
    private IEnumerator WallJumpRoutine()
    {   
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
    }
#endregion
    private void HandleColision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, LayerMask.GetMask("Ground"));
        isWallDetected = Physics2D.Raycast(transform.position, Vector2.right * facingDirection, wallCheckDistance, LayerMask.GetMask("Ground"));
    }

    private void HandleAnimation()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWallDetected", isWallDetected);
    }

    private void HandleMovement()
    {
        if (isWallDetected && xInput * facingDirection > 0)
        return;
        if (isWallJumping)
        return;

        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }
    private void HandleFlip()
    {
        if (rb.linearVelocity.x <0 && facingRight || rb.linearVelocity.x >0 && !facingRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0,180,0);
        facingRight = !facingRight;
    }
    private void UpdateAirBorneStatus()
    {
        if (isGrounded  && isAirborne)
        {
            HandleLanding();
        }
        if (!isGrounded && !isAirborne)
        {
            BecomeAirborne();
        }
    }
    private void HandleLanding()
    {
        isAirborne = false;
        canDoubleJump = true;
        AttemptBufferJump();
    }
    private void BecomeAirborne()
    {
        isAirborne = true;
        if (rb.linearVelocity.y < 0)
        { 
            ActivateCoyoteJump();
            //Debug.Log("Coyote Jump Activated");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDirection), transform.position.y));
    }
} 