using System;
using UnityEngine;

namespace BindingTime
{
    public class Player : MonoBehaviour
    {
        private int tileX, tileY;

        public bool InputLocked { get; set; }

        public Vector2Int Coords => new Vector2Int(tileX, tileY);

        // Fired when the player steps onto an exit tile (GameManager listens to this).
        public event Action ReachedExit;

        void Awake()
        {
            UIManager.player = this;
        }

        void Start()
        {
            // Move to the new origin every time a level is (re)loaded.
            LevelManager.Instance.LevelLoaded += Respawn;

            // If the level got loaded before we subscribed, spawn now.
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

            if (TileManager.Instance.GetTile(tileX, tileY).Type == TileType.Exit)
            {
                ReachedExit?.Invoke();
            }
        }

        public void Wait()
        {
            if (InputLocked) return;
            TimeLoopManager.Instance.Advance();
        }
    }
}