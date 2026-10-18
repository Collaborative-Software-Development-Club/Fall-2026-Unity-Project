using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ConveyorManager : MonoBehaviour 
{
    private List<List<ConveyorSection>> conveyors;
    
    [SerializeField] private float moveInterval = 0.5f;

    private float _elapsed;
    
    public void AddConveyor(int lineIndex)
    {
        var conveyorSection = conveyors[lineIndex];
        
        var newConveyor = new Conveyor();
        var sectionToAddTo = conveyorSection[^1];
        

        if (sectionToAddTo.GetFull())
        {
            sectionToAddTo = new ConveyorSection();
            conveyors[lineIndex].Add(sectionToAddTo);
        }
        
        sectionToAddTo.AddConveyor(newConveyor);
        
    }

    public Conveyor RemoveLastConveyor(int lineIndex)
    {
        return conveyors[lineIndex][^1].RemoveLastConveyor();
    }

    private void Update()
    {
        _elapsed += Time.deltaTime;

        while (_elapsed >= moveInterval)
        {
            _elapsed -= moveInterval;
            AdvanceItems();
        }
    }

    private void AdvanceItems()
    {
        foreach (var chains in conveyors)
        {
            foreach (var sections in chains)
            {
                sections.Advance();
                if (!sections.GetFull()) continue;
            }
        }
    }
}
