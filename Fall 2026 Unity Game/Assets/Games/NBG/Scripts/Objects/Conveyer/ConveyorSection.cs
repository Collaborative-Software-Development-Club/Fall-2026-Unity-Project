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
        _conveyorParts = new IConveyorable[_size];
    }
    
    public IConveyorable[] GetConveyors() => _conveyorParts;

    public void AddConveyorable(IConveyorable conveyorable)
    {
        if (GetSectionFull())
            throw new InvalidOperationException("Conveyor is already full");
        
        _conveyorParts[_conveyorCount] = conveyorable;
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
    public IConveyorable GetFirstConveyorable()
    {
        if (_conveyorCount <= 0) return null;
        return _conveyorParts[0];
    }

    public void Advance(ConveyorSection nextSection)
    {
        if (_conveyorCount == 0)
            return;

        for (int i = _conveyorCount - 1; i >= 0; i--)
        {
            if (_conveyorParts[i] == null)
                continue;

            IConveyorable next = null;
            if (i < _conveyorCount - 1)
                next = _conveyorParts[i + 1];
            else if (nextSection != null)
                next = nextSection.GetFirstConveyorable();

            _conveyorParts[i].DoAction(next);
        }

        IConveyorable lastConveyor = _conveyorParts[_conveyorCount - 1];
        _transferReady = lastConveyor != null && !lastConveyor.HasSpace();
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
