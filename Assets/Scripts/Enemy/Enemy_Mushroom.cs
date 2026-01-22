using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_Mushroom : Enemy
{   
    private BoxCollider2D cd;
    protected override void Update()
    {
        base.Update();
        anim.SetFloat("xVelocity", rb.linearVelocity.x);

        if (isDead)
            return;
        HandleCollision();
        HandleMovement();
        if (isGrounded)
            HandleTurnAround();
    }

    private bool HandleTurnAround()
    {
        if (!isGroundInFrontDetected || isWallDetected)
        {
            Flip();
            idleTimer = idleDuration;
            rb.linearVelocity = Vector2.zero;
        }

        return true;
    }

    private void HandleMovement()
    {
        if (idleTimer > 0)
            return;  
        rb.linearVelocity = new Vector2(moveSpeed * facingDirection, rb.linearVelocity.y);
    }   

}
