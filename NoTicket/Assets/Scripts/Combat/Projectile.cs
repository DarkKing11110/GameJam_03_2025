using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed;
    private float lifetime;
    private float currentLifetime;

    public Vector2 direction;

    private void Start()
    {

        currentLifetime = lifetime;
    }

    private void Update()
    {

        transform.Translate(direction * speed * Time.deltaTime);

        currentLifetime -= Time.deltaTime;
        if (currentLifetime <= 0f)
        {
            Destroy(gameObject); 
        }
    }


    public virtual void OnHit(Collider2D collider)
    {
        Destroy(gameObject); 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnHit(collision.collider);
    }
}
