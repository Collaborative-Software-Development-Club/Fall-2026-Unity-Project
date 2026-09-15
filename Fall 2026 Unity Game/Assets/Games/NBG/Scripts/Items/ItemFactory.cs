using System;
using UnityEngine;

public static class ItemFactory
{
    /// <summary>
    /// Creates a runtime Item from ItemData as a GameObject with a component.
    /// All implemented items (Machine, Brainrot, Lootbox) are MonoBehaviours.
    /// Tool, Seed, and Contract are not implemented yet and throw exceptions.
    /// </summary>
    public static Item CreateItemFromSO(ItemData data)
    {
        if (data == null) return null;

        switch (data.type)
        {
            case ItemType.None:
                Debug.LogWarning("Cannot create an item of type None");
                return null;

            case ItemType.Machine:
                if (data is not MachineData machineData) throw new InvalidCastException("ItemData is not MachineData");

                Machine machine = new Machine(machineData);
                return machine;
            case ItemType.Processable:
                if (data is not ProcessableData processableData) throw new InvalidCastException("ItemData is not ProcessableData");
                
                Processable processable = new Processable(processableData);
                return processable;
            default:
                throw new ArgumentOutOfRangeException($"Unhandled ItemType: {data.type}");
        }
    }

    public static GameObject CreateItemHolder(ItemData data)
    {
        if (data == null) return null;
        Item item = CreateItemFromSO(data);
        
        return ItemHolder.CreateObj(item);
    }
}