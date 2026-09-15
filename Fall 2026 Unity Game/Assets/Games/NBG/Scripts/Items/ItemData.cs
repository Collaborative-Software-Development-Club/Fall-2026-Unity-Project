using UnityEngine;

public enum ItemType
{
    None,
    Machine,
    Processable
}

public enum Rarity
{
    None,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "New Item", menuName = "Game/NBG/Item/Base Item")]
public class ItemData : ScriptableObject
{
    public string text;
    public double value = 0;
    public Sprite icon;
    public ItemType type = ItemType.None;
    public Rarity rarity = Rarity.None;
    public bool isConsumable = false;
    public bool isStackable = true;
}