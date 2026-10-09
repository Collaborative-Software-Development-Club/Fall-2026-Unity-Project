using System;
using System.Collections;
using UnityEngine;

namespace BindingTime
{
    public class Player : MonoBehaviour
    {
        private int tileX, tileY;
        private readonly MoveRecorder recorder = new MoveRecorder();

        // While true, Move() does nothing (during the exit replay).
        public bool InputLocked { get; set; }

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
            tileX = TileManager.Instance.Origin.x;
            tileY = TileManager.Instance.Origin.y;
            InputLocked = false;
            recorder.Begin(new Vector2Int(tileX, tileY));
            ApplyPosition();
        }

        private void ApplyPosition()
        {
            transform.localPosition = new Vector3(tileX, tileY, transform.localPosition.z);
        }

        public void Move(int x, int y)
        {
            if (InputLocked) return;

            int newX = tileX + x;
            int newY = tileY + y;

            // Walls (and anything off the map) block movement.
            if (!TileManager.Instance.IsWalkable(newX, newY)) return;

            tileX = newX;
            tileY = newY;
            recorder.Record(new Vector2Int(tileX, tileY));

            if (TileManager.Instance.GetTile(tileX, tileY).Type == TileType.Exit)
            {
                ReachedExit?.Invoke();
            }
        }

        // Plays back the path walked this level, sped up.
        // speed: playback multiplier. maxSeconds: replay never takes longer than this.
        public IEnumerator ReplayRoutine(float speed, float maxSeconds)
        {
            var steps = recorder.Steps;
            float effectiveSpeed = Mathf.Max(speed, recorder.Duration / Mathf.Max(maxSeconds, 0.01f));

            float startTime = Time.time;
            for (int i = 0; i < steps.Count; i++)
            {
                float due = steps[i].Timestamp / effectiveSpeed;
                while (Time.time - startTime < due) yield return null;

                tileX = steps[i].Tile.x;
                tileY = steps[i].Tile.y;
                ApplyPosition();
            }
        }
    }
}