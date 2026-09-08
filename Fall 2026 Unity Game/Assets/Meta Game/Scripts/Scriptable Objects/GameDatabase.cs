using UnityEngine;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "MiniGame/Game Database")]
public class GameDatabase : ScriptableObject
{
    public GameEntry[] games;
}
