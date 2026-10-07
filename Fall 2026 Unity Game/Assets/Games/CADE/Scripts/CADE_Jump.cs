using UnityEngine;
using UnityEngine.InputSystem;

public static class CADE_Jump
{
    public static void Jump
        (
            Rigidbody2D rb, float jumpHeight, ref float coyoteTimeCounter, ref bool jumpReleased
        )
    {
        if (coyoteTimeCounter > 0f)
        {
            jumpReleased = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);
            coyoteTimeCounter = 0;
        }
    }

    public static int Double_Jump
        (
            Rigidbody2D rb, float jumpHeight, ref float coyoteTimeCounter, ref bool jumpReleased, 
            ref int extraJumpsRemaining
        )
    {
        if (coyoteTimeCounter > 0f)
        {
            jumpReleased = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);
            coyoteTimeCounter = 0;
        }
        else if (extraJumpsRemaining > 0)
        {
            jumpReleased = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);
            extraJumpsRemaining -= 1;
        }

        return extraJumpsRemaining;
    }

    public static void Wall_Jump
        (
            Rigidbody2D rb, float jumpHeight, ref float coyoteTimeCounter, ref bool jumpReleased,
            ref int extraJumpsRemaining, ref int extraJumps, ref float wallJumpLockCounter, ref float wallJumpLockTime,
            float wallJumpForceX, float wallJumpForceY, bool isWallSliding, bool isTouchingWallLeft, bool isTouchingWallRight
        )
    {
        if (coyoteTimeCounter > 0f)
        {
            jumpReleased = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);
            coyoteTimeCounter = 0;
        }
        else if (isWallSliding || isTouchingWallLeft || isTouchingWallRight)
        {
            jumpReleased = false;
            float pushDir = isTouchingWallRight ? -1f : 1f;
            rb.linearVelocity = new Vector2(pushDir * wallJumpForceX, wallJumpForceY);
            wallJumpLockCounter = wallJumpLockTime;
            extraJumpsRemaining = extraJumps; //double jump refresh
        }
    }

    public static void Cut_Jump(Rigidbody2D rb, ref bool jumpReleased) {
        if (!jumpReleased) {
            jumpReleased = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
        }
    }
}
