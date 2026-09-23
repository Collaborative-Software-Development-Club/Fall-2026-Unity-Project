using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class InteractObject : MonoBehaviour
{
    private Collider2D _collider;
    private bool isHovered = false;
    private Camera mainCamera;
    protected virtual void Awake()
    {
        _collider = GetComponent<Collider2D>();
        mainCamera = Camera.main;
    }

    protected virtual void Update()
    {
        // Get the screen moust position.
        Vector2 mouseScreenPos = Vector2.zero;
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            mouseScreenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        }

        // Convert pixel screen coordinates directly into 2D world space. We must assign a fake Z value and then remove it in case
        // any objects in the world are placed at different Z values.
        Vector3 worldPos3D = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f));
        Vector2 worldPos2D = new Vector2(worldPos3D.x, worldPos3D.y);

        // Test if the mouse coordinates overlap our 2D BoxCollider boundary
        bool currentOverlap = _collider.OverlapPoint(worldPos2D);

        // Detection if mouse is hovering over the object.
        if (currentOverlap && !isHovered)
        {
            isHovered = true;
            string HoverMessage = OnHoverEnter();

            // Update the UI object with the hover message.
            TextController.Instance.SetText(HoverMessage);
        }
        else if (!currentOverlap && isHovered)
        {
            isHovered = false;
            OnHoverExit();
            // Clear the UI object hover message
            TextController.Instance.ClearText();
        }

        // Detection if mouse is clicked on the object.
        if (currentOverlap && UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnClick();
        }
    }
    /**
     * This method is called when the object is clicked on.
     * Override this method in a derived class to implement custom click behavior.
     */
    public virtual void OnClick() 
    {
        
    }
    /**
     * This method is called when the mouse hovers over the object.
     * Override this method in a derived class to implement custom hover behavior.
     * @return A string message to display when the mouse hovers over the object.
     */
    public virtual string OnHoverEnter()
    {
        return name;
    }
    /**
     * This method is called when the mouse stops hovering over the object.
     * Override this method in a derived class to implement custom hover exit behavior.
     */
    public virtual void OnHoverExit()
    {
        
    }
}
