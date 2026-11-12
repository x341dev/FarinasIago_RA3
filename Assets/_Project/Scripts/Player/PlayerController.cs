using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    [Header("Settings")]
	[SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 3f;

	[Header("References")]
    [SerializeField] private Transform groundCheck;

	// New Input System
	InputSystem_Actions inputActions;

	// Components
	Rigidbody2D rb;
    SpriteRenderer sr;
    Animator animator;

    // Data
    Vector2 directionalMoveInput;
    public bool isGravityFlipped { get; private set; } = false;

    bool isGamePaused = false;

    // References
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
        GameEvents.OnGravityFlippedStateChanged += SetGravityFlipped;
        GameEvents.OnPauseStateChanged += HandlePauseStateChanged;
    }
    
    void OnDisable()
    {
        inputActions.Player.Disable();
        GameEvents.OnGravityFlipped -= FlipGravity;
		GameEvents.OnGravityFlippedStateChanged -= SetGravityFlipped;
        GameEvents.OnPauseStateChanged -= HandlePauseStateChanged;
    }



	void Start()
    {
        rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
		groundCheckComponent = groundCheck.GetComponent<GroundCheck>();
		groundCheckCollider = groundCheck.GetComponent<BoxCollider2D>();
	}

    private void Update()
    {
        if (isGamePaused) return;

        if (isGravityFlipped)
        {
            rb.gravityScale = -Mathf.Abs(rb.gravityScale);
            groundCheckCollider.offset = new Vector2(0, Mathf.Abs(groundCheckCollider.offset.y));
        }
        else
        {
            rb.gravityScale = Mathf.Abs(rb.gravityScale);
			groundCheckCollider.offset = new Vector2(0, -Mathf.Abs(groundCheckCollider.offset.y));
		}

		sr.flipY = isGravityFlipped;
        sr.flipX = directionalMoveInput.x < 0;

        animator.SetBool("Falling", Mathf.Abs(rb.linearVelocity.y) > 0.1f);
        animator.SetFloat("Velocity", Mathf.Abs(rb.linearVelocity.x));

	}

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(directionalMoveInput.x * moveSpeed, rb.linearVelocity.y);

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (isGamePaused) return;
        directionalMoveInput = context.ReadValue<Vector2>();

	}

	public void OnJump(InputAction.CallbackContext context)
    {
        if (isGamePaused) return;
        if (!context.performed || !groundCheckComponent.IsGrounded) return;

		FlipGravity();
	}

    public void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        GameEvents.RequestPauseToggle();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Death"))
		{
			GameEvents.PlayerDied();
		}
	}

    public void SetGravityFlipped(bool flipped) => isGravityFlipped = flipped;
    public void FlipGravity()
    {
        isGravityFlipped = !isGravityFlipped;
        float velocityY = isGravityFlipped ? Mathf.Abs(jumpForce) : -Mathf.Abs(jumpForce);
		rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocityY);
	}

    private void HandlePauseStateChanged(bool isPaused)
    {
        isGamePaused = isPaused;
        if (isGamePaused) directionalMoveInput = Vector2.zero;
    }
}
