using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
    public enum BreakReason { TooManyLoops, Paradox }

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

        // check this in OnDestroy, Instance would create a new manager during teardown
        public static bool Exists => instance != null;

        private readonly MoveRecorder recorder = new MoveRecorder();
        private readonly List<Ghost> ghosts = new List<Ghost>();
        private readonly HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();
        private readonly SigilManager sigils = new SigilManager();
        private Player player;
        private int lastLoopTick;

        public int CurrentTick => recorder.LastTick;
        public int GhostCount => ghosts.Count;

        public int MaxLoops { get; private set; } = int.MaxValue;

        public bool GhostOnSigil { get; private set; }

        public event Action<BreakReason> TimeBroke;

        public event Action RunStarted;

        void Awake()
        {
            instance = this;
        }

        public void StartLevel(Player p)
        {
            player = p;
            BeginRun();
        }

        public void Advance()
        {
            if (player == null) return;

            recorder.Record(player.Coords);
            int tick = recorder.LastTick;
            foreach (Ghost ghost in ghosts) ghost.SetTick(tick);
            Refresh();
            CheckParadox();
        }

        // standing in a statue on the tick it closes = paradox
        private void CheckParadox()
        {
            if (!sigils.IsForcedOpen(player.Coords)) return;
            player.InputLocked = true;
            TimeBroke?.Invoke(BreakReason.Paradox);
        }

        // spawns a ghost at the origin that replays the run so far
        public void Loop()
        {
            if (player == null || player.InputLocked) return;
            if (recorder.LastTick < 1 || recorder.LastTick <= lastLoopTick) return;

            if (ghosts.Count >= MaxLoops)
            {
                player.InputLocked = true;
                TimeBroke?.Invoke(BreakReason.TooManyLoops);
                return;
            }

            lastLoopTick = recorder.LastTick;
            ghosts.Add(Ghost.Create(player, recorder.ToArray(), recorder.LastTick));
            Refresh();
            CheckParadox();
        }

        public void RestartLevel()
        {
            if (player == null || player.InputLocked) return;
            BeginRun();
        }

        // like R, but ignores and clears InputLocked
        public void ForceRestart()
        {
            if (player == null) return;
            BeginRun();
            player.InputLocked = false;
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
            lastLoopTick = -1;
            MaxLoops = LevelManager.Instance != null ? LevelManager.Instance.CurrentInfo.MaxLoops : int.MaxValue;

            ClearGhosts();
            Refresh();
            RunStarted?.Invoke();
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
            GhostOnSigil = false;
            foreach (Ghost ghost in ghosts)
            {
                if (!ghost.Active) continue;
                occupied.Add(ghost.Coords);
                if (TileManager.Instance.IsSigil(ghost.Coords)) GhostOnSigil = true;
            }
            sigils.Refresh(occupied);
        }
    }
}
