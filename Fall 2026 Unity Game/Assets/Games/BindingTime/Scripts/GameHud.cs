using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BindingTime
{
    public class GameHud : MonoBehaviour
    {
        private const float BreakPopupSeconds = 2.6f;
        private const string ControlsText = "Move: WASD / Arrow keys     Wait: Space     Loop: T     Restart: R     Hints: H";

        private GUIStyle panelStyle, breakStyle, textStyle, centerStyle, titleStyle, bigStyle, smallStyle;

        private bool hintsVisible = true;
        private bool timeBroken;
        private BreakReason breakReason;

        private int stepIndex;
        private int stepStartTick, stepStartWaits, stepStartGhosts;

        private static TimeLoopManager Loops => TimeLoopManager.Instance;

        void Start()
        {
            Loops.TimeBroke += OnTimeBroke;
            Loops.RunStarted += OnRunStarted;
        }

        void OnDestroy()
        {
            if (!TimeLoopManager.Exists) return;
            Loops.TimeBroke -= OnTimeBroke;
            Loops.RunStarted -= OnRunStarted;
        }

        void Update()
        {
            var kb = Keyboard.current;
            if (kb != null && kb[Key.H].wasPressedThisFrame) hintsVisible = !hintsVisible;

            LevelInfo info = LevelManager.Instance.CurrentInfo;
            if (timeBroken || stepIndex >= info.Steps.Count) return;

            if (TriggerMet(info.Steps[stepIndex]))
            {
                stepIndex++;
                BeginStep();
            }
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
            Loops.ForceRestart();
        }

        private void OnRunStarted()
        {
            stepIndex = 0;
            BeginStep();
        }

        private void BeginStep()
        {
            stepStartTick = Loops.CurrentTick;
            stepStartWaits = Player.Instance != null ? Player.Instance.WaitCount : 0;
            stepStartGhosts = Loops.GhostCount;
        }

        private bool TriggerMet(TutorialStep step)
        {
            Player player = Player.Instance;
            switch (step.Trigger)
            {
                case StepTrigger.Ticks: return Loops.CurrentTick - stepStartTick >= step.Count;
                case StepTrigger.Waits: return player != null && player.WaitCount - stepStartWaits >= step.Count;
                case StepTrigger.Loop: return Loops.GhostCount > stepStartGhosts;
                case StepTrigger.GhostSigil: return Loops.GhostOnSigil;
                case StepTrigger.Sigil: return player != null && TileManager.Instance.IsSigil(player.Coords);
                default: return false;
            }
        }

        void OnGUI()
        {
            if (LevelManager.Instance == null || !LevelManager.Instance.IsLoaded) return;

            int font = Mathf.Max(12, Screen.height / 42);
            BuildStyles(font);

            LevelInfo info = LevelManager.Instance.CurrentInfo;
            DrawStatus(font, info);
            if (hintsVisible && !timeBroken) DrawTutorial(font, info);
            DrawControlsBar(font);
            if (timeBroken) DrawTimeBroken(font);
        }

        private void BuildStyles(int fontSize)
        {
            if (panelStyle == null)
            {
                panelStyle = new GUIStyle(GUI.skin.box);
                panelStyle.normal.background = MakeTex(new Color(0.05f, 0.05f, 0.10f, 0.88f));

                breakStyle = new GUIStyle(GUI.skin.box);
                breakStyle.normal.background = MakeTex(new Color(0.35f, 0.02f, 0.05f, 0.95f));

                textStyle = new GUIStyle(GUI.skin.label) { richText = true, wordWrap = true, alignment = TextAnchor.UpperLeft };
                textStyle.normal.textColor = Color.white;

                centerStyle = new GUIStyle(textStyle) { alignment = TextAnchor.UpperCenter };

                titleStyle = new GUIStyle(textStyle);
                titleStyle.normal.textColor = new Color(1f, 0.85f, 0.3f);

                bigStyle = new GUIStyle(textStyle) { alignment = TextAnchor.MiddleCenter };

                smallStyle = new GUIStyle(textStyle) { alignment = TextAnchor.MiddleCenter };
                smallStyle.normal.textColor = new Color(0.8f, 0.8f, 0.85f);
            }

            textStyle.fontSize = fontSize;
            centerStyle.fontSize = fontSize;
            titleStyle.fontSize = Mathf.RoundToInt(fontSize * 0.85f);
            bigStyle.fontSize = fontSize * 2;
            smallStyle.fontSize = Mathf.RoundToInt(fontSize * 0.85f);
        }

        private static Texture2D MakeTex(Color c)
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, c);
            tex.Apply();
            return tex;
        }

        private void DrawStatus(int font, LevelInfo info)
        {
            string title = $"Level {LevelManager.Instance.CurrentLevel + 1} / {LevelManager.Instance.LevelCount}";
            if (!string.IsNullOrEmpty(info.Name)) title += $"  -  {info.Name}";

            bool limited = info.MaxLoops != int.MaxValue;
            string loopText = limited ? $"Loops: {Loops.GhostCount} / {info.MaxLoops}" : $"Loops: {Loops.GhostCount}";
            if (limited && Loops.GhostCount >= info.MaxLoops)
                loopText = $"<color=#ff6666>{loopText}  (one more breaks time!)</color>";

            string body = $"<b>{title}</b>\n{loopText}\n Tick: {Loops.CurrentTick} ";

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
            float innerW = w - font * 1.6f;
            float textH = textStyle.CalcHeight(new GUIContent(text), innerW);
            float h = textH + font * 3f;
            var rect = new Rect((Screen.width - w) / 2f, Screen.height - h - font * 3.2f, w, h);

            GUI.Box(rect, GUIContent.none, panelStyle);
            GUI.Label(new Rect(rect.x + font * 0.8f, rect.y + font * 0.4f, innerW, font * 1.6f), header, titleStyle);
            GUI.Label(new Rect(rect.x + font * 0.8f, rect.y + font * 1.8f, innerW, textH + font), text, textStyle);
        }

        private void DrawControlsBar(int font)
        {
            float h = font * 2f;
            GUI.Label(new Rect(0, Screen.height - h - font * 0.4f, Screen.width, h), ControlsText, smallStyle);
        }

        private void DrawTimeBroken(int font)
        {
            bool paradox = breakReason == BreakReason.Paradox;
            string title = paradox ? "<b>TIME PARADOX</b>" : "<b>TIME HAS BROKEN</b>";
            string body = paradox
                ? "You were inside a statue on the tick it closed.The timeline can't hold that. The level resets..."
                : "You looped too many times and the timeline shattered.The level resets...";

            float top = Screen.height * 0.32f;
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), GUIContent.none, breakStyle);
            GUI.Label(new Rect(0, top, Screen.width, font * 3.5f), title, bigStyle);
            GUI.Label(new Rect(Screen.width * 0.15f, top + font * 4f, Screen.width * 0.7f, font * 6f), body, centerStyle);
        }
    }
}
