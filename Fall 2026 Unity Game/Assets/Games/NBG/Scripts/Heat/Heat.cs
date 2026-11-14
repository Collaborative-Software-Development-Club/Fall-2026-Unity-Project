using System;
using UnityEngine;

// Holds the heat value for a single thing and keeps it from going negative
[Serializable]
public class Heat
{
    [SerializeField] private float value;

    public Heat(float initial = 0f)
    {
        Set(initial);
    }

    public float Value => value;

    // Clamped at zero so heat never goes negative
    public void Set(float amount) => value = Mathf.Max(0f, amount);

    public void Add(float amount) => Set(value + amount);

    public void Dissipate(float amount) => Set(value - amount);

    public override string ToString() => value.ToString();
}
