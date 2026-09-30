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

    // Update is called once per frame
    void Update()
    {
        
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
        Camera cam = Camera.main;
        Debug.Log(cam);
        Vector2 worldVector = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cam.transform.position.z));
        Vector2 worldOrigin = cam.ViewportToWorldPoint(Vector2.zero);
        Debug.Log($"Size: {worldVector}");
        //Debug.Log($"Origin: {worldVector}");
        worldDimensions.halfWidth = -worldVector.x;
        worldDimensions.halfHeight = -worldVector.y;
    }
    

    private void LoopInBounds()
    {
        float clampedX = rb.transform.position.x;
        float clampedY = rb.transform.position.y;

        if (rb.transform.position.x < -worldDimensions.halfWidth) clampedX = worldDimensions.halfWidth;
        else if (rb.transform.position.x > worldDimensions.halfWidth) clampedX = -worldDimensions.halfWidth;
        else if (rb.transform.position.y < -worldDimensions.halfHeight) clampedY = worldDimensions.halfHeight;
        else if (rb.transform.position.y > worldDimensions.halfHeight) clampedY = -worldDimensions.halfHeight;

        rb.transform.position = new Vector3(clampedX, clampedY, rb.transform.position.z);
    }

    
}
