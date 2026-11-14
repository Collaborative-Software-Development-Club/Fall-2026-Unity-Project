public class Grinder : Machine
{
    public Grinder(MachineData itemData) : base(itemData)
    {
    }

    public bool Handler(int[] indexes)
    {
        return PerformOperation();
    }
}
