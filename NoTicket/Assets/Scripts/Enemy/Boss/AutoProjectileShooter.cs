using UnityEngine;

public class AutoProjectileShooter : MonoBehaviour
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private float fireRate;            
    [SerializeField]
    private float fireCooldown;        
    [SerializeField]
    private Vector2 fireDirection; 

    [SerializeField]
    private ProjectilePool projectilePool;

    void Update()
    {
        if (fireCooldown <= 0f)
        {
            FireProjectile();
            fireCooldown = 1f / fireRate;
        }
        else
        {
            fireCooldown -= Time.deltaTime;
        }
    }

    private void FireProjectile()
    {
        if (projectilePool != null)
        {
            GameObject projectileInstance = projectilePool.GetBullet();

            if (projectileInstance != null && projectileSpawnPoint != null)
            {
                projectileInstance.transform.position = projectileSpawnPoint.position;

                Projectile projectile = projectileInstance.GetComponent<Projectile>();
                if (projectile != null)
                {
                    projectile.Fire(fireDirection);
                }
            }
        }
    }
}
