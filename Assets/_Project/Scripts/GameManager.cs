using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    void Awake()
    {
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlipGravity()
	{
		GameEvents.GravityFlipped();
	}
}
