using UnityEngine;

public class ServiceWagonBehaviour : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float damage = 10.0f;
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;

    private Rigidbody2D rb;
    private bool playerDetected = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        DetectPlayer();
        if (playerDetected)
        {
            MoveLeft();
        }
    }

    private void DetectPlayer()
    {
        Collider2D player = Physics2D.OverlapCircle(transform.position, detectionRadius, playerLayer);
        if (player != null)
        {
            playerDetected = true;
        }
    }

    private void MoveLeft()
    {
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
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
            Die();
        }
        else
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}


