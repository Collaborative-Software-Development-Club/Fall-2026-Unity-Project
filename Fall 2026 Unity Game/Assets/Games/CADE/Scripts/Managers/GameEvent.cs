using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/GameEvent")]
public class GameEvent : ScriptableObject
{
    public event Action OnEventRaised;
    public void Raise() => OnEventRaised?.Invoke();
}
