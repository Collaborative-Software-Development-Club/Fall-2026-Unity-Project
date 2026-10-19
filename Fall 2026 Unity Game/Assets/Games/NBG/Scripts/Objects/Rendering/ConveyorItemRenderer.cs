using System;
using UnityEngine;

public class ConveyorItemRenderer : MonoBehaviour
{
    SpriteRenderer _itemRenderer;

    private int _conveyorPosition = -1;
    private bool _canMove = false;
    private const float MoveDistance = 5.0f; 

    private void Start()
    {
       _itemRenderer = GetComponent<SpriteRenderer>(); 
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
