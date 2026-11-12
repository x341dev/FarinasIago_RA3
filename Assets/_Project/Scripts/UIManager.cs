using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameCanvasPrefab;


    private Canvas currentUICanvas;
    private GameObject pausePanel;

    private bool IsGamePaused = false;

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
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameEvents.OnPauseStateChanged -= SetPauseMenuVisibility;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
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
}
