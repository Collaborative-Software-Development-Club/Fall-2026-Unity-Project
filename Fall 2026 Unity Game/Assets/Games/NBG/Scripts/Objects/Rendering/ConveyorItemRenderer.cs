using System;
using UnityEngine;

public class ConveyorItemRenderer : MonoBehaviour
{
    [SerializeField] SpriteRenderer _itemRenderer;
    
    private ConveyorRenderer _parentRenderer;
    private int _conveyorPosition = -1;
    private bool _canMove = false;
    private const float MoveDistance = 5.0f; 

    private void Start()
    {
        if(!_itemRenderer)
            _itemRenderer = GetComponent<SpriteRenderer>(); 
    }
    public bool HasParent =>  _parentRenderer != null;
    public bool IsParent(ConveyorRenderer conveyorRenderer) => _parentRenderer == conveyorRenderer;
    public void SetParentRenderer(ConveyorRenderer parent)
    {
        _parentRenderer = parent;
    }

    public void SetSprite(Sprite sprite)
    {
        _itemRenderer.sprite = sprite;
    }

    public void Move(Vector2 direction)
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + direction , MoveDistance);
    }

}
