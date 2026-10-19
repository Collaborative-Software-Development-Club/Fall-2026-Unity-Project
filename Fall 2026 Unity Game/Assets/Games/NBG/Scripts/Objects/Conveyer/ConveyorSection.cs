using System;
using System.Net;
using System.Text;

public class ConveyorSection 
{
    private int _size;
    private int _lastAddedConveyorIdx = 0;
    private int _conveyorCount = 0;
    
    private Conveyor[] _conveyors;
    
    private bool _transferReady;

    public ConveyorSection(int size = 10)
    {
        _size = size;
        _conveyors = new Conveyor[_size];
    }

    public void AddConveyor(Conveyor conveyor)
    {
        if (GetSectionFull())
            throw new InvalidOperationException("Conveyor is already full");
        
        _conveyors[_conveyorCount] = conveyor;
        _conveyorCount++;
    }

    public bool AddItemToStartOfChain(ItemQuantity itemQuantity)
    {
        return _conveyors[0].AddToBelt(itemQuantity);
    }

    public Conveyor RemoveConveyorAt(int index)
    {
        Conveyor conveyor = _conveyors[index];

        Array.Copy(_conveyors, index + 1, _conveyors, index, _lastAddedConveyorIdx - index);

        _conveyors[_lastAddedConveyorIdx] = null;
        _lastAddedConveyorIdx--;

        return conveyor;
    }

    public Conveyor RemoveLastConveyor()
    {
        return _conveyors[_lastAddedConveyorIdx];
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
            if (_conveyors[i] == null || _conveyors[i + 1] == null)
                continue;

            _conveyors[i].Advance(_conveyors[i + 1]);
        }

        if (_conveyorCount == 0)
            return;

        Conveyor lastConveyor = _conveyors[_conveyorCount - 1];

        _transferReady = !lastConveyor.HasSpace();

        if (!_transferReady || nextSection == null || !nextSection.IsTransferReady())
            return;

        ItemQuantity removedItem = lastConveyor.RemoveLastItemFromBelt();
        nextSection.AddItemToStartOfChain(removedItem);
    }


    public override string ToString()
    {
        var text = new StringBuilder();

        for (int i = 0; i < _conveyors.Length; i++)
        {
            text.AppendLine($"Conveyor {i}:");

            if (_conveyors[i] == null)
                text.AppendLine("  empty");
            else
                text.Append(_conveyors[i]);
        }

        return text.ToString();
    }

}
