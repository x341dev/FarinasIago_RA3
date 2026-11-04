using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class CoinPickup : MonoBehaviour
{
    [SerializeField] private int value = 1;

    private void Reset()
    {
        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameEvents.CoinCollected(value);
			Destroy(gameObject);     
		}
    }
}
