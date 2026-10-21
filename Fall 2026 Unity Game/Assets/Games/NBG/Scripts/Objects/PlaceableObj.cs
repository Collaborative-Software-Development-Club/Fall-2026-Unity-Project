using System;
using UnityEngine;

public class PlaceableObj : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Vector2 _dir = Vector2.zero;
    public Vector2 GetDirection() => _dir;

    private void Awake()
    {
        FindRenderer();
    }

    public void SetDirection(Vector2 dir)
    {
        _dir = dir;
    }

    public void SetSprite(Sprite sprite)
    {
        FindRenderer();

        if (_spriteRenderer == null)
        {
            Debug.LogError($"[{gameObject.name}] PlaceableObj could not find a SpriteRenderer on this object or its children!", this);
            return;
        }

        if (sprite == null)
        {
            Debug.LogWarning($"[{gameObject.name}] Passed sprite is null!", this);
            return;
        }

        _spriteRenderer.sprite = sprite;
    }
    
    private void FindRenderer()
    {
        if (_spriteRenderer == null)
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Checks self and children
    }
}
