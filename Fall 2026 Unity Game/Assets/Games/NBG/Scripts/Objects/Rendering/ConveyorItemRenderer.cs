using System;
using UnityEngine;

public class ConveyorItemRenderer : MonoBehaviour
{
    [SerializeField] SpriteRenderer itemRenderer;
    
    private ConveyorRenderer _parentRenderer;
    private const float MoveDistance = 5.0f;
    private Item _item;
    private float _machineLock;

    public Item Item => _item;
    public bool WasReleasedByMachine => _machineLock > 0f;

    public void SetItem(Item item)
    {
        _item = item;
    }

    public void MarkReleasedByMachine()
    {
        _machineLock = 0.5f;
    }

    public void DetachFromBelt()
    {
        if (_parentRenderer == null) return;

        _parentRenderer.RemoveItem(this);
        _parentRenderer = null;
    }

    private void FixedUpdate()
    {
        if (_machineLock > 0f)
            _machineLock -= Time.fixedDeltaTime;
    }

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
        if (!itemRenderer)
            itemRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!itemRenderer || sprite == null) return;

        itemRenderer.sprite = sprite;
    }

    public void SetColor(Color color)
    {
        if (!itemRenderer)
            itemRenderer = GetComponentInChildren<SpriteRenderer>();

        if (!itemRenderer) return;

        itemRenderer.color = color;
    }

    [SerializeField] private float moveSpeed = 2.0f; 

    public void Move(Vector2 direction)
    {
        Vector2 currentPos = transform.position;
        Vector2 targetPos = currentPos + (direction * moveSpeed * Time.fixedDeltaTime);
    
        transform.position = Vector2.MoveTowards(currentPos, targetPos, moveSpeed * Time.fixedDeltaTime);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_parentRenderer is null) return;
        _parentRenderer.RemoveItem(this);
    }
}
