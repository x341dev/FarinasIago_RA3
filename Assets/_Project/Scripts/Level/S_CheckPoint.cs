using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CheckPoint : MonoBehaviour
{
    [SerializeField] private bool isFlipped = false;

    private void Reset()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
		collider.isTrigger = true;
        collider.size = new Vector2(0.01f, 4f);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
		{
			GameEvents.CheckPointReached(transform.position, isFlipped);
		}
	}
}
