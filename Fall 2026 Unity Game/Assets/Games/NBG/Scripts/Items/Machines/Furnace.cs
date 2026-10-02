using UnityEngine;

public class Furnace : Machine
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Furnace(MachineData itemData) : base(itemData)
    {
        
    }
    


    public bool Handler(int[] indexes) {

        
        return PerformOperation();
    }



}
