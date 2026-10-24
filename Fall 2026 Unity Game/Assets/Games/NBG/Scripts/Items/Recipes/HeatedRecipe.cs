using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Game/NBG/Recipe/Heated Recipe")]

public class HeatedRecipe : Recipe
{
    [SerializeField] private int heatRequired;
}

