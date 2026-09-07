using UnityEngine;

public abstract class Item 
{
    [SerializeField] protected ItemData data;
    protected string Name;

    public void PrintData()
    {
        if(!HasData())
        {
            Debug.LogWarning($"{Name}: ItemData is NULL");
            return;
        }

        Debug.Log(GetDataAsString());
    }

    public virtual string GetDataAsString()
    {
        return $"[{GetRarity()}] {GetName()}\n" +
               $"Type: {GetItemType()}\n" +
               $"Value: {GetValue()}\n" +
               $"Description: {GetDescription()}";
    }
    
    public virtual string GetDisplayName()
    {
        return GetRarity() switch
        {
            Rarity.Common => GetName(),
            _ => $"{GetRarity()} {GetName()}"
        };
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
        return data.name;
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
        
        return clone;
    }
}