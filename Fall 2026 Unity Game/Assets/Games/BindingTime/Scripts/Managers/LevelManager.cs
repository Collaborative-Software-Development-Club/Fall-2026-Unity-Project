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
            LevelCell[,] map = file != null ? ParseLevel(file.text) : FallbackLevel();

            TileManager.Instance.Build(map);
            if (fitCamera) FitCamera();

            IsLoaded = true;
            LevelLoaded?.Invoke(index);
        }

        private LevelCell[,] FallbackLevel()
        {
            var map = new LevelCell[fallbackHeight, fallbackWidth];
            for (int y = 0; y < fallbackHeight; y++)
            {
                for (int x = 0; x < fallbackWidth; x++)
                {
                    bool border = x == 0 || y == 0 || x == fallbackWidth - 1 || y == fallbackHeight - 1;
                    map[y, x] = new LevelCell(border ? TileType.Wall : TileType.Ground);
                }
            }
            map[1, 1] = new LevelCell(TileType.Origin);
            map[fallbackHeight - 2, fallbackWidth - 2] = new LevelCell(TileType.Exit);
            return map;
        }

        // key:  #  wall    .  floor    O  origin (spawn)    E  exit
        //       W  water (decorative)     M  moss (decorative)
        //
        // sigils and statues (a sigil powers every statue on the same channel):
        //       S / F          legacy sigil and statue, channel 0
        //       1-9            sigil on channel 1-9
        //       a-i            statue on channel 1-9: closed, opens while its channel is powered
        //       j-r            INVERTED statue on channel 1-9 (j = 1 ... r = 9):
        //                      open by default, closes while its channel is powered
        // levels r displayed bottom to top
        private LevelCell[,] ParseLevel(string text)
        {
            // ignore blank lines at the end of the file
            string[] lines = text.Replace("\r", "").TrimEnd('\n').Split('\n');
            int h = lines.Length;
            int w = 0;
            foreach (var line in lines) w = Mathf.Max(w, line.Length);

            var map = new LevelCell[h, w];

            for (int row = 0; row < h; row++)
            {
                int y = h - 1 - row; // flip so the first line is the top
                for (int x = 0; x < w; x++)
                {
                    char c = x < lines[row].Length ? lines[row][x] : '#'; // pad short lines with wall
                    map[y, x] = ParseCell(c);
                }
            }
            return map;
        }

        private static LevelCell ParseCell(char c)
        {
            if (c >= '1' && c <= '9') return new LevelCell(TileType.Sigil, c - '0');
            if (c >= 'a' && c <= 'i') return new LevelCell(TileType.Figure, c - 'a' + 1);
            if (c >= 'j' && c <= 'r') return new LevelCell(TileType.Figure, c - 'j' + 1, true);

            switch (c)
            {       //needed in every level
                case '#': return new LevelCell(TileType.Wall);
                case 'O': return new LevelCell(TileType.Origin);
                case 'E': return new LevelCell(TileType.Exit);
                    //legacy sigil / statue (channel 0)
                case 'S': return new LevelCell(TileType.Sigil);
                case 'F': return new LevelCell(TileType.Figure);
                    //decorative stuff
                case 'W': return new LevelCell(TileType.Water);
                case 'M': return new LevelCell(TileType.Moss);
                    //G R O U N D
                default: return new LevelCell(TileType.Ground);
            }
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