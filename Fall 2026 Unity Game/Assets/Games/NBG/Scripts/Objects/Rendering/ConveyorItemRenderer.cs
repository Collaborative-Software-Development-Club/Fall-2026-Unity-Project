using System;
using UnityEngine;

public class ConveyorItemRenderer : MonoBehaviour
{
    [SerializeField] SpriteRenderer itemRenderer;
    
    private ConveyorRenderer _parentRenderer;
    private const float MoveDistance = 5.0f; 

    private void Start()
    {
        if(!itemRenderer)
            itemRenderer = GetComponent<SpriteRenderer>(); 
    }
    public bool HasParent =>  _parentRenderer != null;
    public bool IsParent(ConveyorRenderer conveyorRenderer) => _parentRenderer == conveyorRenderer;
    public void SetParentRenderer(ConveyorRenderer parent)
    {
        _parentRenderer = parent;
    }

    public void SetSpriteRenderer(SpriteRenderer renderer)
    {
        itemRenderer = renderer;
    }

    public SpriteRenderer GetSpriteRenderer => itemRenderer; 

    public void SetSprite(Sprite sprite)
    {
        itemRenderer.sprite = sprite;
    }

    [SerializeField] private float moveSpeed = 2.0f; 

    public void Move(Vector2 direction)
    {
        Vector2 currentPos = transform.position;
        Vector2 targetPos = currentPos + (direction * moveSpeed * Time.fixedDeltaTime);
    
        transform.position = Vector2.MoveTowards(currentPos, targetPos, moveSpeed * Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        if (_parentRenderer is null) return;
        _parentRenderer.RemoveItem(this);
    }
}
