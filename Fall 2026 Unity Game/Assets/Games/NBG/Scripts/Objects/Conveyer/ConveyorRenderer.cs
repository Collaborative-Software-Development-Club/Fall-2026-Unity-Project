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

    public void SetConveyor(Conveyor connectedConveyor)
    {
        _connectedConveyor = connectedConveyor;
    }

    public bool RemoveItem(ConveyorItemRenderer item)
    {
        return _items.Remove(item);
    }
    
    public Conveyor GetConveyor() => _connectedConveyor;

    private void FixedUpdate()
    {
        _items.RemoveAll(item => item is null); 
        
        if (_items.Count < 0) return;
        
        foreach (ConveyorItemRenderer item in _items)
            item.Move(GetDirection());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out ConveyorItemRenderer itemRenderer) || itemRenderer.HasParent) return;
        
        _items.Add(itemRenderer);
        itemRenderer.SetParentRenderer(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out ConveyorItemRenderer itemRenderer) || !itemRenderer.IsParent(this)) return;
        
        _items.Remove(itemRenderer);
        itemRenderer.SetParentRenderer(null);
    }
}
