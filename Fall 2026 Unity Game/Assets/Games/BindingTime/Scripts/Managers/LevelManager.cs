using System;
using UnityEngine;

namespace BindingTime
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }

        [Header("Levels in play order (drag the .txt files here)")]
        [SerializeField] private TextAsset[] levels;

        [SerializeField] private bool fitCamera = true;

        [Header("Fallback level, only used if a level slot is empty")]
        [SerializeField] private int fallbackWidth = 50;
        [SerializeField] private int fallbackHeight = 30;

        public int CurrentLevel { get; private set; }
        public bool IsLoaded { get; private set; }
        public int LevelCount => levels != null ? levels.Length : 0;
        public LevelInfo CurrentInfo { get; private set; } = new LevelInfo();
        public bool HasNextLevel => CurrentLevel + 1 < LevelCount;

        public event Action<int> LevelLoaded;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            LoadLevel(0);
        }

        public void NextLevel()
        {
            if (!HasNextLevel)
            {
                Debug.LogWarning("NextLevel called but there are no more levels.");
                return;
            }
            LoadLevel(CurrentLevel + 1);
        }

        public void LoadLevel(int index)
        {
            CurrentLevel = index;

            TextAsset file = index >= 0 && index < LevelCount ? levels[index] : null;
            var info = new LevelInfo();
            LevelCell[,] map = file != null
                ? LevelParser.Parse(file.text, info)
                : LevelParser.Fallback(fallbackWidth, fallbackHeight);
            CurrentInfo = info;

            TileManager.Instance.Build(map);
            if (fitCamera) FitCamera();

            IsLoaded = true;
            LevelLoaded?.Invoke(index);
        }

        private void FitCamera()
        {
            Camera cam = Camera.main;
            if (cam == null || !cam.orthographic) return;

            TileManager tiles = TileManager.Instance;
            Vector3 center = tiles.transform.TransformPoint(
                new Vector3((tiles.Width - 1) / 2f, (tiles.Height - 1) / 2f, 0f));
            cam.transform.position = new Vector3(center.x, center.y, cam.transform.position.z);

            float halfHeight = tiles.Height / 2f + 0.5f;
            float halfWidth = (tiles.Width / 2f + 0.5f) / cam.aspect;
            cam.orthographicSize = Mathf.Max(halfHeight, halfWidth);
        }
    }
}
