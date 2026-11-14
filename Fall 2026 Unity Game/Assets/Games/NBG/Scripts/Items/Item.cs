using System;
using UnityEngine;
[Serializable]
public abstract class Item : IHeatable
{
    [SerializeField] protected ItemData data;
    protected string Name;

    // Runtime heat, per instance (not on the shared ItemData asset).
    [SerializeField] protected Heat heat = new Heat();

    public Heat Heat => heat;

    public void PrintData()
    {
        if(!HasData())
        {
            Debug.LogWarning($"{Name}: ItemData is NULL");
            return;
        }

        Debug.Log(GetDataAsString());
    }

    public abstract ItemData GetData();
    

    public virtual string GetDataAsString()
    {
        return $"[{GetRarity()}] {GetName()}\n" +
               $"Type: {GetItemType()}\n" +
               $"Value: {GetValue()}\n" +
               $"Heat: {Heat.Value}\n" +
               $"Description: {GetDescription()}";
    }
    
    public virtual string GetDisplayName()
    {
        return GetName();
        /*
        return GetRarity() switch
        {
            Rarity.None => GetName(),
            Rarity.Common => GetName(),
            _ => $"{GetRarity()} {GetName()}"
        };
        */
    }
    public virtual string GetTooltip()
    {
        return
            $"{GetDisplayName()}\n" +
            $"{GetDescription()}\n\n" +
            $"Value: {GetValue()}";
    }
    public virtual string GetName()
    {
        return data.getName();
    }
    public virtual string GetDescription()
    {
        return data.text;
    }
    public virtual double GetValue()
    {
        return data.value;
    }
    public virtual ItemType GetItemType()
    {
        //Debug.Log(data is null);
        return data.type;
    }
    public virtual Rarity GetRarity()
    {
        return data.rarity;
    }
    public virtual Sprite GetIcon()
    {
        return data.icon;
    }
    public virtual void Consume()
    {
        if (!data.isConsumable) return;
    }
    public bool HasData()
    {
        return !(data is null);
    }
    protected virtual void OnValidate()
    {
        if (HasData()) return;
        
        Debug.LogWarning($"{Name} has no ItemData assigned");
    }
    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;

        if (ReferenceEquals(this, obj))
            return true;

        if (obj.GetType() != GetType())
            return false;

        var other = (Item)obj;

        return data == other.data;
    }

    public override int GetHashCode()
    {
        return data != null ? data.GetHashCode() : 0;
    }
    public virtual Item Clone()
    {
        var clone = (Item)MemberwiseClone();

        // Heat is per-instance so give the clone its own copy
        clone.heat = new Heat(heat.Value);

        return clone;
    }

    public bool GetStackable()
    {
        return data.isStackable;
    }


}