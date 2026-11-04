using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
    public class Ghost : MonoBehaviour
    {
        private static readonly Vector2Int Nowhere = new Vector2Int(int.MinValue, int.MinValue);

        private IReadOnlyList<Vector2Int> path;
        private SpriteRenderer sr;
        private int spawnTick;
        private float z;

        public Vector2Int Coords { get; private set; } = Nowhere;

        // False before the tick the ghost was created on (only matters while replaying the level).
        public bool Active { get; private set; }

        // Ticks in one lap of the recording. The ghost does the whole recording from the start,
        // then snaps back to the spawn and does it again, forever.
        public int Period => path.Count;

        // path = everything the player did from the origin up to the moment T was pressed.
        // spawnTick = the tick T was pressed on. The ghost starts at the origin on that tick, so it
        // trails the player by exactly spawnTick ticks.
        public static Ghost Create(Player player, IReadOnlyList<Vector2Int> path, int spawnTick)
        {
            var go = new GameObject("Ghost");
            go.transform.SetParent(player.transform.parent, false);
            go.transform.localScale = player.transform.localScale;

            var sr = go.AddComponent<SpriteRenderer>();
            var playerSr = player.GetComponentInChildren<SpriteRenderer>();
            if (playerSr != null)
            {
                sr.sprite = playerSr.sprite;
                sr.sortingLayerID = playerSr.sortingLayerID;
                sr.sortingOrder = playerSr.sortingOrder - 1;
            }
            sr.color = new Color(0.45f, 0.90f, 1.00f, 0.55f);

            var ghost = go.AddComponent<Ghost>();
            ghost.sr = sr;
            ghost.path = path;
            ghost.spawnTick = spawnTick;
            ghost.z = player.transform.localPosition.z;
            ghost.SetTick(spawnTick);
            return ghost;
        }

        // tick = the player's current tick (ticks since the level started).
        public void SetTick(int tick)
        {
            Active = tick >= spawnTick;
            if (sr != null) sr.enabled = Active;

            if (!Active)
            {
                Coords = Nowhere;
                return;
            }

            Coords = path[(tick - spawnTick) % path.Count];
            transform.localPosition = new Vector3(Coords.x, Coords.y, z);
        }
    }
}
