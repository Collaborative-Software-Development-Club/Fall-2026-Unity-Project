using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEntry", menuName = "MiniGame/Game Entry")]
public class GameEntry : ScriptableObject
{
    [Tooltip("Name shown in the menu")]
    public string displayName;

    [Tooltip("Scene name (must match exactly in Build Settings)")]
    public string sceneName;

    [Tooltip("Thumbnail shown in the menu")]
    public Sprite thumbnail;
}
