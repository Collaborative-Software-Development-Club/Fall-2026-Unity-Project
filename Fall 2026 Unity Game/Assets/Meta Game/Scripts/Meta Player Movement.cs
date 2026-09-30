using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class MetaPlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;

    private (float halfWidth, float halfHeight) worldDimensions;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetWorldDimensions();
    }

    private void FixedUpdate()
    {
        rb.AddForce(movement * speed * Time.fixedDeltaTime,ForceMode2D.Impulse);
        LoopInBounds();
    }

    public void Move(InputAction.CallbackContext context)
    {
        movement = context.ReadValue<Vector2>();
    }

    private void GetWorldDimensions()
    {
        Vector2 worldVector = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        worldDimensions.halfWidth = -worldVector.x;
        worldDimensions.halfHeight = -worldVector.y;
    }
    
    private void LoopInBounds()
    {
        float wrappedX = rb.transform.position.x;
        float wrappedY = rb.transform.position.y;

        if (rb.transform.position.x < -worldDimensions.halfWidth) wrappedX = worldDimensions.halfWidth;
        else if (rb.transform.position.x > worldDimensions.halfWidth) wrappedX = -worldDimensions.halfWidth;
        else if (rb.transform.position.y < -worldDimensions.halfHeight) wrappedY = worldDimensions.halfHeight;
        else if (rb.transform.position.y > worldDimensions.halfHeight) wrappedY = -worldDimensions.halfHeight;

        rb.transform.position = new Vector3(wrappedX, wrappedY, rb.transform.position.z);
    }

    
}
