using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitParticles;

    private BulletPool bulletPool;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Collider2D col;

    private Coroutine hitRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    public void Init(BulletPool pool, Vector2 velocity)
    {
        bulletPool = pool;
        rb.linearVelocity = velocity;
        sr.enabled = true;
        col.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameEvents.PlayerDied();
            Hit();
        }
        else if (collision.CompareTag("BulletDestroyer"))
        {
            Hit();
        }
        else if (collision.CompareTag("BulletDestroyer") || collision.CompareTag("Ground"))
        {
            Hit();
        }
    }

    private void Hit()
    {
        if (hitRoutine != null)
            StopCoroutine(hitRoutine);
        hitRoutine = StartCoroutine(HitRoutine());
    }

    private IEnumerator HitRoutine()
    {
        hitParticles.Play();

        sr.enabled = false;
        col.enabled = false;

        rb.linearVelocity = Vector2.zero;

        float duration = hitParticles.main.duration;
        yield return new WaitForSeconds(duration);
        bulletPool.ReturnBullet(gameObject);
    }
}
