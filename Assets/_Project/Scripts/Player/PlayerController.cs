using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [Header("Settings")]
	[SerializeField] private float moveSpeed = 5f;

	[Header("References")]
    [SerializeField] private Transform groundCheck;

	InputSystem_Actions inputActions;

    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    Vector2 directionalMoveInput;
    public bool isGravityFlipped { get; private set; } = false;
    bool isGrounded;

    BoxCollider2D groundCheckCollider;
    GroundCheck groundCheckComponent;

	private void Awake()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.SetCallbacks(this);
	}

    void OnEnable()
    {
        inputActions.Player.Enable();

        GameEvents.OnGravityFlipped += FlipGravity;
	}
    
    void OnDisable()
    {
        inputActions.Player.Disable();
        GameEvents.OnGravityFlipped -= FlipGravity;
	}



	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		groundCheckComponent = groundCheck.GetComponent<GroundCheck>();
		groundCheckCollider = groundCheck.GetComponent<BoxCollider2D>();
	}

    void Update()
    {
        rb.linearVelocity = new Vector2(directionalMoveInput.x * moveSpeed, rb.linearVelocity.y);

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

    public void FlipGravity()
    {
        isGravityFlipped = !isGravityFlipped;

        if (isGravityFlipped) rb.gravityScale = -Mathf.Abs(rb.gravityScale);
		else rb.gravityScale = Mathf.Abs(rb.gravityScale);

		spriteRenderer.flipY = isGravityFlipped;

        groundCheckCollider.offset = new Vector2(0, -groundCheckCollider.offset.y);
	}
}
