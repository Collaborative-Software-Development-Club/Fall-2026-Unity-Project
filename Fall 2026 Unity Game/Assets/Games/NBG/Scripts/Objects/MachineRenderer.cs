using UnityEngine;

public class MachineRenderer : PlaceableObj
{
    private Machine _machine;
    public Machine GetMachine =>  _machine;

    public void SetMachine(Machine machine)
    {
        _machine = machine;
    }
    
}
