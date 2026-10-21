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
        if (IsInputLocked || !HasSpace()) return false;

        items[0] = itemQuantity;
        return true;
    }

    public ItemQuantity RemoveLastItemFromPart()
    {
        if (IsOutputLocked || _size == 0 || !ItemPresent(items[^1])) return null;

        ItemQuantity lastEntry = items[^1];
        items[^1] = new ItemQuantity();
        _transferReady = false;

        return lastEntry;
    }

    public void ShiftEntries()
    {
        for (int i = _size - 2; i >= 0; i--)
        {
            if (!ItemPresent(items[i]) || ItemPresent(items[i + 1])) continue;

            items[i + 1] = items[i];
            items[i] = new ItemQuantity();
        }

        _transferReady = _size > 0 && ItemPresent(items[_size - 1]);
    }

    /// <summary>
    /// Moves items one step toward the exit, then hands the exit item to the next part when it has room.
    /// </summary>
    /// <param name="nextConveyorable">The conveyor that is being transferred to</param>
    /// <returns>If the option succeeded</returns>
    public bool DoAction(IConveyorable nextConveyorable)
    {
        if (_size == 0) return false;

        bool transferred = false;

        if (_transferReady && nextConveyorable != null && nextConveyorable.HasSpace() && ItemPresent(items[^1]))
        {
            ItemQuantity lastEntry = items[^1];
            if (nextConveyorable.AddItemQuantityToPart(lastEntry))
            {
                items[^1] = new ItemQuantity();
                _transferReady = false;
                transferred = true;
            }
        }

        ShiftEntries();
        return transferred;
    }

    private static bool ItemPresent(ItemQuantity item) => item != null && !item.IsEmpty();

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

    public bool IsInputLocked => !_canInput;
    public bool IsOutputLocked => !_canOutput;
}
