public class Crucible : Machine
{
    public Crucible(MachineData itemData) : base(itemData)
    {
    }

    public bool Handler(int[] indexes)
    {
        return PerformOperation();
    }
}
