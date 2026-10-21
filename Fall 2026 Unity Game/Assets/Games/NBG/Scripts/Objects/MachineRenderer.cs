using UnityEngine;

public class MachineRenderer : PlaceableObj
{
    private Machine _machine;
    public Machine GetMachine =>  _machine;

    public void SetMachine(Machine machine)
    {
        _machine = machine;
    }

    public void Initialize(Machine machine)
    {
        _machine = machine;
        SetSprite(machine.GetIcon());
    }
    
}
