using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    public Transform GroundCheck;
    public LayerMask GroundLayer;
    private Rigidbody2D rb;
    private CapsuleCollider2D playerCollider;
    private PlayerAnimationController animationController;

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
    private float dodgeSpeed;
    [SerializeField]
    private float dodgeTime;
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

    private float crouchColliderHeight;
    private float dodgeColliderHeight;
    private float originalColliderHeight;
    private Vector2 originalColliderOffset;

    private bool isJumping = false;
    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<CapsuleCollider2D>();
        animationController = GetComponentInChildren<PlayerAnimationController>();

        rb.gravityScale = normalGravityScale;

        // Original collider size
        originalColliderHeight = playerCollider.size.y;
        originalColliderOffset = playerCollider.offset;

        // For crouching and dodging
        crouchColliderHeight = originalColliderHeight * 0.5f;
        dodgeColliderHeight = originalColliderHeight * 0.5f;
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
            // Check for isFalling
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
                animationController.SetIsJumping(true);
                ResetColliderSize();
            }
            else
            {
                if (isDoubleJumpAllowed && jumpCount < maxJumpCount)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                    jumpCount++;
                    animationController.SetIsJumping(true);
                }
                else if (timeOnGround < 0.1f && jumpCount == 1)
                {
                    rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
                    jumpCount++;
                    animationController.SetIsJumping(true);
                }
            }
        }

        if (context.canceled)
        {
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            }
            animationController.SetIsJumping(false);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        horizontal = input.x;

        if (IsGrounded())
        {
            bool isRunning = Mathf.Abs(horizontal) > 0.1f;
            animationController.SetIsRunning(isRunning);
        }
        else
        {
            animationController.SetIsRunning(false);
        }
    }

    private void Crouch(InputAction.CallbackContext context)
    {
        if (context.performed && !isJumping)
        {
            isCrouching = true;
            currentSpeed = crouchSpeed;
            animationController.SetIsCrouching(true);

            playerCollider.size = new Vector2(playerCollider.size.x, crouchColliderHeight);
            playerCollider.offset = new Vector2(playerCollider.offset.x, (originalColliderHeight - crouchColliderHeight) / 2);
        }

        if (context.canceled)
        {
            isCrouching = false;
            currentSpeed = groundSpeed;
            animationController.SetIsCrouching(false);
            ResetColliderSize();
        }
    }

    private void Dodge(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (dodgeCooldownTimer <= 0f)
            {
                if (IsGrounded())
                {
                    isDodging = true;
                    dodgeCooldownTimer = dodgeCooldown;

                    playerCollider.size = new Vector2(playerCollider.size.x, dodgeColliderHeight);
                    playerCollider.offset = new Vector2(playerCollider.offset.x, (originalColliderHeight - dodgeColliderHeight) / 2);

                    float dodgeDirection = isFacingRight ? 1f : -1f;

                    rb.linearVelocity = new Vector2(dodgeDirection * (groundSpeed * dodgeSpeed), rb.linearVelocity.y);
                    animationController.SetIsCrouching(true);
                    Invoke(nameof(EndDodge), dodgeTime);
                }
            }
        }
    }

    private void EndDodge()
    {
        isDodging = false;
        animationController.SetIsCrouching(false);
        ResetColliderSize();
    }

    private void ResetColliderSize()
    {
        playerCollider.size = new Vector2(playerCollider.size.x, originalColliderHeight);
        playerCollider.offset = originalColliderOffset;
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
