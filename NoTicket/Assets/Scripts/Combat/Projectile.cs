using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float lifetime;

    private float timer;
    private Vector2 direction;

    void OnEnable()
    {
        timer = 0f;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);

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
        ProjectilePool.Instance.ReturnBullet(gameObject);
    }
}
