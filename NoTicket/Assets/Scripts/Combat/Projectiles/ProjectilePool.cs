using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private int poolSize;
    [SerializeField]
    private Transform bulletParent;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletParent);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            bullet.GetComponent<Projectile>().SetPool(this);
            return bullet;
        }
        else
        {
            return null;
        }
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
