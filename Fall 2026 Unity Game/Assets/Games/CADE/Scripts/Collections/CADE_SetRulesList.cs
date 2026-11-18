using UnityEngine;

namespace CADE_SetRulesList
{
    public enum DoubleJumpRules
    {
        PitWinCondition = -8,
        LaunchBush = -7
    }

    public enum WallJumpRules
    {
        LimitedWallJumps = -8
    }

    public enum DashRules
    {
        SlipperyBlue = -8, 
        TimeLimit = -7
    }

    public enum StickyRules
    {
        FallDamage = -8
    }
}
