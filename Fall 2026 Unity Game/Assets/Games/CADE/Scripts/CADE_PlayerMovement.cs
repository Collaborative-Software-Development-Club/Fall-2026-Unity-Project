using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CADE_PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CADE_ItemManager itemManager;

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
    private float dashForce = 100;
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
    private bool isSticky = false;
    private bool isInvSticky = false;
    private bool isTouchingNorm = false;

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

        if ((wallJumpLockCounter <= 0f && !isInvSticky) || (wallJumpLockCounter <= 0f && isTouchingNorm)) {
            rb.linearVelocity = new Vector2(horizontalMovement * finalSpeed, rb.linearVelocityY);
        }

        if (isWallSliding)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocityY, -wallSlideSpeed));
        }
        else if (!isSticky && !isDashing)
        {
            Gravity();
        }

        if (isSticky)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, verticalMovement * finalSpeed);
        if (isInvSticky && !isTouchingNorm)
            rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
        else
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;

    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (itemManager.doubleJumpFlag)
        {
            CADE_Jump.Double_Jump(context, rb, jumpHeight, ref coyoteTimeCounter, ref jumpReleased, ref extraJumpsRemaining);
        }
        else if (itemManager.wallJumpFlag)
        {
            CADE_Jump.Wall_Jump(context, rb, jumpHeight, ref coyoteTimeCounter, ref jumpReleased,
                ref extraJumpsRemaining, ref extraJumps, ref wallJumpLockCounter, ref wallJumpLockTime,
                wallJumpForceX, wallJumpForceY, isWallSliding, isTouchingWallLeft, isTouchingWallRight);
        }
        else if (!isSticky && !isInvSticky)
        {
            CADE_Jump.Jump(context, rb, jumpHeight, ref coyoteTimeCounter, ref jumpReleased);
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
        if (itemManager.dashFlag || itemManager.inv_DashFlag)
        {
            if (context.performed && Time.time >= nextDashTime)
            {
                nextDashTime = Time.time + dashCooldown;
                if (itemManager.dashFlag)
                    StartCoroutine(PerformDash(horizontalMovement));
                else
                    StartCoroutine(PerformDash(-1));
            }
        }
    }

    private IEnumerator PerformDash(float dir) {
        isDashing = true;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        if (dir > 0 && itemManager.dashFlag) {
            rb.AddForce(transform.right * dashForce, ForceMode2D.Impulse);
        } else {
            if (itemManager.dashFlag)
                rb.AddForce(-transform.right * dashForce, ForceMode2D.Impulse);
            else {
                rb.gravityScale = 0f;
                rb.AddForce(-transform.up * dashForce, ForceMode2D.Impulse);
            }
        }
        
        yield return new WaitForSeconds(0.1f);
        
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, -baseGravity * fallSpeedMultipler);
        Gravity();
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

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("StickyTile") && (itemManager.stickyFlag || itemManager.inv_StickyFlag)) {
            isSticky = true;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            if (itemManager.inv_StickyFlag) {
                isInvSticky = true;
            }
        }
        if (other.gameObject.CompareTag("NormalTile"))
            isTouchingNorm = true;
    }
    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.CompareTag("StickyTile") && (itemManager.stickyFlag || itemManager.inv_StickyFlag))
            rb.gravityScale = 0f;
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("StickyTile") && (itemManager.stickyFlag || itemManager.inv_StickyFlag)) {
            isSticky = false;
            Gravity();
            if (itemManager.inv_StickyFlag)
                isInvSticky = false;
        }
        if (other.gameObject.CompareTag("NormalTile"))
            isTouchingNorm = false;
    }
}
