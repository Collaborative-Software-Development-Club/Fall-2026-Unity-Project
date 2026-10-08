using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
    public struct PathStep
    {
        public float Timestamp; // seconds since the level started (idle gaps are capped)
        public Vector2Int Tile;
    }

    // Remembers every tile the player stood on, and when, so it can be played back.
    public class MoveRecorder
    {
        // Long pauses (thinking time) are shortened to this so replays don't drag.
        private const float MaxGap = 1f;

        private readonly List<PathStep> steps = new List<PathStep>();
        private float clock;
        private float lastRealTime;

        public IReadOnlyList<PathStep> Steps => steps;
        public float Duration => steps.Count > 0 ? steps[steps.Count - 1].Timestamp : 0f;

        // Start new recording
        public void Begin(Vector2Int start)
        {
            steps.Clear();
            clock = 0f;
            lastRealTime = Time.time;
            steps.Add(new PathStep { Timestamp = 0f, Tile = start });
        }

        // Call after every successful move hopefully better way of doing this
        public void Record(Vector2Int tile)
        {
            float now = Time.time;
            clock += Mathf.Min(now - lastRealTime, MaxGap);
            lastRealTime = now;
            steps.Add(new PathStep { Timestamp = clock, Tile = tile });
        }
    }
}