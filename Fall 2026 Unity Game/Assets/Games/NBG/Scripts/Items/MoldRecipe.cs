using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Game/NBG/Recipe/Mold Recipe")]

public class MoldRecipe : Recipe
{
    [SerializeField] private ProcessableData mold;
}

