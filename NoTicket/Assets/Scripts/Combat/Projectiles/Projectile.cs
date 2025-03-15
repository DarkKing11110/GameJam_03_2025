using UnityEngine;

public enum ProjectileType
{
    Straight,
    Homing, 
    Bouncing
}

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifetime;
    [SerializeField]
    private float damageAmount;
    [SerializeField]
    private float bounceFactor;

    [SerializeField]
    private ProjectileType projectileType = ProjectileType.Straight;

    private float timer;
    private Vector2 direction;
    private Vector2 originalDirection;

    private ProjectilePool originPool;
    private Transform playerTransform;

    public void SetPool(ProjectilePool pool)
    {
        originPool = pool;
    }

    void OnEnable()
    {
        timer = 0f;
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        originalDirection = direction;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            DeactivateProjectile();
        }

        switch (projectileType)
        {
            case ProjectileType.Straight:
                MoveStraight();
                break;

            case ProjectileType.Homing:
                HomingMovement();
                break;

            case ProjectileType.Bouncing:
                BounceMovement();
                break;
        }
    }

    private void MoveStraight()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void HomingMovement()
    {
        if (playerTransform != null)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            transform.Translate(directionToPlayer * speed * Time.deltaTime, Space.World);
        }
    }

    private void BounceMovement()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damageAmount);
            DeactivateProjectile();
        }

        if (projectileType == ProjectileType.Bouncing)
        {
            // BounceVector of first position
            Vector2 contactNormal = collider.transform.position - transform.position;
            direction = Vector2.Reflect(direction, contactNormal.normalized) * bounceFactor; // reflected Vector
        }
    }

    private void DeactivateProjectile()
    {
        if (originPool != null)
        {
            originPool.ReturnBullet(gameObject);
        }
        else
        {
            Debug.LogWarning("Origin Pool ist nicht gesetzt!");
        }
        gameObject.SetActive(false);
    }

    public void Fire(Vector2 fireDirection)
    {
        direction = fireDirection.normalized;
    }
}
