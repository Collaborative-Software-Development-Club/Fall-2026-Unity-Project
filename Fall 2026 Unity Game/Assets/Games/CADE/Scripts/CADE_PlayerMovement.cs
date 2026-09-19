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

    [Header("Dashing")] 
    [SerializeField] 
    private float dashForce = 5;

    [Header("Ground Check")]
    [SerializeField]
    private Vector2 boxSize;
    [SerializeField]
    private float castDistance;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField] private float coyoteTime;

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
    private bool isDashing;
    private float coyoteTimeCounter = 0f;

    void Start()
    {
        finalSpeed = moveSpeed;
    }

    private void Update() {
        if (isGrounded()) {
            coyoteTimeCounter = coyoteTime;
        } else {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    void FixedUpdate() {
        if (isDashing) return;
        
        rb.linearVelocity = new Vector2(horizontalMovement * finalSpeed, rb.linearVelocityY);
        Gravity();
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (coyoteTimeCounter > 0f && context.performed)
        {
            jumpReleased = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);
            coyoteTimeCounter = 0;
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
        if (context.performed) {
            StartCoroutine(PerformDash(horizontalMovement));
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

}
