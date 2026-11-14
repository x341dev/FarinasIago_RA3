using System.Collections;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fireRate = 1f;

    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private BulletPool pool;

    private Coroutine fireRoutine;

    void Start()
    {
        if (fireRoutine == null)
        {
            fireRoutine = StartCoroutine(FireRoutine());
        }
    }

    private IEnumerator FireRoutine()
    {
        while (gameObject.activeInHierarchy)
        {
            GameObject bullet = pool.GetBullet(firePoint.position);
            bullet.GetComponent<Bullet>().SetPool(pool);
            bullet.GetComponent<Rigidbody2D>().linearVelocity = firePoint.up * 5f;

            yield return new WaitForSeconds(1f / fireRate);
        }
    }

    
}
