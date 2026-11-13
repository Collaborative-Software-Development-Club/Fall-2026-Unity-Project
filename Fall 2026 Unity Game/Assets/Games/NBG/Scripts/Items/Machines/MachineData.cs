using System.Collections.Generic;
using UnityEngine;

public enum machineType
{ 
    None, Furnace, Hammer, Crucible, Grinder, Mixer
    
}



[CreateAssetMenu(fileName = "New Machine", menuName = "Game/NBG/Machine/Base Machine")]
public class MachineData : ItemData 
{
    public new string name = "Unnamed";
    public machineType processType = machineType.None;
    public Sprite texture;
    public int inputCount = 0;
    public int outputCount = 0;

    [Tooltip("Heat added to an item when it enters this machine.")]
    public float heatPerPass = 0f;
        
    public List<Recipe> recipes = new List<Recipe>();
}