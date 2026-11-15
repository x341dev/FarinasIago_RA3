using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialSize = 10;

    private Stack<GameObject> freeBullets = new Stack<GameObject>();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        { 
            freeBullets.Push(CreateNewBullet());
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        ReturnAllBullets();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        ReturnAllBullets();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReturnAllBullets();
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

    private void ReturnAllBullets()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                ReturnBullet(child.gameObject);
            }
        }
    }
}
