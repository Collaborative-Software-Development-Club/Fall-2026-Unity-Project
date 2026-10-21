using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

public class ConveyorManager : MonoBehaviour
{
    private List<List<ConveyorSection>> conveyors = new();

    [SerializeField] private float moveInterval = 0.5f;

    private float _elapsed;

    public void AddConveyorable(int lineIndex, IConveyorable newConveyorable)
    {
        var conveyorSection = conveyors[lineIndex];

        if (conveyorSection.Count == 0)
        {
            var newSection = new ConveyorSection();
            conveyorSection.Add(newSection);
            newSection.AddConveyorable(newConveyorable);
            return;
        }

        var sectionToAddTo = conveyorSection[^1];

        if (sectionToAddTo.GetSectionFull())
        {
            sectionToAddTo = new ConveyorSection();
            conveyors[lineIndex].Add(sectionToAddTo);
        }

        sectionToAddTo.AddConveyorable(newConveyorable);

    }

    public void AddNewChain(List<ConveyorSection> newChain)
    {
        conveyors.Add(newChain);
    }

    public IConveyorable RemoveLastConveyor(int lineIndex)
    {
        return conveyors[lineIndex][^1].RemoveLastConveyorable();
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
        if (conveyors.Count == 0) return;

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
    /// <summary>
    /// Splits a conveyor line at the specified conveyor
    /// TODO: AI GENERATED REMAKE WITH BETTER PERFORMANCE (this shouldn't be a triple for loop, could be optimized with dict / key system)
    /// </summary>
    public (int firstConveyorIndex, int secondConveyorIndex) SplitLineAtConveyor(Conveyor targetConveyor)
    {
        for (int lineIdx = 0; lineIdx < conveyors.Count; lineIdx++)
        {
            List<ConveyorSection> currentChain = conveyors[lineIdx];
            
            for (int sectionIdx = 0; sectionIdx < currentChain.Count; sectionIdx++)
            {
                ConveyorSection section = currentChain[sectionIdx];
                
                if (section.GetConveyors().Contains(targetConveyor))
                {
                    List<ConveyorSection> newChain = new List<ConveyorSection>();

                    for (int i = sectionIdx; i < currentChain.Count; i++)
                    {
                        newChain.Add(currentChain[i]);
                    }

                    currentChain.RemoveRange(sectionIdx, currentChain.Count - sectionIdx);

                    AddNewChain(newChain);

                    return (lineIdx, conveyors.Count - 1);
                }
            }
        }

        return (-1, -1); 
    }

    public void CombineLines(int firstLineIndex, int secondLineIndex)
    {
        conveyors[firstLineIndex].AddRange(conveyors[secondLineIndex]);
        conveyors.RemoveAt(secondLineIndex);
    }
    
    
}
