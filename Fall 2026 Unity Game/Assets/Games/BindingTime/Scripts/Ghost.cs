using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
    public class Ghost : MonoBehaviour
    {
        private IReadOnlyList<Vector2Int> path;
        private float z;

        public Vector2Int Coords { get; private set; }

        public static Ghost Create(Player player, IReadOnlyList<Vector2Int> path)
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
            ghost.path = path;
            ghost.z = player.transform.localPosition.z;
            ghost.SetTick(0);
            return ghost;
        }

        public void SetTick(int tick)
        {
            Coords = path[Mathf.Clamp(tick, 0, path.Count - 1)];
            transform.localPosition = new Vector3(Coords.x, Coords.y, z);
        }
    }
}