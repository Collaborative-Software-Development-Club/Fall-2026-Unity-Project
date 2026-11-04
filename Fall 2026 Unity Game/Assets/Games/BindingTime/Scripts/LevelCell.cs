namespace BindingTime
{
    // Figure = statue (S is taken by Sigil)
    public enum TileType { Ground, Wall, Origin, Moss, Figure, Water, Exit, Sigil }

    public struct LevelCell
    {
        public TileType Type;
        public int Channel;
        public bool Inverted;

        public LevelCell(TileType type, int channel = 0, bool inverted = false)
        {
            Type = type;
            Channel = channel;
            Inverted = inverted;
        }
    }
}
