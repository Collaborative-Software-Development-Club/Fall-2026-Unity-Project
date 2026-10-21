using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
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
    public static void Place(MachineData machineData, UnboxableConveyorItem unboxableConveyorItem)
    {
        Debug.Log("attempt place");
        
       /*
        * TODO: Implement complex place,
        * I.E. waiting for conveyor to free up
        * than placing the machine prefab right where the conveyor goes
        * than hooking into into the conveyor manager
        */

       ConveyorManager conveyorManager = GameManager.Instance.conveyorManager;

       
       Collider2D boxCollider = unboxableConveyorItem.GetComponent<Collider2D>();
       
       List<Collider2D> contacts = new List<Collider2D>();
       
       boxCollider.Overlap(contacts);
       Debug.Log(contacts.Count);
       ConveyorRenderer conveyorRenderer = null;
       
       foreach (var contact in contacts)
       {
           Debug.Log(contact.gameObject.name);
           if (!contact.TryGetComponent<ConveyorRenderer>(out var conveyor)) continue;
           conveyorRenderer = conveyor;

           break;
       }

       Debug.Log("hello world");
       if (conveyorRenderer is null) return;
       Debug.Log("goodbye world");
       
       (int firstLineIdx, int secondLineIdx) = conveyorManager.SplitLineAtConveyor(conveyorRenderer.GetConveyor());

       // Add Machine data to line
       if (firstLineIdx == -1 || secondLineIdx == -1) return;
       Machine newMachine = ItemFactory.CreateItemFromSO(machineData) as Machine;
       if (newMachine == null) return;

       newMachine.AcceptsConveyorItems = false;
       conveyorManager.AddConveyorable(firstLineIdx, newMachine);
       conveyorManager.CombineLines(firstLineIdx, secondLineIdx);
       
       // Replace renderer with machine renderer
       GameObject rendererObj = conveyorRenderer.gameObject;
       Vector2 beltDirection = conveyorRenderer.GetDirection();
       MachineRenderer newMachineRenderer = rendererObj.AddComponent<MachineRenderer>();
       newMachineRenderer.Initialize(newMachine, beltDirection, ConveyorGenerator.ItemPrefab);
       conveyorRenderer.Destroy();
       unboxableConveyorItem.Destroy();
    }
}
