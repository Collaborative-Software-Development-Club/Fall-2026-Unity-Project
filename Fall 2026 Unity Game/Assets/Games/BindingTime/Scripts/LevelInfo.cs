using System.Collections.Generic;

namespace BindingTime
{
    // One tutorial message. It stays on screen until its trigger is met, then the next step shows.
    // Triggers: ticks:N  waits:N  sigil  loop  ghostsigil  never
    public class TutorialStep
    {
        public string Trigger;
        public string Text;
    }

    // Extra data a level file can carry on lines that start with @ (they are not part of the map):
    //   @name Level title
    //   @loops 3                       max loops (T presses) before time breaks
    //   @step sigil | Text to show     tutorial step with a trigger
    //   @tip Text to show              a hint that stays up for the whole level
    public class LevelInfo
    {
        public string Name = "";
        public int MaxLoops = int.MaxValue;
        public List<TutorialStep> Steps = new List<TutorialStep>();
    }
}
