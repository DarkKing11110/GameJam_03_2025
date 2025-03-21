using UnityEngine;

public class BossAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetIsAngry(bool value)
    {
        animator.SetBool("isAngry", value);
    }

    public void SetIsBlowing(bool value)
    {
        animator.SetBool("isBlowing", value);
    }


    public void SetIsHit()
    {
        animator.SetTrigger("isHit");
    }

    public void SetIsDead(bool value)
    {
        animator.SetBool("isDead", value);
    }
}
