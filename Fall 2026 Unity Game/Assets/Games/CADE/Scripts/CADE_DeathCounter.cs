using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class CADE_DeathCounter : MonoBehaviour
{
    private TextMeshProUGUI deathText;
    private static int deathCount = 0;
    private static string trackedScene = null;
    public static event Action OnDeathRegistered;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetDeathCount()
    {
        deathCount = 0;
        trackedScene = null;
        OnDeathRegistered = null;
    }

    private void Awake()
    {
        deathText = GetComponent<TextMeshProUGUI>();
        string currentScene = SceneManager.GetActiveScene().name;

        if (trackedScene != currentScene)
        {
            deathCount = 0;
            trackedScene = currentScene;
        }

        UpdateUI();
    }

    private void OnEnable()
    {
        OnDeathRegistered += IncrementDeathCount;
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateUI();
    }

    private void OnDisable()
    {
        OnDeathRegistered -= IncrementDeathCount;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void RegisterDeath()
    {
        OnDeathRegistered?.Invoke();
    }

    private void IncrementDeathCount()
    {
        deathCount++;
        UpdateUI();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (trackedScene != scene.name)
        {
            deathCount = 0;
            trackedScene = scene.name;
        }

        UpdateUI();
    }


    private void UpdateUI()
    {
        if (deathText == null)
            return;

        deathText.text = $"Deaths: {deathCount}";
    }
}
