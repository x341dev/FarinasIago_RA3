using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [Header("References")]
    [SerializeField] private Transform groundCheck;

	InputSystem_Actions inputActions;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    Vector2 directionalMoveInput;
    bool isGravityFlipped = false;
    bool isGrounded;

    BoxCollider2D groundCheckCollider;
    GroundCheck groundCheckComponent;

	private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);
	}

    void OnEnable() => inputActions.Player.Enable();
    void OnDisable() => inputActions.Player.Disable();



	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		groundCheckComponent = groundCheck.GetComponent<GroundCheck>();
		groundCheckCollider = groundCheck.GetComponent<BoxCollider2D>();
	}

    void Update()
    {
        rb.linearVelocity = new Vector2(directionalMoveInput.x * 5f, rb.linearVelocity.y);

    }

    public void OnMove(InputAction.CallbackContext context)
    {
		directionalMoveInput = context.ReadValue<Vector2>();

	}

	public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed || !groundCheckComponent.IsGrounded) return;

		FlipGravity();
	}

    private void FlipGravity()
    {
        isGravityFlipped = !isGravityFlipped;

        rb.gravityScale = isGravityFlipped ? -1f : 1f;
        spriteRenderer.flipY = isGravityFlipped;

        groundCheckCollider.offset = new Vector2(0, -groundCheckCollider.offset.y);
	}
}
