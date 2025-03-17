using UnityEngine;
using UnityEngine.AI;

public class TicketInspectorBehaviour : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float jumpForce = 5.0f;
    public LayerMask playerLayer;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Transform player;
    private NavMeshAgent agent;
    private Rigidbody2D rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        agent.speed = moveSpeed;
        agent.SetDestination(player.position);

        if (isGrounded && ShouldJump())
        {
            Jump();
        }
    }

    private bool ShouldJump()
    {
        // Check if there is an obstacle in front of the enemy
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right * Mathf.Sign(player.position.x - transform.position.x), 1.0f, groundLayer);
        return hit.collider != null && Mathf.Abs(player.position.y - transform.position.y) > 1.0f;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1000000);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}

