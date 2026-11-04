using System;
using System.Linq;
using UnityEngine.InputSystem;

namespace BindingTime
{
    public static class InputManager
    {
        private static readonly (Key[] keys, Action action)[] Bindings =
        {
            (new[] { Key.A, Key.LeftArrow }, () => Player.Instance.Move(-1, 0)),
            (new[] { Key.D, Key.RightArrow }, () => Player.Instance.Move(1, 0)),
            (new[] { Key.W, Key.UpArrow }, () => Player.Instance.Move(0, 1)),
            (new[] { Key.S, Key.DownArrow }, () => Player.Instance.Move(0, -1)),
            (new[] { Key.Space }, () => Player.Instance.Wait()),
            (new[] { Key.T }, () => TimeLoopManager.Instance.Loop()),
            (new[] { Key.R }, () => TimeLoopManager.Instance.RestartLevel()),
        };

        public static void Update()
        {
            Keyboard keyboard = Keyboard.current;
            if (keyboard == null || Player.Instance == null) return;

            foreach (var (keys, action) in Bindings)
            {
                if (keys.Any(k => keyboard[k].wasPressedThisFrame)) action();
            }
        }
    }
}
