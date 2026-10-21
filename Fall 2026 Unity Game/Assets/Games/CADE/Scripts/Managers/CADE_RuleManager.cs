using System;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class CADE_RuleManager : MonoBehaviour
{
    public UnityEvent InvertPowerUp;
    public UnityEvent LowGravity;
    public UnityEvent TinyPlayer;
    public UnityEvent QuickTimeEvents;
    public UnityEvent UpDownInverted;
    public UnityEvent LeftRightInverted;
    public UnityEvent LeftDeathTimer;

    [SerializeField]
    private TMP_Text rulesText;

    private string rulesString;

    int ruleCounter = 0;


    private void Start()
    {
        rulesString = " ";
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
            rulesString = "\n- Power Ups are actually Power Downs: Double jump? How bout no jump.\n";
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.DoubleJumpRules)ruleId == CADE_RandomRulesList.DoubleJumpRules.LowGravity)
        {
            Debug.Log("Rule Applied: You are on the moon.");
            ruleCounter++;
            rulesString = "\n- You are on the moon.\n";
            LowGravity.Invoke();
        }

        rulesText.text = rulesText.text + rulesString;
    }

    private void InvokeWallJumpRuleEvent(int ruleId)
    {
        if ((CADE_RandomRulesList.WallJumpRules)ruleId == CADE_RandomRulesList.WallJumpRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs.");
            ruleCounter++;
            rulesString = "\n- Power Ups are actually Power Downs: Wall jump? How bout WALLS KILL.\n";
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.WallJumpRules)ruleId == CADE_RandomRulesList.WallJumpRules.TinyPlayer)
        {
            Debug.Log("Rule Applied: You must be pea-sized to play.");
            ruleCounter++;
            rulesString = "\n- New size requirement: you must be pea-sized.\n";
            TinyPlayer.Invoke();
        }

        rulesText.text = rulesText.text + rulesString;
    }

    private void InvokeDashRuleEvent(int ruleId)
    {
        if ((CADE_RandomRulesList.DashRules)ruleId == CADE_RandomRulesList.DashRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs");
            ruleCounter++;
            rulesString = "\n- Power Ups are actually Power Downs: You dash straight downward.\n";
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.DashRules)ruleId == CADE_RandomRulesList.DashRules.LeftRightInverted)
        {
            Debug.Log("Rule Applied: Left is actually right, and vice versa.");
            ruleCounter++;
            rulesString = "\n- Left is actually right, and vice versa.";
            LeftRightInverted.Invoke();
        }

        // else if ((CADE_RandomRulesList.DashRules)ruleId == CADE_RandomRulesList.DashRules.QuickTimeEvents)
        // {
        //     Debug.Log("Rule Applied: You must complete random quick time events or explode.");
        //     ruleCounter++;
        //     rulesText.SetText(rulesText.text + "\n" + ruleCounter + ". You must complete random quick time events or explode.");
        //     QuickTimeEvents.Invoke();
        // }

        rulesText.text = rulesText.text + rulesString;
    }

    private void InvokeStickyRuleEvent(int ruleId)
    {
        if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.PowerDowns)
        {
            Debug.Log("Rule Applied: Power Ups are actually Power Downs");
            ruleCounter++;
            rulesString = "\n- Power Ups are actually Power Downs: You're too sticky! Walls make you stuck while floors reduce jump height.\n";
            InvertPowerUp.Invoke();
        }
        else if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.UpDownInverted)
        {
            Debug.Log("Rule Applied: Up is actually down, and vice versa.");
            ruleCounter++;
            rulesString = "\n- Up is actually down, and vice versa.\n";
            UpDownInverted.Invoke();
        }
        else if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.LeftRightInverted)
        {
            Debug.Log("Rule Applied: Left is actually right, and vice versa.");
            ruleCounter++;
            rulesText.SetText(rulesText.text + "\n" + ruleCounter + ". Left is actually right, and vice versa.");
            LeftRightInverted.Invoke();
        }
        else if ((CADE_RandomRulesList.StickyRules)ruleId == CADE_RandomRulesList.StickyRules.LeftDeathTimer)
        {
            Debug.Log("Rule Applied: You must not move to the left for more than three seconds or you'll explode.");
            ruleCounter++;
            rulesString = "\n- Don't move to the left for more than 3 seconds, or you die.\n";
            LeftDeathTimer.Invoke();
        }

        rulesText.text = rulesText.text + rulesString;
    }
}
