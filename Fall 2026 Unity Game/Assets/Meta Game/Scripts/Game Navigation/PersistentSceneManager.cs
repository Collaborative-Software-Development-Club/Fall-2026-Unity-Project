using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A persistent singleton that manages scene loading.
/// Pressing the Escape key will return the player to the main menu.
/// </summary>
public class PersistentSceneManager : MonoBehaviour
{
    // The static instance of this class (the singleton)
    public static PersistentSceneManager instance;

    // The name of your main menu scene, configurable in the Unity Inspector
    [Header("Configuration")]
    [Tooltip("The exact name of your main menu scene file.")]
    [SerializeField] private string mainMenuSceneName = "MainGameMenu";

    private void Awake()
    {
        // --- Singleton Pattern Implementation ---
        // If an 'instance' does not yet exist
        if (instance == null)
        {
            // Set this component as the 'instance'
            instance = this;

            // Mark this GameObject to not be destroyed when a new scene is loaded
            DontDestroyOnLoad(gameObject);
        }
        // If an 'instance' already exists and it's not this one
        else if (instance != this)
        {
            // Destroy this GameObject, as a singleton instance already exists.
            // This prevents duplicate managers.
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // Check if the 'Escape' key was pressed down during this frame
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Make sure we are not already in the main menu to avoid uselessly reloading it
            if (SceneManager.GetActiveScene().name != mainMenuSceneName)
            {
                // Load the specified main menu scene
                GoToMainMenu();
            }
        }
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
        Debug.Log($"Loading scene: {mainMenuSceneName}");
    }
}