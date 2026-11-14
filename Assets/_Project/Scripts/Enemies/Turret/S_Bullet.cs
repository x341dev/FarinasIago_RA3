using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    private BulletPool bulletPool;

    public void SetPool(BulletPool pool) => bulletPool = pool;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameEvents.PlayerDied();
            bulletPool.ReturnBullet(gameObject);
        }
        else if (collision.CompareTag("BulletDestroyer"))
        {
            bulletPool.ReturnBullet(gameObject);
        }
    }
}
