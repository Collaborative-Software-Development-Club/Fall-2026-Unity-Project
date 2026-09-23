using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CADE_PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float sprintSpeed = 10f;

    [Header("Jumping")]
    [SerializeField]
    private float jumpHeight = 10f;
    [SerializeField] //added in a way so we can give player as many jumps as we want
    private int extraJumps = 1; //I think we can link this to an item to give them the jump but im not exactly sure. 
    private int extraJumpsRemaining = 0;

    [Header("Dashing")] 
    [SerializeField] 
    private float dashForce = 5;
    [SerializeField] 
    private float dashCooldown = 0.5f;

    [Header("Ground Check")]
    [SerializeField]
    private Vector2 boxSize;
    [SerializeField]
    private float castDistance;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField] private float coyoteTime;

    [Header("Wall Check")]
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private float wallJumpForceX = 8f;
    [SerializeField] private float wallJumpForceY = 10f;
    [SerializeField] private float wallJumpLockTime = 0.15f;
    private bool isTouchingWallRight;
    private bool isTouchingWallLeft;
    private bool isWallSliding;
    private float wallJumpLockCounter;




    [Header("Gravity")]
    [SerializeField]
    private float baseGravity = 1;
    [SerializeField]
    private float maxFallSpeed = 18;
    [SerializeField]
    private float fallSpeedMultipler = 2f;

    private bool jumpReleased;
    private float horizontalMovement;
    private float finalSpeed;
    private float verticalMovement;
    private float nextDashTime = 0f;
    private bool isDashing;
    private float coyoteTimeCounter = 0f;

    void Start()
    {
        finalSpeed = moveSpeed;
    }

    private void Update() {
        if (isGrounded()) {
            coyoteTimeCounter = coyoteTime;
            extraJumpsRemaining = extraJumps;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }

        isTouchingWallLeft = checkWall(Vector2.left);
        isTouchingWallRight = checkWall(Vector2.right);

        bool pressingOnWall = (isTouchingWallRight && horizontalMovement > 0f) || (isTouchingWallLeft && horizontalMovement < 0f);

        isWallSliding = !isGrounded() && pressingOnWall && rb.linearVelocityY < 0f;

        if (wallJumpLockCounter > 0f) { 
            wallJumpLockCounter -= Time.deltaTime;
        }

        //can take this out if we don't want jump refresh on wall jumps
        if (isWallSliding) {
            extraJumpsRemaining = extraJumps;
        }

    }

    void FixedUpdate() {
        if (isDashing) return;

        if (wallJumpLockCounter <= 0f)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * finalSpeed, rb.linearVelocityY);
        }

        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocityY, -wallSlideSpeed));
        }
        else
        {
            Gravity();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
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
            else if (extraJumpsRemaining > 0)
            {
                jumpReleased = false;
                rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);
                extraJumpsRemaining -= 1;
            }
        }
        else if (context.canceled && !jumpReleased)
        {
            jumpReleased = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            finalSpeed = sprintSpeed;
        }
        else if (context.canceled)
        {
            finalSpeed = moveSpeed;
        }
    }

    public void Dash(InputAction.CallbackContext context) {
        if (context.performed && Time.time >= nextDashTime) {
            StartCoroutine(PerformDash(horizontalMovement));
            nextDashTime = Time.time + dashCooldown;
        }
    }

    private IEnumerator PerformDash(float dir) {
        isDashing = true;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        
        float dashDir = horizontalMovement > 0 ? 1 : -1;
        rb.AddForce(new Vector2(dashDir * dashForce, 0f), ForceMode2D.Impulse);

        if (dir > 0) {
            rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
        } else {
            rb.AddForce(-transform.right * dashForce, ForceMode2D.Impulse);
        }
        
        yield return new WaitForSeconds(0.1f);
        
        rb.gravityScale = baseGravity * fallSpeedMultipler;
        isDashing = false;
    }

    private void Gravity()
    {
        if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultipler;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocityY, -maxFallSpeed));
        }
        else {
            rb.gravityScale = baseGravity;
        }
    }

    private bool isGrounded()
    {
        // if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        // {
        //     return true;
        // }
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, castDistance, groundLayer);
        if (hit) {
            return true;
        } else {
            return false;
        }
    }

    private bool checkWall(Vector2 dir) {
        RaycastHit2D hitWall = Physics2D.Raycast(transform.position, dir, wallCheckDistance, wallLayer);
        Debug.DrawRay(transform.position, dir * wallCheckDistance, hitWall ? Color.green : Color.red);
        if (hitWall)
            Debug.Log($"Wall hit: {hitWall.collider.name} on layer {LayerMask.LayerToName(hitWall.collider.gameObject.layer)}");
        return hitWall;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * wallCheckDistance);
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * wallCheckDistance);
    }

}
