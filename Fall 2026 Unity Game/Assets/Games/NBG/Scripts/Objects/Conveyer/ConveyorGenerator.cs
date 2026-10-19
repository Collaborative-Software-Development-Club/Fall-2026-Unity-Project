using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a handmade script to generate a certain conveyor this shouldn't be used more than once
/// </summary>
public class ConveyorGenerator : MonoBehaviour
{
    private Vector2Int size = new Vector2Int(10, 10);

    private void Start()
    {
        List<ConveyorSection> chain =  new List<ConveyorSection>();
        
        ConveyorSection section = new ConveyorSection();
        
        for (int i = 0; i < size.x; i++)
        {
            for (int j = 0; j < size.y; j++)
            {
               if(i != 0 && i != size.x - 1 && j != 0 && j != size.y - 1) continue;

               if (section.GetSectionFull())
               {
                   chain.Add(section);
                   section = new ConveyorSection();
               }

               Conveyor conveyor = new Conveyor();
               section.AddConveyor(conveyor);
            }
        }
        
        chain.Add(section);
        GameManager.Instance.conveyorManager.AddNewChain(chain);
    }
}
