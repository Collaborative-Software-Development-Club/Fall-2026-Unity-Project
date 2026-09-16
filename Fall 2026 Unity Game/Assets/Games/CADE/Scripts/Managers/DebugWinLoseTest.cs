using UnityEngine;
using UnityEngine.InputSystem;

public class DebugWinLoseTrigger : MonoBehaviour
{
    [SerializeField] private GameEvent winEvent;
    [SerializeField] private GameEvent loseEvent;

    private void Update()
    {
        if (Keyboard.current == null) return;

        if (Keyboard.current[Key.Digit1].wasPressedThisFrame)
        {
            winEvent.Raise();
        }

        if (Keyboard.current[Key.Digit0].wasPressedThisFrame)
        {
            loseEvent.Raise();
        }
    }
}