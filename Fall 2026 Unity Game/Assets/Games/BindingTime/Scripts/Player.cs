using System;
using UnityEngine;

namespace BindingTime
{
    public class Player : MonoBehaviour
    {
        private int tileX, tileY;

        public bool InputLocked { get; set; }
        public int WaitCount { get; private set; }

        public Vector2Int Coords => new Vector2Int(tileX, tileY);
        public event Action ReachedExit;

        void Awake()
        {
            UIManager.player = this;
        }

        void Start()
        {
            LevelManager.Instance.LevelLoaded += Respawn;

            if (LevelManager.Instance.IsLoaded) Respawn(LevelManager.Instance.CurrentLevel);
        }

        void OnDestroy()
        {
            if (LevelManager.Instance != null) LevelManager.Instance.LevelLoaded -= Respawn;
        }

        void Update()
        {
            ApplyPosition();
        }

        private void Respawn(int level)
        {
            InputLocked = false;
            TimeLoopManager.Instance.StartLevel(this);
        }

        private void ApplyPosition()
        {
            transform.localPosition = new Vector3(tileX, tileY, transform.localPosition.z);
        }

        public void SetTile(Vector2Int tile)
        {
            tileX = tile.x;
            tileY = tile.y;
            ApplyPosition();
        }

        public void Move(int x, int y)
        {
            if (InputLocked) return;

            int newX = tileX + x;
            int newY = tileY + y;

            if (!TileManager.Instance.IsWalkable(newX, newY)) return;

            tileX = newX;
            tileY = newY;
            TimeLoopManager.Instance.Advance();
            if (InputLocked) return; // a time paradox just happened

            if (TileManager.Instance.GetTile(tileX, tileY).Type == TileType.Exit)
            {
                ReachedExit?.Invoke();
            }
        }

        public void Wait()
        {
            if (InputLocked) return;
            WaitCount++;
            TimeLoopManager.Instance.Advance();
        }
    }
}