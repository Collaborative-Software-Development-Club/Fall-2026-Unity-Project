using System.Text;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class Conveyor : IConveyorable
{
    private int _size;
    private ItemQuantity[] items;
    private bool _transferReady = false;
    private bool _canInput = true;
    private bool _canOutput = true;

    public Conveyor(int size = 3)
    {
        _size = size;
        items = new ItemQuantity[_size]; 
    }

    /// <summary>
    /// Checks if the conveyor has space for incoming items
    /// </summary>
    /// <returns>A boolean that tells if it has space or not</returns>
    public bool HasSpace() => items.Length > 0 && (items[0] == null || items[0].IsEmpty());
    public int GetSize() => _size;
    public bool AddItemQuantityToPart(ItemQuantity itemQuantity)
    {
        if (items[0] is not null && !items[0].IsEmpty() && IsInputLocked) return false;

        items[0] = itemQuantity;
        return true;
    }

    public ItemQuantity RemoveLastItemFromPart()
    {
        if (IsOutputLocked) return null;
        
        ItemQuantity  lastEntry = items[^1];
        items[^1] = new ItemQuantity();
        
        return lastEntry;
    }
    
    public void ShiftEntries()
    {
        for (int i = _size - 1; i > 0; i--)
        {
            if (items[i].IsEmpty() || (items[i + 1] is not null && !items[i + 1].IsEmpty())) continue;

            if (i == _size){ _transferReady = true; continue;}
            
            items[i + 1] =  items[i];
            items[i] = new ItemQuantity();
        }
    }

    /// <summary>
    /// Transfers the last item on the belt to the next belt if there is enough space on the next;
    /// </summary>
    /// <param name="newConveyor">The conveyor that is being transferred to</param>
    /// <returns>If the the option succeeded</returns>
    public bool DoAction(IConveyorable nextConveyorable)
    {
        // Checks if the belt is ready to transfer if not it returns
        if (!_transferReady) return false;
        
        // Swaps the last item with a null item than adds to new belt
        ItemQuantity lastEntry = items[^1];
        items[^1] = new ItemQuantity();
        _transferReady = false;
        ShiftEntries();

        return nextConveyorable.AddItemQuantityToPart(lastEntry);
    }

    public override string ToString()
    {
        var text = new StringBuilder();

        for (int i = 0; i < items.Length; i++)
        {
            text.AppendLine($"({i}) {{{items[i]?.ToString() ?? "empty"}}}");
        }

        return text.ToString();
    }

    
    // These functions might be useless but I'll keep for now
    public void LockInput()
    {
        _canInput = false;
    }

    public void UnlockInput()
    {
        _canInput = true;
    }
    
    public void LockOutput()
    {
        _canOutput = false;
    }
    public void UnlockOutput()
    {
        _canOutput = true;
    }

    public bool IsInputLocked => _canInput;
    public bool IsOutputLocked => _canOutput;
}
