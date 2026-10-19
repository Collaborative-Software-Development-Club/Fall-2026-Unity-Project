using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This should be seen as a temp solution to rendering and shouldn't be kept forever, this should be replaced with sections controlling all items 
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ConveyorRenderer : PlaceableObj 
{
    private List<ConveyorItemRenderer> _items = new();
    private Conveyor _connectedConveyor;

    private float _moveDelayTimer = 0;
    private float _moveDelay = .5f;

    public void SetConveyor(Conveyor connectedConveyor)
    {
        _connectedConveyor = connectedConveyor;
    }
    
    public Conveyor GetConveyor() => _connectedConveyor;

    private void FixedUpdate()
    {
        _moveDelayTimer += Time.fixedDeltaTime;
        
        if (_items.Count < 0 || _moveDelayTimer < _moveDelay) return;
        
        foreach (ConveyorItemRenderer item in _items)
            item.Move(GetDirection());
        
        _moveDelayTimer = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out ConveyorItemRenderer itemRenderer) && !itemRenderer.HasParent)
        {
            _items.Add(itemRenderer);
            itemRenderer.SetParentRenderer(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out ConveyorItemRenderer itemRenderer) && itemRenderer.IsParent(this))
        {
            _items.Remove(itemRenderer);
            itemRenderer.SetParentRenderer(this);
        }
    }
}
