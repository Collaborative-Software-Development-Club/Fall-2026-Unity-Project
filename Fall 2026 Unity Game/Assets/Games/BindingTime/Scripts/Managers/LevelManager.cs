using System;
using UnityEngine;

namespace BindingTime
{
    // Owns the list of levels, which one is active, and moving between them.
    // It reads the level text files and hands the result to TileManager to build.
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
        public bool HasNextLevel => levels != null && CurrentLevel + 1 < levels.Length;

        // passes the level index.
        public event Action<int> LevelLoaded;

        void Awake()
        {
            Instance = this;
        }

        // TileManager.Instance is guaranteed to exist hopefully
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

            TextAsset file = (levels != null && index >= 0 && index < levels.Length) ? levels[index] : null;
            TileType[,] map = file != null ? ParseLevel(file.text) : FallbackLevel();

            TileManager.Instance.Build(map);
            if (fitCamera) FitCamera();

            IsLoaded = true;
            LevelLoaded?.Invoke(index);
        }

        private TileType[,] FallbackLevel()
        {
            var map = new TileType[fallbackHeight, fallbackWidth];
            for (int y = 0; y < fallbackHeight; y++)
            {
                for (int x = 0; x < fallbackWidth; x++)
                {
                    bool border = x == 0 || y == 0 || x == fallbackWidth - 1 || y == fallbackHeight - 1;
                    map[y, x] = border ? TileType.Wall : TileType.Ground;
                }
            }
            map[1, 1] = TileType.Entrance;
            map[fallbackHeight - 2, fallbackWidth - 2] = TileType.Exit;
            return map;
        }

        // key:  #  wall    .  floor    S  entrance (spawn    E  exit
        // levels r displayed bottom to top
        private TileType[,] ParseLevel(string text)
        {
            // ignore blank lines at the end of the file
            string[] lines = text.Replace("\r", "").TrimEnd('\n').Split('\n');
            int h = lines.Length;
            int w = 0;
            foreach (var line in lines) w = Mathf.Max(w, line.Length);

            var map = new TileType[h, w];

            for (int row = 0; row < h; row++)
            {
                int y = h - 1 - row; // flip so the first line is the top
                for (int x = 0; x < w; x++)
                {
                    char c = x < lines[row].Length ? lines[row][x] : '#'; // pad short lines with wall
                    map[y, x] = c switch
                    {
                        '#' => TileType.Wall,
                        'S' => TileType.Entrance,
                        'E' => TileType.Exit,
                        _ => TileType.Ground,
                    };
                }
            }
            return map;
        }

        // Centers camera && zoom
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