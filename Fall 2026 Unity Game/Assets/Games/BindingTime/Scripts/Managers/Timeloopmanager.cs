using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
    public class TimeLoopManager : MonoBehaviour
    {
        private const float NormalTickSeconds = 0.3f;

        private static TimeLoopManager instance;
        public static TimeLoopManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GameObject("TimeLoopManager").AddComponent<TimeLoopManager>();
                }
                return instance;
            }
        }

        private readonly MoveRecorder recorder = new MoveRecorder();
        private readonly List<Vector2Int[]> pastRuns = new List<Vector2Int[]>();
        private readonly List<Ghost> ghosts = new List<Ghost>();
        private readonly HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();
        private readonly SigilManager sigils = new SigilManager();
        private Player player;

        public int CurrentTick => recorder.LastTick;
        public int GhostCount => ghosts.Count;

        void Awake()
        {
            instance = this;
        }

        public void StartLevel(Player p)
        {
            player = p;
            pastRuns.Clear();
            BeginRun();
        }

        public void Advance()
        {
            if (player == null) return;

            recorder.Record(player.Coords);
            int tick = recorder.LastTick;
            foreach (Ghost ghost in ghosts) ghost.SetTick(tick);
            Refresh();
        }

        public void Loop()
        {
            if (player == null || player.InputLocked) return;
            if (recorder.LastTick < 1) return;

            pastRuns.Add(recorder.ToArray());
            BeginRun();
        }

        public void RestartLevel()
        {
            if (player == null || player.InputLocked) return;

            pastRuns.Clear();
            BeginRun();
        }

        public IEnumerator ReplayRoutine(float speed, float maxSeconds)
        {
            int last = recorder.LastTick;
            float tickSeconds = Mathf.Min(
                NormalTickSeconds / Mathf.Max(speed, 0.01f),
                maxSeconds / Mathf.Max(last, 1));

            float startTime = Time.time;
            for (int tick = 0; tick <= last; tick++)
            {
                float due = tick * tickSeconds;
                while (Time.time - startTime < due) yield return null;

                player.SetTile(recorder.PositionAt(tick));
                foreach (Ghost ghost in ghosts) ghost.SetTick(tick);
                Refresh();
            }
        }

        private void BeginRun()
        {
            Vector2Int origin = TileManager.Instance.Origin;
            player.SetTile(origin);
            recorder.Begin(origin);

            ClearGhosts();
            foreach (Vector2Int[] run in pastRuns) ghosts.Add(Ghost.Create(player, run));

            Refresh();
        }

        private void ClearGhosts()
        {
            foreach (Ghost ghost in ghosts)
            {
                if (ghost != null) Destroy(ghost.gameObject);
            }
            ghosts.Clear();
        }

        private void Refresh()
        {
            occupied.Clear();
            occupied.Add(player.Coords);
            foreach (Ghost ghost in ghosts) occupied.Add(ghost.Coords);
            sigils.Refresh(occupied);
        }
    }
}