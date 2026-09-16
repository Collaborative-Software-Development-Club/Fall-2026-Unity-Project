using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 movement;
    public Rigidbody2D rb;
    public float moveSpeed = 400f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        movement.Normalize();

    }

    private void FixedUpdate()
    {
        rb.linearVelocity = movement * Time.fixedDeltaTime * moveSpeed;
    }
}
