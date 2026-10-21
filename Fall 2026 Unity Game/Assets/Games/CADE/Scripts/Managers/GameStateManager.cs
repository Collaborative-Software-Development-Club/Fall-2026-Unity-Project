using System;
using System.ComponentModel.Design;
using UnityEditor.Media;
using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
public class GameStateManager : MonoBehaviour
{
    [SerializeField] private CADE_SceneList.Scenes currentScene;
    [SerializeField] private GameEvent winEvent;
    [SerializeField] private GameEvent loseEvent;
    [SerializeField] private CADE_SceneManager sceneManager;
    [SerializeField] private CADE_RuleManager ruleManager;
    [SerializeField] private CADE_SceneList.Scenes nextScene;


    private void OnEnable()
    {
        winEvent.OnEventRaised += winHandle;
        loseEvent.OnEventRaised += loseHandle;
        RandomlySelectRules();
    }

    private void OnDisable()
    {
        winEvent.OnEventRaised -= winHandle;
        loseEvent.OnEventRaised -= loseHandle;
    }

    private void winHandle()
    {
        sceneManager.ChangeScene((int)nextScene);
    }
    private void loseHandle() { 
        resetLevel();
    }
    private void resetLevel() { 
        Scene currScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currScene.name);
    }

    private void RandomlySelectRules()
    {
        // here is where random rules are applied
        if (currentScene == CADE_SceneList.Scenes.CADE_DoubleJump)
        {

            for (int i = 0; i < Enum.GetNames(typeof(CADE_RandomRulesList.DoubleJumpRules)).Length; i++)
            {
                // 50-50 shot whether the rule is applied
                if (UnityEngine.Random.value < 0.5)
                {
                    // Call the rule manager to call the according function
                    ruleManager.ApplyRandomRule(i, currentScene);
                }
            }
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_WallJump)
        {
            for (int i = 0; i < Enum.GetNames(typeof(CADE_RandomRulesList.WallJumpRules)).Length; i++)
            {
                // 50-50 shot whether the rule is applied
                if (UnityEngine.Random.value < 0.5)
                {
                    // Call the rule manager to call the according function
                    ruleManager.ApplyRandomRule(i, currentScene);
                }
            }
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_Dash)
        {
            for (int i = 0; i < Enum.GetNames(typeof(CADE_RandomRulesList.DashRules)).Length; i++)
            {
                // 50-50 shot whether the rule is applied
                if (UnityEngine.Random.value < 0.5)
                {
                    // Call the rule manager to call the according function
                    ruleManager.ApplyRandomRule(i, currentScene);
                }
            }
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_Sticky)
        {
            for (int i = 0; i < Enum.GetNames(typeof(CADE_RandomRulesList.StickyRules)).Length; i++)
            {
                // 50-50 shot whether the rule is applied
                if (UnityEngine.Random.value < 0.5)
                {
                    // Call the rule manager to call the according function
                    ruleManager.ApplyRandomRule(i, currentScene);
                }
            }
        }
    }
}
