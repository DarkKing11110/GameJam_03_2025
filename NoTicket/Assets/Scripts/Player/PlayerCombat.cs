using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            animator.SetTrigger("Attack");
            Debug.Log("Player is Attacking!");
        }
    }
}
