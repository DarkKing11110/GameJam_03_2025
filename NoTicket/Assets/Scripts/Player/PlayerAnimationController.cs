using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetIsRunning(bool value)
    {
        animator.SetBool("isRunning", value);
    }

    public void SetIsJumping(bool value)
    {
        animator.SetBool("isJumping", value);
    }


    public void SetIsCrouching(bool value)
    {
        animator.SetBool("isCrouching", value);
    }

    public void SetIsShooting(bool value)
    {
        animator.SetBool("isShooting", value);
    }

    public void SetIsReloading(bool value)
    {
        animator.SetBool("isReloading", value);
    }
    public void SetIsHit(bool value)
    {
        animator.SetBool("isHit", value);
    }

    public void SetIsDead(bool value)
    {
        animator.SetBool("isDead", value);
    }
}
