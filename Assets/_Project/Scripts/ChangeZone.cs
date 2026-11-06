using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ChangeZone : MonoBehaviour
{
    [SerializeField] private int beforeZoneIndex;
    [SerializeField] private int afterZoneIndex;

    [SerializeField] private bool isFlipped = false;

	private float enterSide;
	BoxCollider2D box;

	private void Reset()
	{
		box = GetComponent<BoxCollider2D>();
		box.isTrigger = true;
		box.size = new Vector2(0.01f, 4f);
	}

    private void Start()
    {
        box = GetComponent<BoxCollider2D>();
	}

    private int GetSize(Collider2D other)
    {
        Vector3 local = transform.InverseTransformPoint(other.bounds.center);

        float value = box.size.x <= box.size.y 
            ? local.x - box.offset.x 
            : local.y - box.offset.y;

        return value >= 0 ? 1 : -1;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (!collision.CompareTag("Player")) return;
		
		enterSide = GetSize(collision);
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        float exitSide = GetSize(collision);

		if (enterSide == exitSide) return;

        bool toPositive = exitSide > 0;
        if (isFlipped) toPositive = !toPositive;

		if (toPositive)
        {
            GameEvents.ZoneChanged(afterZoneIndex);
        }
        else
        {
            GameEvents.ZoneChanged(beforeZoneIndex);
        }
    }
}
