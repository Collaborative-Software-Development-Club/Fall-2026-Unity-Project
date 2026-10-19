/// <summary>
/// This makes it so something can be put into a conveyor belt line
/// </summary>
public interface IConveyorable
{
   public int GetSize();
   public bool AddItemQuantityToPart(ItemQuantity itemQuantity);
   public ItemQuantity RemoveLastItemFromPart();
   public bool DoAction(IConveyorable nextConveyorable);
   public bool HasSpace();
}
