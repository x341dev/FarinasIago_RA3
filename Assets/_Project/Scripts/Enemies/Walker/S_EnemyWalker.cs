using System.Collections;
using UnityEngine;

public class EnemyWalker : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float timeToChangeDirection = 2f;


    [Header("Editor Settings")]
    [SerializeField] private bool isGravityFlipped = false;
    [SerializeField] private bool isStartingRight = false;

    [Header("References")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float detectionRadius = 0.15f;


    private Rigidbody2D rb;
    private Animator animator;

    private bool isFacingRight = false;
    private bool isWaiting = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ApplyGravityChanges();
        ApplyStartingRight();
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Velocity", Mathf.Abs(rb.linearVelocity.x));

        if (isWaiting)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }        

        float direction = isFacingRight ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);

        CheckObstacle();
    }

    private void OnValidate()
    {
        ApplyGravityChanges();
        ApplyStartingRight();
    }

    private void ApplyStartingRight()
    {
        isFacingRight = isStartingRight;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (isStartingRight ? -1f : 1f);
        transform.localScale = scale;
    }

    private void ApplyGravityChanges()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();

        Vector3 scale = transform.localScale;
        scale.y = Mathf.Abs(scale.y) * (isGravityFlipped ? -1f : 1f);
        transform.localScale = scale;

        if (rb != null)
        {
            rb.gravityScale = Mathf.Abs(rb.gravityScale) * (isGravityFlipped ? -1f : 1f);
        }
    }

    private bool CheckGround()
    {
        Collider2D groundColision = Physics2D.OverlapCircle(groundCheck.position, detectionRadius, groundLayer);
        return groundColision != null && groundColision.CompareTag("Ground");
    }

    private bool CheckWall()
    {
        Collider2D wallColision = Physics2D.OverlapCircle(wallCheck.position, detectionRadius, groundLayer);
        return wallColision != null && wallColision.CompareTag("Ground");
    }

    private void CheckObstacle()
    {
        bool isThereGround = CheckGround();
        bool isThereWall = CheckWall();

        if (isThereWall || !isThereGround)
        {
            StartCoroutine(ChangeDirectionAfterDelay());
        }
    }



    private IEnumerator ChangeDirectionAfterDelay()
    {
        isWaiting = true;
        yield return new WaitForSeconds(timeToChangeDirection);
        Flip();
        isWaiting = false;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = CheckGround() ? Color.red : Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, detectionRadius);
        }

        if (wallCheck != null)
        {
            Gizmos.color = CheckWall() ? Color.red : Color.green;
            Gizmos.DrawWireSphere(wallCheck.position, detectionRadius);
        }
    }
}
