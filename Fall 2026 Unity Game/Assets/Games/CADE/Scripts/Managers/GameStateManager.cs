using UnityEditor.Media;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameEvent winEvent;
    [SerializeField] private GameEvent loseEvent;
    [SerializeField] private CADE_SceneManager sceneManager;
    [SerializeField] private CADE_SceneList.Scenes nextScene;


    private void OnEnable()
    {
        winEvent.OnEventRaised += winHandle;
        loseEvent.OnEventRaised += loseHandle;
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
}
