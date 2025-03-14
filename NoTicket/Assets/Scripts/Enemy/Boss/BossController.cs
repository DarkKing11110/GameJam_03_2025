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

    private float lastAttackTime;
    private Transform playerTransform;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            if (distanceToPlayer < attackRange)
            {
                // Melee
            }
            else if (distanceToPlayer < rangedAttackRange)
            {
                PerformAttack();
            }
        }
    }

    private void PerformAttack()
    {
        RangedAttack();
        lastAttackTime = Time.time;
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
    }

}
