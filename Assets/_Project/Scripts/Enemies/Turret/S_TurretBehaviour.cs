using System.Collections;
using UnityEngine;

public class TurretBehaviour : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float bulletSpeed = 5f;

    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParticleSystem fireParticles;
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
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            Vector2 velocity = firePoint.up * bulletSpeed;
            bulletScript.Init(pool, velocity);

            fireParticles.Play();

            yield return new WaitForSeconds(1f / fireRate);
        }
    }

    
}
