using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace BindingTime {
    public static class UIManager {
        public static Player player;

        public static Dictionary<Key[], Action> HoldKeyBindings = new() {
            {new[] {Key.A, Key.LeftArrow}, () => {}},
            {new[] {Key.D, Key.RightArrow}, () => {}},
            {new[] {Key.W, Key.UpArrow}, () => {}},
            {new[] {Key.S, Key.DownArrow}, () => {}},
            {new[] {Key.Space}, () => {}},
        };

        public static Dictionary<Key[], Action> TapKeyBindings = new() {
            {new[] {Key.A, Key.LeftArrow}, () => player.Move(-1, 0)},
            {new[] {Key.D, Key.RightArrow}, () => player.Move(1, 0)},
            {new[] {Key.W, Key.UpArrow}, () => player.Move(0, 1)},
            {new[] {Key.S, Key.DownArrow}, () => player.Move(0, -1)},
            {new[] {Key.Space}, () => {}},
        };

        public static void Update() {
            var keyboard = Keyboard.current;
            //hold actions
            foreach (var (keySet, command) in HoldKeyBindings) {
                if (keySet.Any(k => keyboard[k].isPressed)) command();
            }

            //tap actions
            foreach (var (keySet, command) in TapKeyBindings) {
                if (keySet.Any(k => keyboard[k].wasPressedThisFrame)) command();
            }
        }
    }
}
