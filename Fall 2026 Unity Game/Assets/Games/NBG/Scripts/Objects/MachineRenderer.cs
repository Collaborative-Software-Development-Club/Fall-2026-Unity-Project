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
    private readonly HashSet<ConveyorItemRenderer> _exitingItems = new();
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

    private void EmptyOutput(ItemQuantity added)
    {
        if (added == null || added.IsEmpty() || added.Item == null) return;

        // TODO: Need to spawn box renderers if it's a box renderer
        ConveyorItemRenderer itemRenderer =  SpawnOutputRenderer();
        if (itemRenderer == null) return;

        _exitingItems.Add(itemRenderer);
        itemRenderer.DetachFromBelt();
        itemRenderer.SetItem(added.Item);
        itemRenderer.transform.position = (Vector2)transform.position + GetDirection() * 0.5f;
        itemRenderer.gameObject.SetActive(true);

        SpriteRenderer spriteRenderer = itemRenderer.GetSpriteRenderer;
        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        _touchedItems.Add(itemRenderer);
    }
    private ConveyorItemRenderer SpawnOutputRenderer()
    {
        if (_itemPrefab == null)
        {
            Debug.LogError("Machine has no item prefab, so it cannot spawn output sprites.", this);
            return null;
        }

        GameObject spawned = Instantiate(_itemPrefab);
        ConveyorItemRenderer itemRenderer = spawned.GetComponent<ConveyorItemRenderer>();
        if (itemRenderer != null) return itemRenderer;

        Debug.LogError("Item prefab has no ConveyorItemRenderer.", spawned);
        Destroy(spawned);
        return null;
    }

    private void AddItem(Item item)
    {
        // Should do a check to see if this is contained when added if not don't add it in should also stall conveyors
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.TryGetComponent(out ConveyorItemRenderer itemRenderer)) return;
        if (!_touchedItems.Contains(itemRenderer) || !itemRenderer.gameObject.activeInHierarchy) return;
        itemRenderer.Move(GetDirection());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out ConveyorItemRenderer itemRenderer) || _exitingItems.Contains(itemRenderer) || _touchedItems.Contains(itemRenderer)) return;

        _touchedItems.Add(itemRenderer);
        itemRenderer.gameObject.SetActive(false);
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent(out ConveyorItemRenderer itemRenderer)) return;

        if (!itemRenderer.gameObject.activeInHierarchy && _touchedItems.Contains(itemRenderer))
            return;

        _touchedItems.Remove(itemRenderer);
        _exitingItems.Remove(itemRenderer);
    }
}
