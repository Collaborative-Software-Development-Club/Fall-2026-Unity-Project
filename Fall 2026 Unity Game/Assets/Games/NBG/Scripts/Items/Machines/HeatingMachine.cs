// Runtime machine that heats items as they pass through, like the Furnace and Crucible.
// Paired with HeatingMachineData. Heating behavior lives here instead of on the base Machine.
public class HeatingMachine : Machine
{
    private readonly HeatApplicator _heatApplicator;

    public HeatingMachine(HeatingMachineData itemData) : base(itemData)
    {
        if (itemData.heatPerPass > 0f)
            _heatApplicator = new HeatApplicator(itemData.heatPerPass);
    }

    public HeatingMachine(HeatingMachineData itemData, string itemName) : base(itemData, itemName)
    {
        if (itemData.heatPerPass > 0f)
            _heatApplicator = new HeatApplicator(itemData.heatPerPass);
    }

    // Heat the outgoing item on its way out of the machine.
    protected override void OnItemRemovedFromOutput(Item item)
    {
        _heatApplicator?.ApplyTo(item);
    }
}
