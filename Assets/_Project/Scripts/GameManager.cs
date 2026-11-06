using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // ==== References ====
    GameObject player;

    // Constants
    private const float deathDelay = 2f;

	// ==== Data ====
	// Respawn
	private Vector3 respawnPosition;
    private bool respawnFlipped;
	// Player
    private bool playerDied = false;

    // Timers
    private float deathTimer = 0f;


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
        // Get the player from the scene
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (playerDied)
		{
			deathTimer += Time.deltaTime;
			if (deathTimer >= deathDelay)
			{
				RespawnPlayer();
				deathTimer = 0f;
			}
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
        playerDied = true;
	}

    public void RespawnPlayer()
    { 
        if (player == null) return;     

		playerDied = false;
		player.SetActive(true);

		player.transform.position = respawnPosition;

		GameEvents.GravityFlippedStateChanged(respawnFlipped);
	}
}
