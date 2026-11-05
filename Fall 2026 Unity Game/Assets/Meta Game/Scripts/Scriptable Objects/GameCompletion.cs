public static class GameCompletion
{
    public static int GamesBeat { get; private set; } = 0;


    public static void BeatGame()
    {
        GamesBeat++;
    }

    public static void SetBeaten(int newVal)
    {
        GamesBeat = newVal;
    }
}
