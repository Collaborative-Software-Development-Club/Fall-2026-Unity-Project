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

            // In the future, send a call to the UI object to display the output of OnHoverEnter.
        }
        else if (!currentOverlap && isHovered)
        {
            isHovered = false;
            OnHoverExit();

            // In the future, send a call to the UI object to remove the hover message.
        }

        // Detection if mouse is clicked on the object.
        if (currentOverlap && UnityEngine.InputSystem.Mouse.current != null && UnityEngine.InputSystem.Mouse.current.leftButton.wasPressedThisFrame)
        {
            OnClick();
        }
    }
    public virtual void OnClick() 
    {
        // This is placeholder code that should be reimplemented by each class.
        Debug.Log(name + " was clicked. Override this method!");
    }

    public virtual string OnHoverEnter()
    {
        return name;
    }

    public virtual void OnHoverExit()
    {
        Debug.Log(name + " is no longer hovered. Override this method!");
    }
}
