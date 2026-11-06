using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    // ==== References ====
    [SerializeField] private GameObject playerPrefab;
    private GameObject player;

    // Constants
    private const float deathDelay = 2f;

	// ==== Data ====
	// Respawn
	private Vector3 respawnPosition;
    private bool respawnFlipped;

	//Coroutine
    private Coroutine respawnRoutine;

	// Subscribe from events
	private void OnEnable()
    {
        GameEvents.OnCheckPointReached += SetRespawnPosition;
        GameEvents.OnPlayerDied += PlayerDie;
	}

	// Unsubscribe from events
	private void OnDisable()
    {
        GameEvents.OnCheckPointReached -= SetRespawnPosition;
		GameEvents.OnPlayerDied -= PlayerDie;
	}

    
	void Awake()
    {
		// Singleton pattern
		if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
		// Get the player from the scene if not assigned in the inspector
		GameObject scenePlayer = GameObject.FindGameObjectWithTag("Player");
        if (scenePlayer != null)
		{
			player = scenePlayer;
		}
		else
		{
            GameObject spawnPosition = GameObject.FindGameObjectWithTag("SpawnPosition");
			player = Instantiate(playerPrefab, spawnPosition.transform.position, Quaternion.identity);
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
        player.SetActive(false);

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

        player.SetActive(true);
		player.transform.position = respawnPosition;

		GameEvents.GravityFlippedStateChanged(respawnFlipped);
	}
}
