public static class GameCompletion
{
    public static int GamesBeat { get; private set; } = 0;


    public static void BeatGame()
    {
        GamesBeat++;
    }
    
    /// <summary>
    /// For debug use only
    /// </summary>
    /// <param name="newVal"></param>
    public static void SetBeaten(int newVal)
    {
        GamesBeat = newVal;
    }
}
