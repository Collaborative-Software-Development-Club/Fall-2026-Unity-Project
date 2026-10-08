using System;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : ScriptableObject
{
    [SerializeField] private List<ItemQuantity> inputs;
    [SerializeField] private List<ItemQuantity> outputs;

    // TODO: MAKE ERROR FOR DUPLICATE INPUTS LATER ME
}

