using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ConveyorManager : MonoBehaviour 
{
    private List<List<ConveyorSection>> conveyors = new();
    
    [SerializeField] private float moveInterval = 0.5f;

    private float _elapsed;
    
    public void AddConveyor(int lineIndex)
    {
        var conveyorSection = conveyors[lineIndex];
        
        var newConveyor = new Conveyor();
        var sectionToAddTo = conveyorSection[^1];
        

        if (sectionToAddTo.GetSectionFull())
        {
            sectionToAddTo = new ConveyorSection();
            conveyors[lineIndex].Add(sectionToAddTo);
        }
        
        sectionToAddTo.AddConveyor(newConveyor);
        
    }

    public void AddNewChain(List<ConveyorSection> newChain)
    {
        conveyors.Add(newChain);
    }
    public Conveyor RemoveLastConveyor(int lineIndex)
    {
        return conveyors[lineIndex][^1].RemoveLastConveyor();
    }

    private void FixedUpdate()
    {
        _elapsed += Time.deltaTime;

        if (!(_elapsed >= moveInterval)) return;

        _elapsed = 0;
        AdvanceItems();
    }

    private void AdvanceItems()
    {
        if(conveyors.Count == 0) return;
        
        // This goes through all conveyor chains in the game
        foreach (var chains in conveyors)
        {
            // This goes through a chain of conveyors
            for (int i = chains.Count - 2; i >= 0; i--)
            {
                ConveyorSection section = chains[i];
                
                section.Advance(chains[i + 1]);
            }
        }
    }
}
