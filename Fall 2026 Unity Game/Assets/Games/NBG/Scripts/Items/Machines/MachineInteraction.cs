using System;
using UnityEngine;
using System.Collections.Generic;

public class MachineInteraction : MonoBehaviour, ILeftClickable, IHoverable
{
    [SerializeField] private MachineData machineData;

    private Machine _machine;
    public Action onMachineUsed; 
    public List<Item> itemsgiven;

    private void Awake()
    {
        if (machineData == null) return;
        _machine = new Machine(machineData); 
    }
    public string InteractionPrompt => "Open machine " + _machine.GetName(); 
    public bool Interact()
    {
        if (_machine == null)
        {
            Debug.LogWarning($"{_machine.GetName()}: no MachineData assigned.");
            return false;
        }

        int[] args = new int[2];

        return true;
    }

    public void OnLeftClick()
    {
        TextController.Instance.QueueNewText(_machine.GetInputInventory().ToString(), 2);
    }

    public void OnHoverEnter()
    {
        TextController.Instance.removePriorityFromQueue(1);
        TextController.Instance.QueueNewText(HoverMessage, 1);
    }

    public void OnHoverExit()
    {
        TextController.Instance.removeFromQueue(_machine.GetInputInventory().ToString());
        TextController.Instance.removeFromQueue(HoverMessage);
    }

    public string HoverMessage => _machine.GetName();
}