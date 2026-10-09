using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameNode : MonoBehaviour
{
    [SerializeField] Canvas gameDisplay;
    [SerializeField] Image thumbnailImage;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] float pullStrength = 500f;
    private string sceneToLoad;

    private void LoadScene()
    {
        if (sceneToLoad == null)
            return;

        SceneManager.LoadScene(sceneToLoad);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gameDisplay.gameObject.SetActive(true);
        if(collision.CompareTag("Player"))
        {
            collision.GetComponent<MetaPlayerMovement>().Selected.AddListener(LoadScene);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Rigidbody2D crb = collision.attachedRigidbody;
            crb.AddForce((transform.position - crb.transform.position) * Time.fixedDeltaTime * pullStrength);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        gameDisplay.gameObject.SetActive(false);
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<MetaPlayerMovement>().Selected.RemoveAllListeners();
        }
    }

    public void Setup(GameEntry entry)
    {
        thumbnailImage.sprite = entry.thumbnail;
        titleText.text = entry.displayName;
        sceneToLoad = entry.sceneName;
    }
}
