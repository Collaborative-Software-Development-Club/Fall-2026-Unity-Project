using System.Collections;
using UnityEngine;

namespace BindingTime
{
    public class GameManager : MonoBehaviour
    {
        [Header("Exit replay")]
        [SerializeField] private float replaySpeed = 4f;
        [SerializeField] private float maxReplaySeconds = 4f;     // long runs are sped up more to fit
        [SerializeField] private float pauseBeforeReplay = 0.4f;
        [SerializeField] private float pauseAfterReplay = 0.6f;

        void Start()
        {
            new GameObject("RulesScreen").AddComponent<RulesScreen>();

            UIManager.player.ReachedExit += OnReachedExit;
        }

        void OnDestroy()
        {
            if (UIManager.player != null) UIManager.player.ReachedExit -= OnReachedExit;
        }

        void OnReachedExit()
        {
            StartCoroutine(ExitSequence());
        }

        private IEnumerator ExitSequence()
        {
            Player player = UIManager.player;
            player.InputLocked = true;

            yield return new WaitForSeconds(pauseBeforeReplay);
            yield return TimeLoopManager.Instance.ReplayRoutine(replaySpeed, maxReplaySeconds);
            yield return new WaitForSeconds(pauseAfterReplay);

            if (LevelManager.Instance.HasNextLevel)
            {
                LevelManager.Instance.NextLevel(); // Player respawns and unlocks itself
            }
            else
            {
                Debug.Log("All levels complete!");  // input stays locked on the last level
            }
        }

        void Update()
        {
            UIManager.Update();
        }
    }
}