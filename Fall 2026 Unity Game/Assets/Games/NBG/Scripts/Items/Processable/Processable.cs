using UnityEngine;

public class Processable : Item 
{
    private ProcessableData ProcessableData => data as ProcessableData;
    
    public Processable(ProcessableData itemData)
    {
        data = itemData;
    }
}
