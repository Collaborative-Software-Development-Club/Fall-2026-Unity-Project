using System.Collections.Generic;
using UnityEngine;

public class Recipe : ScriptableObject
{
    public Dictionary<Item, int> inputs;
    public Dictionary<Item, int> outputs;
}
