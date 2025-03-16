using UnityEngine;

public enum ProjectileType
{
    Straight,
    Homing,
    Bouncing
}

public class Projectile : MonoBehaviour
{
    [Header("Standard Settings")]
    [SerializeField]
    private ProjectileType projectileType = ProjectileType.Straight;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifetime;
    [SerializeField]
    private float damageAmount;

    [Header("Homing Settings")]
    [SerializeField]
    private float turnSpeed;
    [SerializeField]
    private float homingDuration;

    [Header("Bouncing Settings")]
    [SerializeField]
    private float bounceFactor;

    private float timer;
    private float homingTimer;
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
        homingTimer = 0f;

        playerTransform = GameObject.FindWithTag("Player")?.transform;
        originalDirection = direction;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= lifetime)
        {
            DeactivateProjectile();
            return;
        }

        switch (projectileType)
        {
            case ProjectileType.Straight:
                MoveStraight();
                break;

            case ProjectileType.Homing:
                HomingLogic();
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

    private void HomingLogic()
    {
        homingTimer += Time.deltaTime;

        if (homingTimer <= homingDuration)
        {
            HomingMovement();
        }
        else
        {
            // Nach der Homing-Phase fliegt es geradeaus weiter
            MoveStraight();
        }
    }

    private void HomingMovement()
    {
        if (playerTransform == null)
            return;

        // direction towards player
        Vector2 toPlayer = (playerTransform.position - transform.position).normalized;

        // current direction vector
        Vector2 currentDirection = direction;

        // angle between current direction and targetdirection 
        float angle = Vector2.SignedAngle(currentDirection, toPlayer);

        // max turn per spin
        float maxTurn = turnSpeed * Time.deltaTime;

        float clampedAngle = Mathf.Clamp(angle, -maxTurn, maxTurn);

        // calculate new direction with rotation
        Quaternion rotation = Quaternion.AngleAxis(clampedAngle, Vector3.forward);
        Vector2 newDirection = rotation * currentDirection;

        // Neue Richtung setzen
        direction = newDirection.normalized;

        // Bewegung
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
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
            // Calculate reflected Vector
            Vector2 contactNormal = collider.transform.position - transform.position;
            direction = Vector2.Reflect(direction, contactNormal.normalized) * bounceFactor;
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
