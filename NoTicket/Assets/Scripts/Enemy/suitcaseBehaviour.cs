using UnityEngine;

public class SuitcaseBehaviour : MonoBehaviour
{
    public float damage = 10.0f;
    public float health = 20.0f;
    public float moveSpeed = 2.0f;
    public float patrolDistance = 5.0f;
    public LayerMask playerLayer;
    public Transform topCheck;
    public float topCheckRadius = 0.5f;

    private Vector2 startPosition;
    private bool movingRight = true;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Patrol();
        CheckPlayerOnTop();
    }

    private void Patrol()
    {
        if (movingRight)
        {
            transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
            if (Vector2.Distance(startPosition, transform.position) >= patrolDistance)
            {
                movingRight = false;
            }
        }
        else
        {
            transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
            if (Vector2.Distance(startPosition, transform.position) <= 0)
            {
                movingRight = true;
            }
        }
    }

    private void CheckPlayerOnTop()
    {
        Collider2D playerOnTop = Physics2D.OverlapCircle(topCheck.position, topCheckRadius, playerLayer);
        if (playerOnTop != null)
        {
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            /*PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
            */
        }
    }

    private void Die()
    {
        // Trigger death animation or effects here if any
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (topCheck == null)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(topCheck.position, topCheckRadius);
    }
}

