using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    private float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

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
        // Death Logic
        Destroy(gameObject);
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
