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
        TextController.Instance.SetText(_machine.GetInputInventory().ToString());
    }

    public void OnHoverEnter()
    {
        TextController.Instance.SetText(HoverMessage);
    }

    public void OnHoverExit()
    {
        String currentText = TextController.Instance.GetText();
        if (currentText == _machine.GetInputInventory().ToString() || currentText == HoverMessage)
        {
            TextController.Instance.ClearText();
        }
    }

    public string HoverMessage => _machine.GetName();
}