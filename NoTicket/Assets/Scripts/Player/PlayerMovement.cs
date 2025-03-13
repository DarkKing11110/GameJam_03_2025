using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Transform GroundCheck;
    public LayerMask GroundLayer;
    private Rigidbody2D rb;
    private CapsuleCollider2D playerCollider;

    [Header("Movement Settings")]
    [SerializeField]
    private float groundSpeed;
    [SerializeField]
    private float airSpeed;
    [SerializeField]
    private float crouchSpeed;

    [Header("Jump Settings")]
    [SerializeField]
    private bool isDoubleJumpAllowed = false;
    [SerializeField]
    private float jumpingPower;
    [SerializeField]
    private int maxJumpCount;

    [Header("Gravity Settings")]
    [SerializeField]
    private float normalGravityScale;
    [SerializeField]
    private float fallingGravityScale;

    [Header("Dodge Settings")]
    [SerializeField]
    private float dodgeForce;
    [SerializeField]
    private float dodgeCooldown;
    private float dodgeCooldownTimer;

    private float horizontal;
    private float currentSpeed;
    private int jumpCount;
    private float timeOnGround;
    private bool isFacingRight = true;
    private bool isCrouching = false;
    private bool isDodging = false;

    private float originalColliderHeight;
    private float crouchColliderHeight;
    private float dodgeColliderHeight;
    private float originalColliderRadius;
    private float crouchColliderRadius;

    private bool isJumping = false;
    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();

        rb.gravityScale = normalGravityScale;

        originalColliderHeight = playerCollider.size.y;
        originalColliderRadius = playerCollider.size.x / 2;

        crouchColliderHeight = originalColliderHeight / 1.88f;
        crouchColliderRadius = originalColliderRadius;

        dodgeColliderHeight = originalColliderHeight / 1.88f;
    }

    private void Update()
    {
        dodgeCooldownTimer -= Time.deltaTime;

        if (!isDodging)
        {
            rb.linearVelocity = new Vector2(horizontal * currentSpeed, rb.linearVelocity.y);
        }

        SpeedJumpAndGroundCheck();
        GetDirectionAndFlip();
        AdjustGravityScale();
    }

    private void SpeedJumpAndGroundCheck()
    {
        if (IsGrounded())
        {
            timeOnGround = 0f;
            jumpCount = 0;

            if (isCrouching)
            {
                currentSpeed = crouchSpeed;
            }
            else
            {
                currentSpeed = groundSpeed;
            }

            isJumping = false;
            isFalling = false;
        }
        else
        {
            timeOnGround += Time.deltaTime;
            currentSpeed = airSpeed;

            // Check for isJumping
            if (rb.linearVelocity.y > 0.1f)
            {
                isJumping = true;
                isFalling = false;
            }
            // check for isFalling
            else if (rb.linearVelocity.y < -0.1f)
            {
                isFalling = true;
                isJumping = false;
            }
        }
    }

    private void GetDirectionAndFlip()
    {
        if (!isFacingRight)
        {
            if (horizontal > 0f)
            {
                Flip();
            }
        }
        else
        {
            if (horizontal < 0f)
            {
                Flip();
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (IsGrounded())
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                jumpCount = 1;

                // reset collider if player jumps
                ResetColliderSize();
            }
            else
            {
                if (isDoubleJumpAllowed && jumpCount < maxJumpCount)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                    jumpCount++;
                }
                else if (timeOnGround < 0.1f && jumpCount == 1)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                    jumpCount++;
                }
            }
        }

        if (context.canceled)
        {
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        horizontal = input.x;
    }

    public void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed && !isJumping) // Check if player is in air
        {
            isCrouching = true;
            currentSpeed = crouchSpeed;

            // set smaller collider if player crouchs
            playerCollider.size = new Vector2(crouchColliderRadius * 2, crouchColliderHeight);
            playerCollider.offset = new Vector2(playerCollider.offset.x, -(originalColliderHeight - crouchColliderHeight) / 2); // Offset anpassen
        }

        if (context.canceled)
        {
            isCrouching = false;
            currentSpeed = groundSpeed;

            ResetColliderSize();
        }
    }

    public void Dodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (dodgeCooldownTimer <= 0f)
            {
                if (IsGrounded())
                {
                    isDodging = true;
                    dodgeCooldownTimer = dodgeCooldown;

                    // set smaller collider if player dodges
                    playerCollider.size = new Vector2(crouchColliderRadius * 2, dodgeColliderHeight);
                    playerCollider.offset = new Vector2(playerCollider.offset.x, -(originalColliderHeight - dodgeColliderHeight) / 2); // Offset anpassen

                    // dodge direction and speed
                    float dodgeDirection = 0f;

                    if (isFacingRight)
                    {
                        dodgeDirection = 1f;
                    }
                    else
                    {
                        dodgeDirection = -1f;
                    }

                    // while dodge double speed 
                    rb.linearVelocity = new Vector2(dodgeDirection * (groundSpeed * 2), rb.linearVelocity.y);

                    Invoke(nameof(EndDodge), 0.3f); // dodge duration
                }
            }
        }
    }

    private void EndDodge()
    {
        isDodging = false;
        ResetColliderSize();
    }

    private void ResetColliderSize()
    {
        // set collider to original size
        playerCollider.size = new Vector2(originalColliderRadius * 2, originalColliderHeight);
        playerCollider.offset = new Vector2(playerCollider.offset.x, 0f);
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(GroundCheck.position, Vector3.down, Color.red, 0.2f);
        bool grounded = Physics2D.OverlapCircle(GroundCheck.position, 0.2f, GroundLayer);

        return grounded;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;

        Vector3 localScale = transform.localScale;
        localScale.x = localScale.x * -1f;
        transform.localScale = localScale;
    }

    private void AdjustGravityScale()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = fallingGravityScale;
        }
        else
        {
            rb.gravityScale = normalGravityScale;
        }
    }
}
