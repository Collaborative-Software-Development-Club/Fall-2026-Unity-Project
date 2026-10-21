using UnityEditor;
using UnityEngine;

public class MachinePlace
{
    /// <summary>
    /// THIS IS A VERY MUCH TEMP FILE AND SHOULD BE HEAVILY TWEAKED IN THE FUTURE
    /// </summary>
    /// <param name="conveyor"></param>
    /// <param name="machineData"></param>
    /// <param name="conveyorRenderer"></param>
    /// <param name="unboxableConveyorItem"></param>
    public static void Place(Conveyor conveyor, MachineData machineData, ConveyorRenderer conveyorRenderer, UnboxableConveyorItem unboxableConveyorItem)
    {
       /*
        * TODO: Implement complex place,
        * I.E. waiting for conveyor to free up
        * than placing the machine prefab right where the conveyor goes
        * than hooking into into the conveyor manager
        */

       conveyor.LockInput();
       ConveyorManager conveyorManager = GameManager.Instance.conveyorManager;
       
       (int firstLineIdx, int secondLineIdx) = conveyorManager.SplitLineAtConveyor(conveyor);

       // Add Machine data to line
       if (firstLineIdx == -1 || secondLineIdx == -1) return;
       Machine newMachine = ItemFactory.CreateItemFromSO(machineData) as Machine;
       conveyorManager.AddConveyorable(firstLineIdx, newMachine);
       conveyorManager.CombineLines(firstLineIdx, secondLineIdx);
       
       // Replace renderer with machine renderer
       GameObject rendererObj = conveyorRenderer.gameObject;
       MachineRenderer newMachineRenderer = rendererObj.AddComponent<MachineRenderer>();
       newMachineRenderer.Initialize(newMachine);
       conveyorRenderer.Destroy();
       unboxableConveyorItem.Destroy();
    }
}
