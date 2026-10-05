using System;
using UnityEngine;

namespace BindingTime
{
    public class Player : MonoBehaviour
    {
        private int tileX, tileY;

        // Hook your win screen / next level up to this.
        public event Action ReachedExit;

        void Awake()
        {
            UIManager.player = this;
        }

        void Start()
        {
            // Spawn on the entrance tile.
            var tiles = TileManager.Instance;
            tileX = tiles.Entrance.x;
            tileY = tiles.Entrance.y;
            ApplyPosition();
        }

        void Update()
        {
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

            // Walls (and anything off the map) block movement.
            if (!TileManager.Instance.IsWalkable(newX, newY)) return;

            tileX = newX;
            tileY = newY;

            if (TileManager.Instance.GetTile(tileX, tileY).Type == TileType.Exit)
            {
                Debug.Log("Reached the exit!");
                ReachedExit?.Invoke();
            }
        }
    }
}