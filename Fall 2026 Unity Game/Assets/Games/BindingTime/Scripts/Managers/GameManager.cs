using UnityEngine;

namespace BindingTime
{
    public class GameManager : MonoBehaviour
    {
        void Start()
        {
            UIManager.player.ReachedExit += OnReachedExit;
        }

        void OnDestroy()
        {
            if (UIManager.player != null) UIManager.player.ReachedExit -= OnReachedExit;
        }

        void OnReachedExit()
        {

            if (LevelManager.Instance.HasNextLevel)
            {
                LevelManager.Instance.NextLevel();
            }
            else
            {
                Debug.Log("All levels complete!");
            }
        }
        void Update()
        {
            UIManager.Update();
        }
    }
}