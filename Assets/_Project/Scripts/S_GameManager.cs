using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // ==== References ====
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;

    // Constants
    private const float deathDelay = 1.5f;

	// ==== Data ====
	// Respawn
	private Vector3 respawnPosition;
    private bool respawnFlipped;
    public bool IsGamePaused { get; private set; } = false;

	//Coroutine
    private Coroutine respawnRoutine;

	// Subscribe from events
	private void OnEnable()
    {
        SceneManager.sceneLoaded += StartLevel;

        GameEvents.OnCheckPointReached += SetRespawnPosition;
        GameEvents.OnPlayerDied += PlayerDie;
        GameEvents.OnPauseToggleRequested += TogglePause;
        GameEvents.OnLevelCompleted += LevelCompleted;
        GameEvents.OnNextLevelRequested += HandleNextLevelRequest;
        GameEvents.OnExitToMainMenuRequested += HandleExitToMainMenuRequest;
    }

	// Unsubscribe from events
	private void OnDisable()
    {
        SceneManager.sceneLoaded -= StartLevel;

        GameEvents.OnCheckPointReached -= SetRespawnPosition;
		GameEvents.OnPlayerDied -= PlayerDie;
        GameEvents.OnPauseToggleRequested -= TogglePause;
        GameEvents.OnLevelCompleted -= LevelCompleted;
        GameEvents.OnNextLevelRequested -= HandleNextLevelRequest;
        GameEvents.OnExitToMainMenuRequested -= HandleExitToMainMenuRequest;
    }

    
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

    private void Start()
    {
        StartLevel(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void StartLevel(Scene scene, LoadSceneMode mode)
    {
        int currentSceneIndex = scene.buildIndex;
        int lastIndex = SceneManager.sceneCountInBuildSettings - 1;

        if (currentSceneIndex == 0 || currentSceneIndex == lastIndex) return;
        IsGamePaused = false;
        Time.timeScale = 1;
        GameEvents.NotifyPauseStateChanged(IsGamePaused);
        GameObject scenePlayer = GameObject.FindGameObjectWithTag("Player");
        if (scenePlayer != null)
        {
            player = scenePlayer;
        }
        else
        {
            GameObject spawnPosition = GameObject.FindGameObjectWithTag("SpawnPosition");
            player = Instantiate(playerPrefab, spawnPosition.transform.position, Quaternion.identity);
            player.GetComponent<SpriteRenderer>().enabled = false;
			GameEvents.LevelStarted();
		}
    }

    public void FlipGravity()
    {
        GameEvents.GravityFlipped();
    }

    public void SetRespawnPosition(Vector3 position, bool isFlipped)
    {
        respawnPosition = position;
        respawnFlipped = isFlipped;
    }

    public void PlayerDie()
    {
        if (player == null) return;

        if (respawnRoutine != null) StopCoroutine(respawnRoutine);
        respawnRoutine = StartCoroutine(RespawnAfterDelay());
	}

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(deathDelay);
        RespawnPlayer();
        respawnRoutine = null;
	}

    public void RespawnPlayer()
    { 
        if (player == null) return;

		player.transform.position = respawnPosition;

        GameEvents.PlayerRespawned();
		GameEvents.GravityFlippedStateChanged(respawnFlipped);
	}

    private void TogglePause()
    {
        IsGamePaused = !IsGamePaused;
        Time.timeScale = IsGamePaused ? 0 : 1;

        GameEvents.NotifyPauseStateChanged(IsGamePaused);
    }

    private void LevelCompleted()
    {
        Time.timeScale = 0;
    }

    public void HandleNextLevelRequest()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void HandleExitToMainMenuRequest()
    {
        SceneManager.LoadScene(0);
    }

    public void HandleQuitGameRequest()
    {
        Application.Quit();
    }
}
