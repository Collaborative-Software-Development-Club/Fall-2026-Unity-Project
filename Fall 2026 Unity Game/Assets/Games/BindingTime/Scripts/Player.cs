using System;
using UnityEngine;

namespace BindingTime
{
    public class Player : MonoBehaviour
    {
        private int tileX, tileY;

        // Hook your win screen / next level up to this gamemanger needs ts
        public event Action ReachedExit;

        void Awake()
        {
            UIManager.player = this;
        }

        void Start()
        {
            // Spawn on the entrance tile.
            TileManager.Instance.LevelLoaded += Respawn;
            Respawn(TileManager.Instance.CurrentLevel);
        }

        void Update()
        {
            ApplyPosition();
        }
        void OnDestroy()
        {
            if (TileManager.Instance != null) TileManager.Instance.LevelLoaded -= Respawn;
        }
        private void Respawn(int level)
        {
            tileX = TileManager.Instance.Entrance.x;
            tileY = TileManager.Instance.Entrance.y;
            ApplyPosition();
        }

        private void ApplyPosition()
        {
            transform.localPosition = new Vector3(tileX, tileY, transform.localPosition.z);
        }

        public void Move(int x, int y)
        {
            int newX = tileX + x;
            int newY = tileY + y;

            // illegal movement.
            if (!TileManager.Instance.IsWalkable(newX, newY)) return;

            tileX = newX;
            tileY = newY;

            if (TileManager.Instance.GetTile(tileX, tileY).Type == TileType.Exit)
            {
                Debug.Log("Reached the exit! woo");
                ReachedExit?.Invoke();
            }
        }
    }
}