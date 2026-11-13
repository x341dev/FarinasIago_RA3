using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameCanvasPrefab;


    private Canvas currentUICanvas;
    private GameObject pausePanel;
    private GameObject levelCompletedPanel;


    public static UIManager Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        GameEvents.OnPauseStateChanged += SetPauseMenuVisibility;
        GameEvents.OnLevelCompleted += HandleLevelEnd;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameEvents.OnPauseStateChanged -= SetPauseMenuVisibility;
        GameEvents.OnLevelCompleted -= HandleLevelEnd;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        int currentSceneIndex = scene.buildIndex;
        int lastIndex = SceneManager.sceneCountInBuildSettings - 1;

        if (currentSceneIndex == 0 || currentSceneIndex == lastIndex) return;

        Canvas sceneCanvas = GameObject.FindWithTag("GameCanvas")?.GetComponent<Canvas>();

        if (sceneCanvas != null)
        {
            currentUICanvas = sceneCanvas;
        }
        else if (gameCanvasPrefab != null)
        {
            currentUICanvas = Instantiate(gameCanvasPrefab).GetComponent<Canvas>();
            currentUICanvas.name = gameCanvasPrefab.name;
        } 
        else
        {
            Debug.LogWarning("No GameCanvas found in the scene and no prefab assigned in UIManager.");
            return;
        }

        pausePanel = currentUICanvas.transform.Find("PausePanel")?.gameObject;
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("PausePanel not found in the GameCanvas.");
        }

        levelCompletedPanel = currentUICanvas.transform.Find("LevelCompletedPanel")?.gameObject;
        if (levelCompletedPanel != null)
        {
            levelCompletedPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("LevelCompletedPanel not found in the GameCanvas.");
        }
    }

    private void SetPauseMenuVisibility(bool isPaused)
    {
        if (currentUICanvas == null) return;

        if (pausePanel == null)
        {
            pausePanel = currentUICanvas.transform.Find("PausePanel")?.gameObject;
            if (pausePanel == null) return;
        }

        pausePanel.SetActive(isPaused);
    }

    public void OnResumeButtonClicked()
    {
        GameEvents.RequestPauseToggle();
    }

    private void HandleLevelEnd()
    {
        
        if (currentUICanvas == null) return;

        if (levelCompletedPanel == null)
        {
            levelCompletedPanel = currentUICanvas.transform.Find("LevelCompletedPanel")?.gameObject;
            if (levelCompletedPanel == null) return;
        }

        levelCompletedPanel.SetActive(true);
    }

    public void OnNextLevelButtonClicked()
    {
        GameEvents.RequestNextLevel();
    }

    public void OnExitToMainMenuButtonClicked()
    {
        GameEvents.RequestExitToMainMenu();
    }
}
