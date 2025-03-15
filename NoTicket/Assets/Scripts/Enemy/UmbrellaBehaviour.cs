using UnityEngine;

public class UmbrellaBehaviour : MonoBehaviour
{
    public float fallSpeed = 5.0f;
    public float damage = 10.0f;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    private bool isDiving = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isDiving)
        {
            FlyOverPlayer();
        }
    }

    private void FlyOverPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, 0.5f, playerLayer);
        if (player != null)
        {
            isDiving = true;
            rb.linearVelocity = new Vector2(0, -fallSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
