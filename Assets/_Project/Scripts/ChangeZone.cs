using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ChangeZone : MonoBehaviour
{
    [SerializeField] private int zoneIndex;

	private void Reset()
	{
		var col = GetComponent<BoxCollider2D>();
		col.isTrigger = true;
		col.size = new Vector2(0.01f, 4f);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
		GameEvents.ZoneChanged(zoneIndex);
	}
}
