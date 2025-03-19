using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float maxHealth;
    public float MaxHealth => maxHealth;
    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private MenuManager menuManager;

    public delegate void HealthChangedHandler();
    public event HealthChangedHandler OnHealthChanged;

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

        OnHealthChanged?.Invoke();
    }

    public void Heal(float healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);

        OnHealthChanged?.Invoke();
    }

    private void Die()
    {
        Debug.Log("Maddie ist gestorben!");
        gameObject.SetActive(false);
        menuManager.GameOver();

    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
}
