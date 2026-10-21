using System;
using System.Net;
using System.Text;
using UnityEngine;

public class ConveyorSection 
{
    private int _size;
    private int _lastAddedConveyorIdx = 0;
    private int _conveyorCount = 0;
    
    private IConveyorable[] _conveyorParts;
    
    private bool _transferReady;
    
    public int GetSize => _size;

    public ConveyorSection(int size = 10)
    {
        _size = size;
        _conveyorParts = new Conveyor[_size];
    }
    
    public IConveyorable[] GetConveyors() => _conveyorParts;

    public void AddConveyorable(Conveyor conveyor)
    {
        if (GetSectionFull())
            throw new InvalidOperationException("Conveyor is already full");
        
        _conveyorParts[_conveyorCount] = conveyor;
        _conveyorCount++;
    }

    public bool AddItemToStartOfChain(ItemQuantity itemQuantity)
    {
        return _conveyorParts[0].AddItemQuantityToPart(itemQuantity);
    }

    public IConveyorable RemoveConveyorAt(int index)
    {
        IConveyorable conveyor = _conveyorParts[index];

        Array.Copy(_conveyorParts, index + 1, _conveyorParts, index, _lastAddedConveyorIdx - index);

        _conveyorParts[_lastAddedConveyorIdx] = null;
        _lastAddedConveyorIdx--;

        return conveyor;
    }

    public IConveyorable RemoveLastConveyorable()
    {
        return _conveyorParts[_lastAddedConveyorIdx];
    }

    /// <summary>
    /// Gets whether the max amount of conveyors has been added
    /// </summary>
    /// <returns>A boolean that shows if the max number of conveyors have been added</returns>
    public bool GetSectionFull() => _conveyorCount >= _size; 

    /// <summary>
    /// Checks if the section is ready to transfer to another section
    /// </summary>
    /// <returns>If the transfer is ready</returns>
    public bool IsTransferReady() => _transferReady;
    public void Advance(ConveyorSection nextSection)
    {
        for (int i = _conveyorCount - 2; i >= 0; i--)
        {
            if (_conveyorParts[i] == null || _conveyorParts[i + 1] == null)
                continue;

            _conveyorParts[i].DoAction(_conveyorParts[i + 1]);
        }

        if (_conveyorCount == 0)
            return;

        IConveyorable lastConveyor = _conveyorParts[_conveyorCount - 1];

        _transferReady = !lastConveyor.HasSpace();

        if (!_transferReady || nextSection == null || !nextSection.IsTransferReady())
            return;

        ItemQuantity removedItem = lastConveyor.RemoveLastItemFromPart();
        nextSection.AddItemToStartOfChain(removedItem);
    }


    public override string ToString()
    {
        var text = new StringBuilder();

        for (int i = 0; i < _conveyorParts.Length; i++)
        {
            text.AppendLine($"Conveyor {i}:");

            if (_conveyorParts[i] == null)
                text.AppendLine("  empty");
            else
                text.Append(_conveyorParts[i]);
        }

        return text.ToString();
    }

}
