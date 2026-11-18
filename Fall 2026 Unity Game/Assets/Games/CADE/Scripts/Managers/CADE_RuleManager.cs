using System;
using System.Collections.Generic;
using System.Data;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.WSA;

public class CADE_RuleManager : MonoBehaviour
{
    public UnityEvent InvertPowerUp;
    public UnityEvent LowGravity;
    public UnityEvent TinyPlayer;
    public UnityEvent QuickTimeEvents;
    public UnityEvent UpDownInverted;
    public UnityEvent LeftRightInverted;
    public UnityEvent LeftDeathTimer;

    public UnityEvent<int, string> AddRule;

    private Dictionary<int, string> rules;

    int ruleCounter = 0;


    private void Start()
    {
        //rulesString = " ";
    }

    
    public void ApplySetRules(CADE_SceneList.Scenes currentScene)
    {

        if (currentScene == CADE_SceneList.Scenes.CADE_DoubleJump)
        {
            AddRule.Invoke((int)CADE_SetRulesList.DoubleJumpRules.PitWinCondition, "\n- To win, you must fall to your death in the level's only pit.\n");
            AddRule.Invoke((int)CADE_SetRulesList.DoubleJumpRules.LaunchBush, "\n - The bushes launch you if you press the jump button.\n");

            Debug.Log("Added Set Rules");

        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_WallJump)
        {
            AddRule.Invoke((int)CADE_SetRulesList.WallJumpRules.LimitedWallJumps, "\n- 5 Consectuive wall jumps kill you.\n");
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_Dash)
        {
            AddRule.Invoke((int)CADE_SetRulesList.DashRules.SlipperyBlue, "\n- Blue platforms are slippery.\n");
            AddRule.Invoke((int)CADE_SetRulesList.DashRules.TimeLimit, "\n- Must beat the level in 50 seconds.\n");
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_Sticky)
        {
            AddRule.Invoke((int)CADE_SetRulesList.StickyRules.FallDamage, "\n- There is fall damage.\n");
        }

    }


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
                string ruleDesc = InvokeDoubleJumpRuleEvent(ruleId);

                AddRule.Invoke(ruleId, ruleDesc);
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
                AddRule.Invoke(ruleId, InvokeWallJumpRuleEvent(ruleId));
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
                AddRule.Invoke(ruleId, InvokeDashRuleEvent(ruleId));
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

                AddRule.Invoke(ruleId, InvokeStickyRuleEvent(ruleId));
            }
        }
    }


    private string InvokeDoubleJumpRuleEvent(int ruleId)
    {
        string ruleString = string.Empty;

        if ((CADE_RandomRulesList.DoubleJumpRules)ruleId == CADE_RandomRulesList.DoubleJumpRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs.");
            ruleString = "\n- Power Ups are actually Power Downs. Double Jump? How about NO JUMPING ALLOWED.\n";
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.DoubleJumpRules)ruleId == CADE_RandomRulesList.DoubleJumpRules.LowGravity)
        {
            Debug.Log("Rule Applied: You are on the moon.");
            ruleCounter++;
            ruleString = "\n- You are on the moon.\n";
            LowGravity.Invoke();
        }

        return ruleString;
    }

    private string InvokeWallJumpRuleEvent(int ruleId)
    {
        string ruleString = string.Empty;

        if ((CADE_RandomRulesList.WallJumpRules)ruleId == CADE_RandomRulesList.WallJumpRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs.");
            ruleCounter++;
            ruleString = "\n- Power Ups are actually Power Downs: Wall jump? How bout WALLS KILL.\n";
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.WallJumpRules)ruleId == CADE_RandomRulesList.WallJumpRules.TinyPlayer)
        {
            Debug.Log("Rule Applied: You must be pea-sized to play.");
            ruleCounter++;
            ruleString = "\n- New size requirement: you must be pea-sized.\n";
            TinyPlayer.Invoke();
        }

        return ruleString;
    }

    private string InvokeDashRuleEvent(int ruleId)
    {
        string ruleString = string.Empty;

        if ((CADE_RandomRulesList.DashRules)ruleId == CADE_RandomRulesList.DashRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs");
            ruleCounter++;
            ruleString = "\n- Power Ups are actually Power Downs: You dash straight downward.\n";
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.DashRules)ruleId == CADE_RandomRulesList.DashRules.LeftRightInverted)
        {
            Debug.Log("Rule Applied: Left is actually right, and vice versa.");
            ruleCounter++;
            ruleString = "\n- Left is actually right, and vice versa.";
            LeftRightInverted.Invoke();
        }

        return ruleString;
    }

    private string InvokeStickyRuleEvent(int ruleId)
    {
        string ruleString = string.Empty;

        if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs");
            ruleCounter++;
            ruleString = "\n- Power Ups are actually Power Downs: You're too sticky! Walls make you stuck while floors reduce jump height.\n";
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.UpDownInverted)
        {
            Debug.Log("Rule Applied: Up is actually down, and vice versa.");
            ruleCounter++;
            ruleString = "\n- Up is actually down, and vice versa.\n";
            UpDownInverted.Invoke();
        }
        else if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.LeftRightInverted)
        {
            Debug.Log("Rule Applied: Left is actually right, and vice versa.");
            ruleCounter++;
            ruleString = "\n- Left is actually right, and vice versa.\n";
            LeftRightInverted.Invoke();
        }
        else if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.LeftDeathTimer)
        {
            Debug.Log("Rule Applied: You must not move to the left for more than three seconds or you'll explode.");
            ruleCounter++;
            ruleString = "\n- Don't move to the left for more than 3 seconds, or you die.\n";
            LeftDeathTimer.Invoke();
        }

        return ruleString;
    }
}
