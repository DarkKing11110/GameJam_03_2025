using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth;

    private BossAnimationController animationController;

    private void Start()
    {
        animationController = GetComponentInChildren<BossAnimationController>();
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        animationController.SetIsHit();
        Debug.Log("isHit");
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
    }

    private void Die()
    {
        Debug.Log("Boss ist gestorben!");
        animationController.SetIsDead(true);
        gameObject.SetActive(false);

    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
