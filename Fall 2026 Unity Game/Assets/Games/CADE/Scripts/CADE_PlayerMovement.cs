using UnityEngine;
using UnityEngine.InputSystem;

public class CADE_PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float jumpHeight = 10f;

    private float horizontalMovement;
    private float verticalMovement;

    void Update()
    {
        rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocityY);
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && rb.linearVelocityY == 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpHeight);
        }
        else if (context.canceled) 
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * 0.5f);
        }
    }
}
