using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialSize = 10;

    public static BulletPool Instance { get; private set; }

    private Stack<GameObject> freeBullets = new Stack<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < initialSize; i++)
        { 
            freeBullets.Push(CreateNewBullet());
        }
    }


    private GameObject CreateNewBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab);
        newBullet.transform.SetParent(transform);
        newBullet.SetActive(false);
        return newBullet;
    }

    public GameObject GetBullet(Vector3 position)
    {
        GameObject bullet;

        if (freeBullets.Count > 0)
            bullet = freeBullets.Pop();
        else
            bullet = CreateNewBullet();

        bullet.transform.position = position;
        bullet.SetActive(true);
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        freeBullets.Push(bullet);
    }
}
