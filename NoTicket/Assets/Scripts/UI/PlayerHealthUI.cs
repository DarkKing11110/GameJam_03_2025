using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Herzen Setup")]
    [SerializeField]
    private Image[] hearts;
    [SerializeField]
    private Sprite fullHeart;
    [SerializeField]
    private Sprite emptyHeart;

    [SerializeField]
    private PlayerHealth playerHealth;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHearts;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHearts;
    }

    private void UpdateHearts()
    {
        float currentHealth = playerHealth.GetCurrentHealth();
        float maxHealth = playerHealth.MaxHealth;

        int healthPerHeart = Mathf.CeilToInt(maxHealth / hearts.Length);

        for (int i = 0; i < hearts.Length; i++)
        {
            if (currentHealth >= (i + 1) * healthPerHeart)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }
    }
}
