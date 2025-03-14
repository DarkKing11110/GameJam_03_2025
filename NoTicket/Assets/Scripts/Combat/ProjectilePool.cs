using UnityEngine;
using System.Collections.Generic;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance;

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
        // Singleton-Pattern
        Instance = this;

        // Initialzation active bullet and deactivate
        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletParent);
            bullet.SetActive(false);  // at start all bullets deactive
            bulletPool.Enqueue(bullet);  // que bullets to pool
        }
    }

    public GameObject GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            GameObject bullet = bulletPool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            Debug.LogWarning("Pool is empty!");
            return null;
        }
    }
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
