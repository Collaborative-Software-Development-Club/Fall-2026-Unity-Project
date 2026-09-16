using UnityEditor.Media;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStateManager : MonoBehaviour
{
    [SerializeField] private GameEvent winEvent;
    [SerializeField] private GameEvent loseEvent;

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
        Debug.Log("You win");
    }
    private void loseHandle() { 
        resetLevel();
    }
    private void resetLevel() { 
        Scene currScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currScene.name);
    }
}
