using System;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private const int BeltSize = 3;
    private const float BeltTransferTime = 1; // In Seconds
    private const float MoveSpeed = 3f;

    private float beltTransferTimer = BeltTransferTime;
    
    private Inventory _beltInventory = new(BeltSize);
    private ItemHolder[] _itemHolders = new ItemHolder[BeltSize]; 

    [SerializeField] private Conveyor nextConveyor;

    private Queue<int> _indexAddQueue = new();

    public Action ConveyorUpdated;
    
    public Vector2 conveyorDirection = Vector2.right;
    
    // JUST FOR TESTING
    public ItemData testItem;

    private void Start()
    {
        if (!testItem) return;

        for (int i = 0; i < 3; i++)
        {
            GameObject testObj = ItemFactory.CreateItemHolder(testItem);

            AddToConveyor(testObj.GetComponent<ItemHolder>());
        }
    }

    private void Update()
    {
        beltTransferTimer -= Time.deltaTime;

        if (beltTransferTimer <= 0)
        {
            TransferToNextConveyor();
            beltTransferTimer = BeltTransferTime;
        }
        
        UpdateItemPositions();
    }

    public bool AddToConveyor(ItemHolder itemHolder)
    {
        if (!_beltInventory.HasEmptySlots()) return false;

        itemHolder.transform.parent = transform;
        int slotIndex = _beltInventory.AddItemToInventory(itemHolder.Item);
        _itemHolders[slotIndex] = itemHolder;
        _indexAddQueue.Enqueue(slotIndex);

        return true;
    }

    public ItemHolder RemoveFromConveyor()
    {
        int slotIndex = _indexAddQueue.Dequeue();
        ItemHolder itemHolder = _itemHolders[slotIndex];
        _itemHolders[slotIndex] = null;
        
        InventorySlot oldSlot = _beltInventory.slots[slotIndex];
        _beltInventory.RemoveFromSlot(slotIndex, oldSlot.quantity);
        
        ShiftItemsForward();
        
        ConveyorUpdated?.Invoke();

        return itemHolder;
    }

    public void TransferToNextConveyor()
    {
        if (!nextConveyor || !nextConveyor.DoesBeltHaveRoom() || _indexAddQueue.Count <= 0) return;
        
        ItemHolder holder = RemoveFromConveyor();
        if (!holder) return;
        
        nextConveyor.AddToConveyor(holder);
    }
    
    private void UpdateItemPositions()
    {
        for (int i = 0; i < BeltSize; i++)
        {
            if (_itemHolders[i] == null) continue;

            Vector3 targetLocalPos = GetLocalPositionForSlot(i);

            _itemHolders[i].transform.localPosition = Vector3.MoveTowards(
                _itemHolders[i].transform.localPosition,
                targetLocalPos,
                MoveSpeed * Time.deltaTime
            );
        }
    }

    private Vector3 GetLocalPositionForSlot(int slotIndex)
    {
        float step = 1.0f / BeltSize;
        float xOffset = (Math.Abs(slotIndex - BeltSize) * step) - .1f;
        float yOffset = (Math.Abs(slotIndex - BeltSize) * step) - .1f; 
        return new Vector3(xOffset * conveyorDirection.x, yOffset * conveyorDirection.y, 0);
    }
    
    private void ShiftItemsForward()
    {
        for (int i = _itemHolders.Length - 1; i > 0; i--)
        {
            if (_itemHolders[i - 1] == null || _itemHolders[i] != null) continue;
            
            _itemHolders[i] = _itemHolders[i - 1];
            _itemHolders[i - 1] = null;
        }
    }
    
    public bool DoesBeltHaveRoom() => _beltInventory.HasEmptySlots();
    public Conveyor GetNextConveyor() => nextConveyor;
    public int GetBeltSize() => BeltSize;
}
