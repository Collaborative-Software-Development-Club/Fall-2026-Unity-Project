using UnityEngine;

/// <summary>
/// Data for a machine that applies heat to items passing through it
/// (e.g. Furnace, Crucible). Heat is opt-in: plain machines use
/// <see cref="MachineData"/> and never add heat.
/// </summary>
[CreateAssetMenu(fileName = "New Heating Machine", menuName = "Game/NBG/Machine/Heating Machine")]
public class HeatingMachineData : MachineData
{
    [Tooltip("Heat added to an item when it passes through this machine.")]
    public float heatPerPass = 0f;
}
