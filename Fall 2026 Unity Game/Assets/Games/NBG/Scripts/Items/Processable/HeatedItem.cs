using System;
using UnityEngine;

// A processable that carries heat. Only heated items hold heat, not every item.
[Serializable]
public class HeatedItem : Processable, IHeatable
{
    // Runtime heat, per instance (not on the shared ItemData asset).
    [SerializeField] private Heat heat = new Heat();

    public Heat Heat => heat;

    public HeatedItem(ProcessableData itemData) : base(itemData)
    {
    }

    public override string GetDataAsString()
    {
        return $"[{GetRarity()}] {GetName()}\n" +
               $"Type: {GetItemType()}\n" +
               $"Value: {GetValue()}\n" +
               $"Heat: {Heat.Value}\n" +
               $"Description: {GetDescription()}";
    }

    public override Item Clone()
    {
        var clone = (HeatedItem)base.Clone();

        // Heat is per-instance so give the clone its own copy
        clone.heat = new Heat(heat.Value);

        return clone;
    }
}
