using System;
using UnityEngine;
using System.Collections.Generic;

public class MachineInteraction : MonoBehaviour
{
    [SerializeField] private MachineData machineData;

    [SerializeField] private bool hasUI = false;
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

        Action machineHandler = () =>
        {
            _machine.SetFunctionality(args);
        };

        machineHandler += () => onMachineUsed?.Invoke();        
        if (hasUI){}
            //GameManager.Instance.GUIManager.OpenMachineUI(_machine, machineHandler);
        else
            machineHandler.Invoke();

        return true;
    } 
}