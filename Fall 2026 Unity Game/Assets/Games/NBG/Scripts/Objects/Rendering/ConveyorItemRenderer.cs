using System;
using UnityEngine;

public class ConveyorItemRenderer : MonoBehaviour, IHoverable
{
    [SerializeField] SpriteRenderer itemRenderer;
    
    private ConveyorRenderer _parentRenderer;
    [SerializeField] private float moveSpeed = 2.0f; 
    private Item _item;

    public Item Item => _item;

    public void SetItem(Item item)
    {
        name = item.GetDisplayName();
        _item = item;
        SetSprite(item.GetIcon());
    }

    public void DetachFromBelt()
    {
        if (_parentRenderer == null) return;

        _parentRenderer.RemoveItem(this);
        _parentRenderer = null;
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


    public void Move(Vector2 direction)
    {
        Vector2 currentPos = transform.position;
        Vector2 targetPos = currentPos + direction * (moveSpeed * Time.fixedDeltaTime);
    
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

    // TODO: Make these not part of ConveyorItemRenderer
    public void OnHoverEnter()
    {
       TextController.Instance.SetText(HoverMessage); 
    }

    public void OnHoverExit()
    {
        TextController.Instance.ClearText();
    }

    public string HoverMessage => _item.GetDisplayName();

    public static GameObject CreateRenderer()
    {
        GameObject temp = new GameObject();
        temp.AddComponent<ItemHolder>();
        temp.AddComponent<ConveyorItemRenderer>();
        temp.AddComponent<BoxCollider2D>().size = new Vector2(.8f, .8f);
        temp.AddComponent<Rigidbody2D>().gravityScale = 0;

        return temp;
    }
}
