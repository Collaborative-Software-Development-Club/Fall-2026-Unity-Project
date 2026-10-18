using UnityEngine;

public class Conveyor
{
    private int _size;
    private ItemQuantity[] items;
    private bool _transferReady = false;

    public Conveyor(int size = 3)
    {
        _size = size;
        items = new ItemQuantity[_size]; 
    }

    public bool AddToBelt(ItemQuantity item)
    {
        if (!items[0].IsEmpty()) return false;

        items[0] = item;
        return true;
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

    public void Advance(Conveyor newConveyor)
    {
        if (!_transferReady) return;
        
        ItemQuantity lastEntry = items[^1];
        items[^1] = new ItemQuantity();

        newConveyor.AddToBelt(lastEntry);
    }
}
