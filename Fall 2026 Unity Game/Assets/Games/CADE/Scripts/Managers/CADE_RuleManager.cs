using System;
using System.Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CADE_RuleManager : MonoBehaviour
{
    public UnityEvent InvertPowerUp;
    public UnityEvent LowGravity;
    public UnityEvent TinyPlayer;
    public UnityEvent QuickTimeEvents;
    public UnityEvent UpInverted;
    public UnityEvent DownInverted;
    public UnityEvent LeftDeathTimer;

    public void ApplyRandomRule(int ruleId, CADE_SceneList.Scenes currentScene)
    {
        if (currentScene == CADE_SceneList.Scenes.CADE_DoubleJump)
        {
            int length = Enum.GetNames(typeof(CADE_RandomRulesList.DoubleJumpRules)).Length;

            if (ruleId > length || ruleId < 0)
            {
                Debug.Log("Invalid Double Jump Rule Id.");
                return;
            }
            else
            {
                InvokeDoubleJumpRuleEvent(ruleId);
            }
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_WallJump)
        {
            int length = Enum.GetNames(typeof(CADE_RandomRulesList.WallJumpRules)).Length;

            if (ruleId > length || ruleId < 0)
            {
                Debug.Log("Invalid Wall Jump Rule Id.");
                return;
            }
            else
            {
                InvokeWallJumpRuleEvent(ruleId);
            }
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_Dash)
        {
            int length = Enum.GetNames(typeof(CADE_RandomRulesList.DashRules)).Length;

            if (ruleId > length || ruleId < 0)
            {
                Debug.Log("Invalid Dash Rule Id.");
                return;
            }
            else
            {
                InvokeDashRuleEvent(ruleId);
            }
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_Sticky)
        {
            int length = Enum.GetNames(typeof(CADE_RandomRulesList.StickyRules)).Length;

            if (ruleId > length || ruleId < 0)
            {
                Debug.Log("Invalid Sticky Rule Id.");
                return;
            }
            else
            {
                InvokeStickyRuleEvent(ruleId);
            }
        }    
    }


    private void InvokeDoubleJumpRuleEvent(int ruleId)
    {
        if ((CADE_RandomRulesList.DoubleJumpRules)ruleId == CADE_RandomRulesList.DoubleJumpRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs.");
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.DoubleJumpRules)ruleId == CADE_RandomRulesList.DoubleJumpRules.LowGravity)
        {
            Debug.Log("Rule Applied: You are on the moon.");
            LowGravity.Invoke();
        }
    }

    private void InvokeWallJumpRuleEvent(int ruleId)
    {
        if ((CADE_RandomRulesList.WallJumpRules)ruleId == CADE_RandomRulesList.WallJumpRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs.");
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.WallJumpRules)ruleId == CADE_RandomRulesList.WallJumpRules.TinyPlayer)
        {
            Debug.Log("Rule Applied: You must be able to fit in a tin can to play.");
            TinyPlayer.Invoke();
        }
    }

    private void InvokeDashRuleEvent(int ruleId)
    {
        if ((CADE_RandomRulesList.DashRules)ruleId == CADE_RandomRulesList.DashRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs");
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.DashRules)ruleId == CADE_RandomRulesList.DashRules.QuickTimeEvents)
        {
            Debug.Log("Rule Applied: You must complete random quick time events or explode.");
            QuickTimeEvents.Invoke();
        }
    }

    private void InvokeStickyRuleEvent(int ruleId)
    {
        if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs");
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.UpDownInverted)
        {
            Debug.Log("Rule Applied: Up is actually down, and vice versa.");
            UpInverted.Invoke();
        }
        else if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.LeftDeathTimer)
        {
            Debug.Log("Rule Applied: You must not move to the left for more than three seconds or you'll explode.");
            LeftDeathTimer.Invoke();
        }
    }
}
