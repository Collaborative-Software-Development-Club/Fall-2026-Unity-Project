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

    public void SetSprite(Sprite sprite)
    {
        itemRenderer.sprite = sprite;
    }

    public void Move(Vector2 direction)
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, new Vector2(gameObject.transform.position.x, gameObject.transform.position.y) + direction , MoveDistance);
    }

}
