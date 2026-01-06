using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
   private Rigidbody2D rb;
   private Animator anim;
   [SerializeField] private float moveSpeed;
   [SerializeField] private float jumpForce;
   [SerializeField] private float doubleJumpForce;
   public bool canDoubleJump;
   private float xInput;
   private float yInput;
   private bool facingRight = true;
   private int facingDirection = 1;
   [SerializeField] private float groundCheckDistance;
   [SerializeField] private float wallCheckDistance;
    private bool isGrounded;
    private bool isAirborne;
    private bool isWallDetected;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        UpdateAirBorneStatus();
        HandleInput();
        HandleWallSlide();
        HandleMovement();
        HandleFlip();
        HandleColision();
        HandleAnimation();
    }

    private void HandleWallSlide()
    {
        bool canWallSlide = isWallDetected && rb.linearVelocity.y < 0;
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
        }
    }
    private void JumpButton()
    {
        if (isGrounded)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }
    }
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    private void DoubleJump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
        canDoubleJump = false;
    }
    

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
        if (isWallDetected)
            return;

        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
    }
    private void HandleFlip()
    {
        if (rb.linearVelocity.x <0 && facingRight || rb.linearVelocity.x >0 && !facingRight)
        {
            Filp();
        }
    }

    private void Filp()
    {
        facingDirection *= -1;
        transform.Rotate(0,180,0);
        facingRight = !facingRight;
    }
    private void UpdateAirBorneStatus()
    {
        if (isGrounded  && isAirborne)
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
    }
    private void BecomeAirborne()
    {
        isAirborne = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (wallCheckDistance * facingDirection), transform.position.y));
    }
}
