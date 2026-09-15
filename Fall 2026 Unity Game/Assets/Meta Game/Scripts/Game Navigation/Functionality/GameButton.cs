using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameButton : MonoBehaviour
{
    [SerializeField] private Image thumbnailImage;
    [SerializeField] private TMP_Text titleText;
    private string sceneToLoad;

    public void Setup(GameEntry entry)
    {
        thumbnailImage.sprite = entry.thumbnail;
        titleText.text = entry.displayName;
        sceneToLoad = entry.sceneName;
    }

    public void OnClick()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
