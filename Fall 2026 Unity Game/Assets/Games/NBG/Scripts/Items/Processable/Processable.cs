using UnityEngine;

public class Processable : Item 
{
    private ProcessableData ProcessableData => data as ProcessableData;
    
    public Processable(ProcessableData itemData)
    {
        data = itemData;
    }

    /// <summary>
    /// Function for retrieving the machine's data
    /// </summary>
    /// <returns></returns>
    public override ItemData GetData()
    {
        return data;
    }
}
