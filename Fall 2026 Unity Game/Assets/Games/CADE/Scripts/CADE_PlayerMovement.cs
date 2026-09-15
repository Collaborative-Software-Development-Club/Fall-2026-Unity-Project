using UnityEngine;
using UnityEngine.InputSystem;

public class CADE_PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [Header("Movement")]
    [SerializeField]
    private float moveSpeed = 5f;

    [Header("Jumping")]
    [SerializeField]
    private float jumpHeight = 10f;

    [Header("Ground Check")]
    [SerializeField]
    private Vector2 boxSize;
    [SerializeField]
    private float castDistance;
    [SerializeField]
    private LayerMask groundLayer;

    [Header("Gravity")]
    [SerializeField]
    private float baseGravity = 1;
    [SerializeField]
    private float maxFallSpeed = 18;
    [SerializeField]
    private float fallSpeedMultipler = 2f;

    private bool jumpReleased;
    private float horizontalMovement;
    private float verticalMovement;

    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocityY);
        Gravity();
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded() && context.performed)
        {
            jumpReleased = false;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);
        }
        else if (context.canceled && !jumpReleased)
        {
            jumpReleased = true;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
        }
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
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

}
