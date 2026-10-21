using UnityEngine;
using UnityEngine.InputSystem;

namespace BindingTime
{
    // Small "how to play" overlay. Shows on startup; press H (or any key while it is open) to toggle.
    // Creates itself at runtime, so there is nothing to add to the scene.
    public class RulesScreen : MonoBehaviour
    {
        private const string Rules =
            "<b>BINDING TIME</b>\n\n" +
            "<b>Move</b>   WASD / arrows        <b>Wait</b>   Space\n" +
            "<b>T</b>   Loop: your run becomes a ghost       <b>R</b>   Restart level\n\n" +
            "<b>Time</b>   Every move or wait is one tick.\n" +
            "<b>Ghosts</b>   Repeat your last run forever, one lap per recorded tick,\n" +
            "then snap back to the start. You always start at the same tick as them.\n" +
            "To hold a sigil longer, wait on it before pressing T.\n\n" +
            "<b>Sigils</b>   Powered while you or a ghost stand on them.\n" +
            "<b>Statues</b>   Same colour as a sigil = linked. Normal statues open\n" +
            "while powered. Inverted statues are open until powered, then close.\n" +
            "Anything standing in a statue keeps it open.\n\n" +
            "<b>Goal</b>   Reach the gold exit.\n\n" +
            "<i>Press H to show / hide this screen</i>";

        private bool visible = true;
        private GUIStyle boxStyle, textStyle;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Spawn()
        {
            if (FindFirstObjectByType<RulesScreen>() != null) return;
            new GameObject("RulesScreen").AddComponent<RulesScreen>();
        }

        void Update()
        {
            var kb = Keyboard.current;
            if (kb == null) return;
            if (kb[Key.H].wasPressedThisFrame) visible = !visible;
            else if (visible && kb.anyKey.wasPressedThisFrame && !kb[Key.H].isPressed) visible = false;
        }

        void OnGUI()
        {
            if (!visible) return;

            int fontSize = Mathf.Max(12, Screen.height / 38);
            if (textStyle == null)
            {
                textStyle = new GUIStyle(GUI.skin.label) { richText = true, wordWrap = true, alignment = TextAnchor.UpperLeft };
                textStyle.normal.textColor = Color.white;
                boxStyle = new GUIStyle(GUI.skin.box);
                var tex = new Texture2D(1, 1);
                tex.SetPixel(0, 0, new Color(0.05f, 0.05f, 0.1f, 0.92f));
                tex.Apply();
                boxStyle.normal.background = tex;
            }
            textStyle.fontSize = fontSize;

            float w = Mathf.Min(Screen.width * 0.9f, fontSize * 36f);
            float h = fontSize * 21f;
            var rect = new Rect((Screen.width - w) / 2f, (Screen.height - h) / 2f, w, h);
            GUI.Box(rect, GUIContent.none, boxStyle);
            GUI.Label(new Rect(rect.x + fontSize, rect.y + fontSize, w - fontSize * 2, h - fontSize * 2), Rules, textStyle);
        }
    }
}