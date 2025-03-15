using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField]
    private float attackRange;
    [SerializeField]
    private float rangedAttackRange;
    [SerializeField]
    private float attackCooldown;

    [SerializeField]
    private ProjectilePool bossProjectilePool;
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private PlayerHealth playerHealth;

    private float lastAttackTime;

    private void Update()
    {
        if (IsPlayerDead())
        {
            return;
        }
        DistanceCheck();
    }

    private bool IsPlayerDead()
    {
        return playerHealth == null || playerHealth.GetCurrentHealth() <= 0;
    }

    // Checks Player Distance 
    private void DistanceCheck()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (distanceToPlayer < attackRange)
            {
                // Melee-Angriff
            }
            else if (distanceToPlayer < rangedAttackRange)
            {
                RangedAttack();
            }
        }
    }

    private void RangedAttack()
    {
        GameObject projectile = bossProjectilePool.GetBullet();

        if (projectile != null)
        {
            projectile.transform.position = new Vector3(transform.position.x, transform.position.y, 0);

            Vector2 direction = (playerTransform.position - transform.position).normalized;

            projectile.GetComponent<Projectile>().Fire(direction);
        }
        lastAttackTime = Time.time;
    }
}
