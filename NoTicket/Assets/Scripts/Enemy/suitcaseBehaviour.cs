using UnityEngine;

public class SuitcaseBehaviour : MonoBehaviour
{
    public float damage = 10.0f;
    public float moveSpeed = 2.0f;
    public float patrolDistance = 5.0f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    public Transform topCheck;
    public float topCheckRadius = 0.5f;
    public float obstacleCheckDistance = 0.1f;

    private bool movingLeft = true; // Start moving left
    private Rigidbody2D rb;
    private Vector2 initialPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialPosition = transform.position;
    }

    private void Update()
    {
        Patrol();
        CheckPlayerOnTop();
    }

    private void Patrol()
    {
        // Check for obstacles
        if (IsObstacleInFront())
        {
            movingLeft = !movingLeft;
        }

        if (movingLeft)
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
            if (Vector2.Distance(initialPosition, transform.position) >= patrolDistance)
            {
                movingLeft = false;
            }
        }
        else
        {
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
            if (Vector2.Distance(initialPosition, transform.position) <= 0.1f)
            {
                movingLeft = true;
            }
        }
    }

    private bool IsObstacleInFront()
    {
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, obstacleCheckDistance, obstacleLayer);
        Debug.DrawRay(transform.position, direction * obstacleCheckDistance, Color.red); // Debug: Draw the ray
        return hit.collider != null;
    }

    private void CheckPlayerOnTop()
    {
        Collider2D playerOnTop = Physics2D.OverlapCircle(topCheck.position, topCheckRadius, playerLayer);
        if (playerOnTop != null && playerOnTop.CompareTag("Player"))
        {
            Die();
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
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (topCheck == null)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(topCheck.position, topCheckRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(initialPosition, initialPosition + Vector2.right * patrolDistance);
        Gizmos.DrawLine(initialPosition, initialPosition + Vector2.left * patrolDistance);

        Gizmos.color = Color.red;
        Vector2 direction = movingLeft ? Vector2.left : Vector2.right;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * obstacleCheckDistance);
    }
}
