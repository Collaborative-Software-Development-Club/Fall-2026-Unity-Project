using System;
using UnityEngine;

namespace BindingTime
{
    public class Player : MonoBehaviour
    {
        public static Player Instance { get; private set; }

        public Vector2Int Coords { get; private set; }
        public bool InputLocked { get; set; }
        public int WaitCount { get; private set; }

        public event Action ReachedExit;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            LevelManager.Instance.LevelLoaded += Respawn;
            if (LevelManager.Instance.IsLoaded) Respawn(LevelManager.Instance.CurrentLevel);
        }

        void OnDestroy()
        {
            if (LevelManager.Instance != null) LevelManager.Instance.LevelLoaded -= Respawn;
            if (Instance == this) Instance = null;
        }

        private void Respawn(int level)
        {
            InputLocked = false;
            TimeLoopManager.Instance.StartLevel(this);
        }

        public void SetTile(Vector2Int tile)
        {
            Coords = tile;
            transform.localPosition = new Vector3(tile.x, tile.y, transform.localPosition.z);
        }

        public void Move(int dx, int dy)
        {
            if (InputLocked) return;

            Vector2Int next = Coords + new Vector2Int(dx, dy);
            if (!TileManager.Instance.IsWalkable(next)) return;

            SetTile(next);
            TimeLoopManager.Instance.Advance();
            if (InputLocked) return; // paradox

            if (TileManager.Instance.GetTile(Coords).Type == TileType.Exit) ReachedExit?.Invoke();
        }

        public void Wait()
        {
            if (InputLocked) return;
            WaitCount++;
            TimeLoopManager.Instance.Advance();
        }
    }
}
