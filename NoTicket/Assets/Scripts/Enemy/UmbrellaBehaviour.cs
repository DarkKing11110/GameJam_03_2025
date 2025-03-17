using UnityEngine;

public class UmbrellaBehaviour : MonoBehaviour
{
    public float fallSpeed = 5.0f;
    public float moveSpeed = 2.0f;
    public float damage = 10.0f;
    public float detectionRadius = 5.0f;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    private bool isDiving = false;
    private bool playerDetected = false;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isDiving)
        {
            DetectPlayer();
            if (playerDetected)
            {
                MoveLeft();
                CheckForDive();
            }
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

    private void CheckForDive()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, detectionRadius, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * detectionRadius);
    }
}


