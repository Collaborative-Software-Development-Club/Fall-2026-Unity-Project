using UnityEngine;
using TMPro;
using System.Collections.Generic;

// TODO: Put this into a UI Manager eventually
public class TextController : MonoBehaviour
{
    public static TextController Instance { get; private set; }
    
    public TextMeshPro worldText;

    private class priorityElement<T>
    {
        private T element;
        private int priority;

        public priorityElement(T element, int priority)
        {
            this.element = element;
            this.priority = priority;
        }

        public T Element { get { return element; } }

        public int Priority { get { return priority; } }
    }

    private List<priorityElement<string>> priorityTextQueue = new List<priorityElement<string>>();

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

    /***
     * Use this to queue new text to be displayed on the UI element.
     * The text with the highest priority will be displayed.
     * If multiple texts have the same priority, the most recently added text will be displayed.
     * 0 = recipe messages
     * 1 = hover messages
     * 2 = machine interaction messages
     * 3 = tutorial messages.
     */
    public void QueueNewText(string newText, int priority = 0)
    {
               priorityTextQueue.Add(new priorityElement<string>(newText, priority));
        priorityTextQueue.Sort((a, b) => b.Priority.CompareTo(a.Priority)); // Sort by priority descending
        SetText(priorityTextQueue[0].Element); // Set the text to the highest priority element
    }
    /***
     * Use this to remove text from the queue based on its priority.
     * If the removed text was the highest priority, the next highest priority text will be displayed.
     */
    public void removePriorityFromQueue(int priority)
    {
        priorityTextQueue.RemoveAll(item => item.Priority == priority);
        if (priorityTextQueue.Count > 0)
        {
            SetText(priorityTextQueue[0].Element); // Set the text to the highest priority element
        }
        else
        {
            ClearText(); // Clear the text if the queue is empty
        }
    }

    /***
     * Use this to remove text from the queue.
     */
    public void removeFromQueue(string textToRemove)
    {
        priorityTextQueue.RemoveAll(item => item.Element == textToRemove);
        if (priorityTextQueue.Count > 0)
        {
            SetText(priorityTextQueue[0].Element); // Set the text to the highest priority element
        }
        else
        {
            ClearText(); // Clear the text if the queue is empty
        }
    }

    /***
     * Use this to set the text of the UI element to a specific string.
     */
    public void SetText(string newText) { 
        worldText.text = newText;
    }
    /***
     * Use this to clear the text of the UI element.
     */
    public void ClearText() {
        worldText.text = "";
    }
    /***
     * Use this to build a string by appending new text to the existing text of the UI element.
     */
    public void AppendText(string newText) {
        worldText.text += newText;
    }

    /***
     * Use to get the current text of the UI element.
     */
    public string GetText()
    {
        return worldText.text;
    }
}
