using UnityEngine;

public class Enemy_Mushroom : Enemy
{   
    protected override void Update()
    {
        base.Update();
        anim.SetFloat("xVelocity", rb.linearVelocity.x);

        HandleColision();
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
