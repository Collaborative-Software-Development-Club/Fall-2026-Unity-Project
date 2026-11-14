using UnityEngine;

// Machine data that also heats items passing through, like the Furnace and Crucible
[CreateAssetMenu(fileName = "New Heating Machine", menuName = "Game/NBG/Machine/Heating Machine")]
public class HeatingMachineData : MachineData
{
    [Tooltip("Heat added to an item when it passes through this machine.")]
    public float heatPerPass = 0f;
}
