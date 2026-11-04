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

        // Being inside a statue on the tick it closes is a time paradox: the level has to reset.
        private void CheckParadox()
        {
            if (!sigils.IsForcedOpen(player.Coords)) return;
            player.InputLocked = true;
            TimeBroke?.Invoke(BreakReason.Paradox);
        }

        // T: the player stays exactly where they are. A ghost spawns at the origin and repeats
        // everything the player has done so far, so it trails the player by CurrentTick ticks.
        public void Loop()
        {
            if (player == null || player.InputLocked) return;
            if (recorder.LastTick < 1) return;                // nothing to copy yet
            if (recorder.LastTick <= lastLoopTick) return;    // already looped on this tick

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

        // Restart after time broke: same as R, but works while input is locked and unlocks you again.
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
                foreach (Ghost ghost in ghosts) ghost.SetTick(tick);   // ghosts pop in on the tick they were made
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
                Tile under = TileManager.Instance.GetTile(ghost.Coords.x, ghost.Coords.y);
                if (under != null && under.BaseType == TileType.Sigil) GhostOnSigil = true;
            }
            sigils.Refresh(occupied);
        }
    }
}
