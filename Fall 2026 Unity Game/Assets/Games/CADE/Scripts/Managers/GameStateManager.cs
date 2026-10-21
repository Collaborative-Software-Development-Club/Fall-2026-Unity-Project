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
        Unity.Mathematics.Random rand = new Unity.Mathematics.Random();

        // here is where random rules are applied
        if (currentScene == CADE_SceneList.Scenes.CADE_DoubleJump)
        {

            for (int i = 0; i < Enum.GetNames(typeof(CADE_RandomRules.DoubleJumpRules)).Length; i++)
            {
                // 50-50 shot whether the rule is applied
                if (rand.NextFloat() < 0.5)
                {
                    // Call the rule manager to call the according function
                }
            }
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_WallJump)
        {
            for (int i = 0; i < Enum.GetNames(typeof(CADE_RandomRules.WallJumpRules)).Length; i++)
            {
                // 50-50 shot whether the rule is applied
                if (rand.NextFloat() < 0.5)
                {
                    // Call the rule manager to call the according function
                }
            }
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_Dash)
        {
            for (int i = 0; i < Enum.GetNames(typeof(CADE_RandomRules.DashRules)).Length; i++)
            {
                // 50-50 shot whether the rule is applied
                if (rand.NextFloat() < 0.5)
                {
                    // Call the rule manager to call the according function
                }
            }
        }
        else if (currentScene == CADE_SceneList.Scenes.CADE_Sticky)
        {
            for (int i = 0; i < Enum.GetNames(typeof(CADE_RandomRules.StickyRules)).Length; i++)
            {
                // 50-50 shot whether the rule is applied
                if (rand.NextFloat() < 0.5)
                {
                    // Call the rule manager to call the according function
                }
            }
        }
    }
}
