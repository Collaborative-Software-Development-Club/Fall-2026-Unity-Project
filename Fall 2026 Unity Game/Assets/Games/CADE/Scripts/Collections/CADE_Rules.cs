using UnityEngine;

namespace CADE_RandomRules
{
    public enum DoubleJumpRules
    {
        PowerDowns = 0,
        LowGravity = 1
    }

    public enum WallJumpRules
    {
        PowerDowns = 0,
        TinyPlayer = 1
    }

    public enum DashRules
    {
        PowerDowns = 0,
        QuickTimeEvents = 1
    }

    public enum StickyRules
    {
        PowerDowns = 0,
        UpInverted = 1,
        DownInverted = 2,
        LeftDeathTimer = 3
    }
}
