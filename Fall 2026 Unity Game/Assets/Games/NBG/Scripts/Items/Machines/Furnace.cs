public class Furnace : Machine
{
    public Furnace(MachineData itemData) : base(itemData)
    {
    }

    public bool Handler(int[] indexes)
    {
        return PerformOperation();
    }
}
