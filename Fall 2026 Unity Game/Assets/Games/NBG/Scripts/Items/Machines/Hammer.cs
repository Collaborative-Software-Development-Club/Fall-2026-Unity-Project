public class Hammer : Machine
{
    public Hammer(MachineData itemData) : base(itemData)
    {
    }

    public bool Handler(int[] indexes)
    {
        return PerformOperation();
    }
}
