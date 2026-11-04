using System.Collections;
using UnityEngine;

namespace BindingTime
{
    public class GameManager : MonoBehaviour
    {
        [Header("Exit replay")]
        [SerializeField] private float replaySpeed = 4f;
        [SerializeField] private float maxReplaySeconds = 4f;
        [SerializeField] private float pauseBeforeReplay = 0.4f;
        [SerializeField] private float pauseAfterReplay = 0.6f;

        void Start()
        {
            new GameObject("GameHud").AddComponent<GameHud>();
            Player.Instance.ReachedExit += OnReachedExit;
        }

        void OnDestroy()
        {
            if (Player.Instance != null) Player.Instance.ReachedExit -= OnReachedExit;
        }

        void Update()
        {
            InputManager.Update();
        }

        private void OnReachedExit()
        {
            StartCoroutine(ExitSequence());
        }

        private IEnumerator ExitSequence()
        {
            Player.Instance.InputLocked = true;

            yield return new WaitForSeconds(pauseBeforeReplay);
            yield return TimeLoopManager.Instance.ReplayRoutine(replaySpeed, maxReplaySeconds);
            yield return new WaitForSeconds(pauseAfterReplay);

            if (LevelManager.Instance.HasNextLevel) LevelManager.Instance.NextLevel();
            else Debug.Log("All levels complete!");
        }
    }
}
