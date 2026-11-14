public class Mixer : Machine
{
    public Mixer(MachineData itemData) : base(itemData)
    {
    }

    public bool Handler(int[] indexes)
    {
        return PerformOperation();
    }
}
