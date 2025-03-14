using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifetime;

    private float timer;
    private Vector2 direction;

    private ProjectilePool originPool;

    public void SetPool(ProjectilePool pool)
    {
        originPool = pool;
    }

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            DeactivateProjectile();
        }
    }

    public void Fire(Vector2 fireDirection)
    {
        direction = fireDirection.normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DeactivateProjectile();
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
}
