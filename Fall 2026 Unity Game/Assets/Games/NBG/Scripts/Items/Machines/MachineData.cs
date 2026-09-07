using System.Collections.Generic;
using UnityEngine;

public enum machineType
{ 
    None,
}



[CreateAssetMenu(fileName = "New Machine", menuName = "Game/BrainrotMixer/Machine/Base Machine")]
public class MachineData : ItemData 
{
    public new string name = "Unnamed";
    public machineType processType = machineType.None;
    public Sprite texture;
    public int inputCount = 0;
    public int outputCount = 0;
    
    public List<Recipe> recipes = new List<Recipe>();
}