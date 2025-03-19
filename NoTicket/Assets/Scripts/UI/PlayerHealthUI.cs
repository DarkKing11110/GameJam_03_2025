using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("Herzen Setup")]
    [SerializeField] 
    private GameObject heartPrefab;
    [SerializeField] 
    private Sprite fullHeart;
    [SerializeField] 
    private Sprite emptyHeart;

    [SerializeField] private PlayerHealth playerHealth;

    private Image[] hearts;

    private void OnEnable()
    {
        playerHealth.OnHealthChanged += UpdateHearts;
    }

    private void OnDisable()
    {
        playerHealth.OnHealthChanged -= UpdateHearts;
    }
    private void Start()
    {
        CreateHearts();
        UpdateHearts();
    }

    private void CreateHearts()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        int maxHealth = Mathf.RoundToInt(playerHealth.MaxHealth);

        hearts = new Image[maxHealth];

        for (int i = 0; i < maxHealth; i++)
        {

            GameObject heartObj = Instantiate(heartPrefab, transform);

            if (heartObj == null)
            {
                continue;
            }

            Image heartImage = heartObj.GetComponent<Image>();

            if (heartImage == null)
            {
                continue;
            }

            hearts[i] = heartImage;
        }
    }

    private void UpdateHearts()
    {
        int currentHealth = Mathf.RoundToInt(playerHealth.GetCurrentHealth());
        int maxHealth = Mathf.RoundToInt(playerHealth.MaxHealth);

        if (hearts == null || hearts.Length == 0)
        {
            CreateHearts();
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null)
            {
                continue;
            }

            if (i < currentHealth)
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
