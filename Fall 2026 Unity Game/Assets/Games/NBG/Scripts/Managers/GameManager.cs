using UnityEngine;

public class GameManager : MonoBehaviour
{
    public AudioManager AudioManager;
    public RecipeManager RecipeManager;
    public ConveyorManager conveyorManager;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject); 
        
        if (conveyorManager is null)
            conveyorManager = GetComponent<ConveyorManager>();
        if (RecipeManager is null)
            RecipeManager = GetComponent<RecipeManager>();
        if (AudioManager is null)
            AudioManager = GetComponent<AudioManager>();
    }

    
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager not found");
            }
            return _instance;
        }
    }
}