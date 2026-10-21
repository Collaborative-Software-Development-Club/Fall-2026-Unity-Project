using System;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    private Camera _mainCamera;
    private IHoverable _currentHover; 

    private void Start()
    {
       _mainCamera = FindFirstObjectByType<Camera>();
    }

    private void Update()
    {
        CheckInteractions();
    }

    private void CheckInteractions()
    {
        // Get the screen moust position.
        Vector2 mouseScreenPos = Vector2.zero;
        if (UnityEngine.InputSystem.Mouse.current != null)
        {
            mouseScreenPos = UnityEngine.InputSystem.Mouse.current.position.ReadValue();
        }

        // Convert pixel screen coordinates directly into 2D world space. We must assign a fake Z value and then remove it in case
        // any objects in the world are placed at different Z values.
        //print(mainCamera);
        Vector3 worldPos3D = _mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 10f));
        Vector2 worldPos2D = new Vector2(worldPos3D.x, worldPos3D.y);

        Collider2D[] results = Physics2D.OverlapPointAll(new Vector2(worldPos2D.x, worldPos2D.y));

        IHoverable nextHover = null;
        var mouse = UnityEngine.InputSystem.Mouse.current;

        foreach (Collider2D collider in results)
        {
            if (nextHover == null &&
                collider.TryGetComponent<IHoverable>(out var hoverable))
            {
                nextHover = hoverable;
            }

            if (mouse != null && mouse.leftButton.wasPressedThisFrame &&
                collider.TryGetComponent<ILeftClickable>(out var leftClickable))
            {
                leftClickable.OnLeftClick();
            }

            if (mouse != null && mouse.rightButton.wasPressedThisFrame &&
                collider.TryGetComponent<IRightClickable>(out var rightClickable))
            {
                rightClickable.OnRightClick();
            }
        }

        if (ReferenceEquals(nextHover, _currentHover)) return;
        
        _currentHover?.OnHoverExit();
        _currentHover = nextHover;
            
        if (_currentHover != null)
            _currentHover.OnHoverEnter();
    }
}
