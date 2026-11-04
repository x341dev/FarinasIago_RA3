using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshPro scoreText;



    private int score = 0;

    private void OnEnable()
    {
        GameEvents.OnCoinCollected += UpdateScoreUI;
    }

    private void OnDisable()
    {
        GameEvents.OnCoinCollected -= UpdateScoreUI;
	}

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void UpdateScoreUI(int amount)
	{
        score += amount;
		scoreText.text = $"Score: {score}";
	}
}
