using System;
using UnityEngine;

/// <summary>
/// Self-contained heat state. Owns a single clamped heat value and the
/// operations that mutate it. Reusable by anything that needs to carry heat
/// (items, machines, etc.) without duplicating the clamping/mutation logic.
/// </summary>
[Serializable]
public class Heat
{
    [SerializeField] private float value;

    public Heat(float initial = 0f)
    {
        Set(initial);
    }

    /// <summary>Current heat value (never negative).</summary>
    public float Value => value;

    /// <summary>Sets the heat to an absolute value, clamped at zero.</summary>
    public void Set(float amount) => value = Mathf.Max(0f, amount);

    /// <summary>Adds heat. Negative amounts reduce heat (clamped at zero).</summary>
    public void Add(float amount) => Set(value + amount);

    /// <summary>Removes a flat amount of heat (clamped at zero).</summary>
    public void Dissipate(float amount) => Set(value - amount);

    public override string ToString() => value.ToString();
}
