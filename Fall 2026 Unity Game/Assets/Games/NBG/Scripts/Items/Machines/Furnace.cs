using System.Threading.Tasks;
using UnityEngine;

public class Furnace : Machine
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Furnace(MachineData itemData) : base(itemData)
    {
        
    }
    


    public async Task<bool> Handler(int[] indexes) {

        
        return await PerformOperation();
    }



}
