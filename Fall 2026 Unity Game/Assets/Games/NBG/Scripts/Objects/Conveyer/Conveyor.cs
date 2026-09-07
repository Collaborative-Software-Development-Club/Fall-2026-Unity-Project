using System;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    private const int BeltSize = 6;
    private const float BeltTransferTime = 1; // In Seconds

    private float beltTransferTimer = BeltTransferTime;
    
    private Inventory _beltInventory = new (BeltSize);

    private Conveyor _nextConveyor;

    private Queue<int> _indexAddQueue;

    private void Update()
    {
        beltTransferTimer -= Time.deltaTime;

        if (beltTransferTimer <= 0)
        {
            TransferToNextConveyor();
            beltTransferTimer = BeltTransferTime;
        }
    }

    public bool AddToConveyor(InventorySlot slotToAdd)
    {
        if (!_beltInventory.HasEmptySlots()) return false;
        
        _indexAddQueue.Enqueue(_beltInventory.AddItemToInventory(slotToAdd.item, slotToAdd.quantity));

        return true;
    }

    public InventorySlot RemoveFromConveyor()
    {
        int slotIndex = _indexAddQueue.Dequeue();
        
        InventorySlot oldSlot = _beltInventory.slots[slotIndex];
        InventorySlot newSlot = new InventorySlot(oldSlot.item, oldSlot.quantity);

        _beltInventory.RemoveFromSlot(slotIndex, oldSlot.quantity);

        return newSlot;
    }

    public void TransferToNextConveyor()
    {
        if (!_nextConveyor || !_nextConveyor.DoesBeltHaveRoom() || _indexAddQueue.Count <= 0) return;
        
        _nextConveyor.AddToConveyor(RemoveFromConveyor());
    }

    public bool DoesBeltHaveRoom()
    {
        return _beltInventory.HasEmptySlots();
    }
}
