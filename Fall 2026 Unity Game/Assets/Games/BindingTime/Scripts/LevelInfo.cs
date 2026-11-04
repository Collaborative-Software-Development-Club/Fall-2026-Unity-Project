using System.Collections.Generic;

namespace BindingTime
{
    public enum StepTrigger { Never, Ticks, Waits, Sigil, Loop, GhostSigil }

    public class TutorialStep
    {
        public StepTrigger Trigger;
        public int Count = 1;
        public string Text;

        // trigger text: ticks:N  waits:N  sigil  loop  ghostsigil  never
        public static TutorialStep Parse(string trigger, string text)
        {
            trigger = trigger.Trim().ToLowerInvariant();
            var step = new TutorialStep { Text = text.Trim() };

            string name = trigger;
            int colon = trigger.IndexOf(':');
            if (colon >= 0)
            {
                name = trigger.Substring(0, colon);
                int.TryParse(trigger.Substring(colon + 1), out step.Count);
            }

            step.Trigger = name switch
            {
                "ticks" => StepTrigger.Ticks,
                "waits" => StepTrigger.Waits,
                "sigil" => StepTrigger.Sigil,
                "loop" => StepTrigger.Loop,
                "ghostsigil" => StepTrigger.GhostSigil,
                _ => StepTrigger.Never,
            };
            return step;
        }
    }

    public class LevelInfo
    {
        public string Name = "";
        public int MaxLoops = int.MaxValue;
        public List<TutorialStep> Steps = new List<TutorialStep>();
    }
}
