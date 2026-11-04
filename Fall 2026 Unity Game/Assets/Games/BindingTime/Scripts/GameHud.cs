using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BindingTime
{
    // HUD + step-by-step tutorial + the "time has broken" popup.
    // Created by GameManager.Start(), so it only exists in this game's scene.
    public class GameHud : MonoBehaviour
    {
        private const float BreakPopupSeconds = 2.6f;

        private GUIStyle panelStyle, textStyle, titleStyle, bigStyle, smallStyle;
        private Texture2D panelTex, breakTex;

        private bool hintsVisible = true;
        private bool timeBroken;
        private BreakReason breakReason;

        // tutorial progress
        private int stepIndex;
        private int stepStartTick, stepStartWaits, stepStartGhosts;

        void Start()
        {
            TimeLoopManager.Instance.TimeBroke += OnTimeBroke;
            TimeLoopManager.Instance.RunStarted += OnRunStarted;
        }

        void OnDestroy()
        {
            if (TimeLoopManager.Instance == null) return;
            TimeLoopManager.Instance.TimeBroke -= OnTimeBroke;
            TimeLoopManager.Instance.RunStarted -= OnRunStarted;
        }

        private void OnTimeBroke(BreakReason reason)
        {
            if (timeBroken) return;
            breakReason = reason;
            StartCoroutine(BreakRoutine());
        }

        private IEnumerator BreakRoutine()
        {
            timeBroken = true;
            yield return new WaitForSeconds(BreakPopupSeconds);
            timeBroken = false;
            TimeLoopManager.Instance.ForceRestart();
        }

        private void OnRunStarted()
        {
            stepIndex = 0;
            BeginStep();
        }

        private void BeginStep()
        {
            stepStartTick = TimeLoopManager.Instance.CurrentTick;
            stepStartWaits = UIManager.player != null ? UIManager.player.WaitCount : 0;
            stepStartGhosts = TimeLoopManager.Instance.GhostCount;
        }

        void Update()
        {
            var kb = Keyboard.current;
            if (kb != null && kb[Key.H].wasPressedThisFrame) hintsVisible = !hintsVisible;

            LevelInfo info = LevelManager.Instance.CurrentInfo;
            if (timeBroken || stepIndex >= info.Steps.Count) return;

            if (TriggerMet(info.Steps[stepIndex].Trigger))
            {
                stepIndex++;
                BeginStep();
            }
        }

        private bool TriggerMet(string trigger)
        {
            var loops = TimeLoopManager.Instance;
            Player player = UIManager.player;

            string name = trigger;
            int number = 1;
            int colon = trigger.IndexOf(':');
            if (colon >= 0)
            {
                name = trigger.Substring(0, colon);
                int.TryParse(trigger.Substring(colon + 1), out number);
            }

            switch (name)
            {
                case "ticks": return loops.CurrentTick - stepStartTick >= number;
                case "waits": return player != null && player.WaitCount - stepStartWaits >= number;
                case "loop": return loops.GhostCount > stepStartGhosts;
                case "ghostsigil": return loops.GhostOnSigil;
                case "sigil":
                    if (player == null) return false;
                    Tile t = TileManager.Instance.GetTile(player.Coords.x, player.Coords.y);
                    return t != null && t.BaseType == TileType.Sigil;
                default: return false; // "never" and anything unknown: stay on this step
            }
        }

        private void BuildStyles(int fontSize)
        {
            if (panelStyle == null)
            {
                panelTex = MakeTex(new Color(0.05f, 0.05f, 0.10f, 0.88f));
                breakTex = MakeTex(new Color(0.35f, 0.02f, 0.05f, 0.95f));

                panelStyle = new GUIStyle(GUI.skin.box);
                panelStyle.normal.background = panelTex;

                textStyle = new GUIStyle(GUI.skin.label) { richText = true, wordWrap = true, alignment = TextAnchor.UpperLeft };
                textStyle.normal.textColor = Color.white;

                titleStyle = new GUIStyle(textStyle);
                titleStyle.normal.textColor = new Color(1f, 0.85f, 0.3f);

                bigStyle = new GUIStyle(textStyle) { alignment = TextAnchor.MiddleCenter };
                smallStyle = new GUIStyle(textStyle) { alignment = TextAnchor.MiddleCenter };
                smallStyle.normal.textColor = new Color(0.8f, 0.8f, 0.85f);
            }
            textStyle.fontSize = fontSize;
            titleStyle.fontSize = Mathf.RoundToInt(fontSize * 0.85f);
            bigStyle.fontSize = Mathf.RoundToInt(fontSize * 2f);
            smallStyle.fontSize = Mathf.RoundToInt(fontSize * 0.85f);
        }

        private static Texture2D MakeTex(Color c)
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, c);
            tex.Apply();
            return tex;
        }

        void OnGUI()
        {
            if (LevelManager.Instance == null || !LevelManager.Instance.IsLoaded) return;

            int font = Mathf.Max(12, Screen.height / 42);
            BuildStyles(font);

            LevelInfo info = LevelManager.Instance.CurrentInfo;
            TimeLoopManager loops = TimeLoopManager.Instance;

            DrawStatus(font, info, loops);
            if (hintsVisible && !timeBroken) DrawTutorial(font, info);
            DrawControlsBar(font);
            if (timeBroken) DrawTimeBroken(font);
        }

        private void DrawStatus(int font, LevelInfo info, TimeLoopManager loops)
        {
            string title = $"Level {LevelManager.Instance.CurrentLevel + 1} / {LevelManager.Instance.LevelCount}";
            if (!string.IsNullOrEmpty(info.Name)) title += $"  -  {info.Name}";

            bool limited = info.MaxLoops != int.MaxValue;
            string loopText = limited ? $"Loops: {loops.GhostCount} / {info.MaxLoops}" : $"Loops: {loops.GhostCount}";
            bool last = limited && loops.GhostCount >= info.MaxLoops;
            if (last) loopText = $"<color=#ff6666>{loopText}  (one more breaks time!)</color>";

            string body = $"<b>{title}</b>\n{loopText}\nTick: {loops.CurrentTick}";

            float w = font * 22f, h = font * 5.2f;
            var rect = new Rect(font * 0.6f, font * 0.6f, w, h);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUI.Label(new Rect(rect.x + font * 0.6f, rect.y + font * 0.4f, w - font * 1.2f, h - font * 0.8f), body, textStyle);
        }

        private void DrawTutorial(int font, LevelInfo info)
        {
            if (stepIndex >= info.Steps.Count) return;

            string text = info.Steps[stepIndex].Text;
            string header = info.Steps.Count > 1 ? $"TUTORIAL  {stepIndex + 1}/{info.Steps.Count}" : "HINT";

            float w = Mathf.Min(Screen.width * 0.9f, font * 40f);
            float textH = textStyle.CalcHeight(new GUIContent(text), w - font * 1.6f);
            float h = textH + font * 3f;
            var rect = new Rect((Screen.width - w) / 2f, Screen.height - h - font * 3.2f, w, h);
            GUI.Box(rect, GUIContent.none, panelStyle);
            GUI.Label(new Rect(rect.x + font * 0.8f, rect.y + font * 0.4f, w - font * 1.6f, font * 1.6f), header, titleStyle);
            GUI.Label(new Rect(rect.x + font * 0.8f, rect.y + font * 1.8f, w - font * 1.6f, textH + font), text, textStyle);
        }

        private void DrawControlsBar(int font)
        {
            string text = "Move: WASD / Arrow keys     Wait: Space     Loop: T     Restart: R     Hints: H";
            float w = Screen.width;
            float h = font * 2f;
            GUI.Label(new Rect(0, Screen.height - h - font * 0.4f, w, h), text, smallStyle);
        }

        private void DrawTimeBroken(int font)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none, new GUIStyle(panelStyle) { normal = { background = breakTex } });
            var rect = new Rect(0, Screen.height * 0.32f, Screen.width, font * 3.5f);
            bool paradox = breakReason == BreakReason.Paradox;
            GUI.Label(rect, paradox ? "<b>TIME PARADOX</b>" : "<b>TIME HAS BROKEN</b>", bigStyle);
            var rect2 = new Rect(Screen.width * 0.15f, Screen.height * 0.32f + font * 4f, Screen.width * 0.7f, font * 6f);
            var center = new GUIStyle(textStyle) { alignment = TextAnchor.UpperCenter };
            GUI.Label(rect2, paradox
                ? "You were inside a statue on the tick it closed.\nThe timeline can't hold that. The level resets..."
                : "You looped too many times and the timeline shattered.\nThe level resets...", center);
        }
    }
}
