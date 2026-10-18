using System;

public class ConveyorSection 
{
    private int _size;
    private int _lastFilledIdx = 0;
    private Conveyor[] _conveyors;

    public ConveyorSection(int size = 10)
    {
        _size = size;
        _conveyors = new Conveyor[_size];
    }

    public void AddConveyor(Conveyor conveyor)
    {
        _conveyors[_lastFilledIdx + 1] = conveyor;
        _lastFilledIdx++;
    }

    public Conveyor RemoveConveyorAt(int index)
    {
        Conveyor conveyor = _conveyors[index];

        Array.Copy(_conveyors, index + 1, _conveyors, index, _lastFilledIdx - index);

        _conveyors[_lastFilledIdx] = null;
        _lastFilledIdx--;

        return conveyor;
    }

    public Conveyor RemoveLastConveyor()
    {
        return _conveyors[_lastFilledIdx];
    }
    
    public bool GetFull() => _lastFilledIdx < _size;

    public void Advance()
    {
        
    }
}
