using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
    public class MoveRecorder
    {
        private readonly List<Vector2Int> positions = new List<Vector2Int>();

        public int Length => positions.Count;
        public int LastTick => positions.Count - 1;

        // Start new recording
        public void Begin(Vector2Int start)
        {
            positions.Clear();
            positions.Add(start);
        }

        public void Record(Vector2Int tile)
        {
            positions.Add(tile);
        }

        public Vector2Int PositionAt(int tick)
        {
            return positions[Mathf.Clamp(tick, 0, positions.Count - 1)];
        }

        public Vector2Int[] ToArray()
        {
            return positions.ToArray();
        }
    }
}
