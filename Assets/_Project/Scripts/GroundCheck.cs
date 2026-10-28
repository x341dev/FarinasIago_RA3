using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (collision.CompareTag("Ground")) IsGrounded = true;
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.CompareTag("Ground")) IsGrounded = false;
	}
}
