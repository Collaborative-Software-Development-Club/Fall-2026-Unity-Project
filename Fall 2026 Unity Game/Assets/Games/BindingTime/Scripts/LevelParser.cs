using System.Collections.Generic;
using UnityEngine;

namespace BindingTime
{
    // Map:   #  wall   .  floor   O  origin   E  exit   W  water   M  moss
    //        S / F  sigil / statue on channel 0
    //        1-9 sigil,  a-i statue,  j-r inverted statue (channels 1-9)
    // Info:  @name Title   @loops N   @tip Text   @step trigger | Text
    // The first map line is the top row.
    public static class LevelParser
    {
        public static LevelCell[,] Parse(string text, LevelInfo info)
        {
            var lines = new List<string>();
            foreach (string raw in text.Replace("\r", "").TrimEnd('\n').Split('\n'))
            {
                if (raw.StartsWith("@")) ParseInfoLine(raw, info);
                else lines.Add(raw);
            }

            int h = lines.Count;
            int w = 0;
            foreach (string line in lines) w = Mathf.Max(w, line.Length);

            var map = new LevelCell[h, w];
            for (int row = 0; row < h; row++)
            {
                int y = h - 1 - row;
                for (int x = 0; x < w; x++)
                {
                    char c = x < lines[row].Length ? lines[row][x] : '#';
                    map[y, x] = ParseCell(c);
                }
            }
            return map;
        }

        public static LevelCell[,] Fallback(int width, int height)
        {
            var map = new LevelCell[height, width];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bool border = x == 0 || y == 0 || x == width - 1 || y == height - 1;
                    map[y, x] = new LevelCell(border ? TileType.Wall : TileType.Ground);
                }
            }
            map[1, 1] = new LevelCell(TileType.Origin);
            map[height - 2, width - 2] = new LevelCell(TileType.Exit);
            return map;
        }

        private static void ParseInfoLine(string line, LevelInfo info)
        {
            int space = line.IndexOf(' ');
            string key = (space < 0 ? line : line.Substring(0, space)).ToLowerInvariant();
            string rest = space < 0 ? "" : line.Substring(space + 1).Trim();

            switch (key)
            {
                case "@name":
                    info.Name = rest;
                    break;
                case "@loops":
                    if (int.TryParse(rest, out int loops)) info.MaxLoops = loops;
                    break;
                case "@tip":
                    info.Steps.Add(TutorialStep.Parse("never", rest));
                    break;
                case "@step":
                    int bar = rest.IndexOf('|');
                    if (bar >= 0) info.Steps.Add(TutorialStep.Parse(rest.Substring(0, bar), rest.Substring(bar + 1)));
                    break;
            }
        }

        private static LevelCell ParseCell(char c)
        {
            if (c >= '1' && c <= '9') return new LevelCell(TileType.Sigil, c - '0');
            if (c >= 'a' && c <= 'i') return new LevelCell(TileType.Figure, c - 'a' + 1);
            if (c >= 'j' && c <= 'r') return new LevelCell(TileType.Figure, c - 'j' + 1, true);

            return c switch
            {
                '#' => new LevelCell(TileType.Wall),
                'O' => new LevelCell(TileType.Origin),
                'E' => new LevelCell(TileType.Exit),
                'S' => new LevelCell(TileType.Sigil),
                'F' => new LevelCell(TileType.Figure),
                'W' => new LevelCell(TileType.Water),
                'M' => new LevelCell(TileType.Moss),
                _ => new LevelCell(TileType.Ground),
            };
        }
    }
}
