using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public static TextController Instance { get; private set; }
    
    public TextMeshPro worldText;

    private void Awake()
    {
        // Set up the singleton instance.
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /** Use this to set the text of the UI element to a specific string.
    */
    public void SetText(string newText) { 
        worldText.text = newText;
    }
    /**
     * Use this to clear the text of the UI element.
     */
    public void ClearText() {
        worldText.text = "";
    }
    /** Use this to build a string by appending new text to the existing text of the UI element.
     */
    public void AppendText(string newText) {
        worldText.text += newText;
    }
}
