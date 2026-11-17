using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MachineRenderer : PlaceableObj
{
    private Machine _machine;
    private GameObject _itemPrefab;
    private float _elapsed;

    private List<ConveyorItemRenderer> _touchedItems = new();
    public Machine GetMachine => _machine;

    public void SetMachine(Machine machine)
    {
        _machine = machine;
    }

    public void Initialize(Machine machine)
    {
        Initialize(machine, GetDirection(), ConveyorGenerator.ItemPrefab);
    }

    public void Initialize(Machine machine, Vector2 beltDirection, GameObject itemPrefab)
    {
        _machine = machine;
        _itemPrefab = itemPrefab;
        SetDirection(beltDirection.sqrMagnitude > 0.01f ? beltDirection.normalized : Vector2.right);
        SetSprite(machine.GetIcon());

        if (!_itemPrefab)
            Debug.LogError("Machine has no item prefab, so it cannot spawn output sprites.", this);
        
        _machine.OnOutputAdded += EmptyOutput;
        _machine.OnInputAdded += AddItem;
    }

    private void EmptyOutput()
    {
        int outputLength = _machine.GetOutputInventory().Length;
        
        List<ConveyorItemRenderer> itemsToCheck = _touchedItems;
        List<ConveyorItemRenderer> checkedItems = new();

        while (outputLength > 0)
        {
            ConveyorItemRenderer item;
            if (itemsToCheck.Count > 0)
            {
                item = itemsToCheck[^1];
                item.SetItem(_machine.GetOutputFromSlot(outputLength));
                _touchedItems.RemoveAt(_touchedItems.Count - 1);
            }
            else
            {
                item = ConveyorItemRenderer.CreateRenderer().GetComponent<ConveyorItemRenderer>();
            }
            checkedItems.Add(item);
            outputLength--;
        }
        
        _touchedItems = checkedItems;
        
        // Teleports objects to end of conveyor
        foreach (var itemRenderer in _touchedItems)
        {
            Debug.Log("Is this thing on?");
            itemRenderer.gameObject.SetActive(true);
            itemRenderer.transform.position = GetDirection() * itemRenderer.transform.localScale.x; 
        } 
    }

    private void AddItem(Item item)
    {
        // Should do a check to see if this is contained when added if not don't add it in should also stall conveyors
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(!other.TryGetComponent(out ConveyorItemRenderer itemRenderer)) return;
        if (!_touchedItems.Contains(itemRenderer)) return;
        itemRenderer.Move(GetDirection());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO: Make it invisible based on if it's added later on
        if (other.TryGetComponent(out ConveyorItemRenderer conveyorItemRenderer) &&
            !_touchedItems.Contains(conveyorItemRenderer))
        {
            _touchedItems.Add(conveyorItemRenderer);
            conveyorItemRenderer.gameObject.SetActive(false);
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
       if (other.TryGetComponent(out ConveyorItemRenderer conveyorItemRenderer))
           _touchedItems.Remove(conveyorItemRenderer);
    }
}
