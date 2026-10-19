using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This should be seen as a temp solution to rendering and shouldn't be kept forever, this should be replaced with sections controlling all items 
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ConveyorRenderer : MonoBehaviour
{
    private Vector2 _dir = Vector2.zero;
    private List<ConveyorItemRenderer> _items = new();
    private Conveyor _connectedConveyor;

    public void SetConveyor(Conveyor connectedConveyor)
    {
        _connectedConveyor = connectedConveyor;
    }
    
    public Conveyor GetConveyor() => _connectedConveyor;
    public Vector2 GetDirection() => _dir;

    public void SetDirection(Vector2 dir)
    {
        _dir = dir;
    }

    private void FixedUpdate()
    {
        if (_items.Count < 0) return;
        
        foreach (ConveyorItemRenderer item in _items)
        {
            item.Move(_dir);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ConveyorItemRenderer itemRenderer))
        {
            _items.Add(itemRenderer);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out ConveyorItemRenderer itemRenderer))
        {
            _items.Remove(itemRenderer);
        } 
    }
}
