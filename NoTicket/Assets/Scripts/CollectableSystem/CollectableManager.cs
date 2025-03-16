using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectableManager : MonoBehaviour
{
    public static CollectableManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] 
    private TextMeshProUGUI lolliCounterText;
    [SerializeField] 
    private Image lolliIcon;

    private int lolliCount = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateLolliUI();
    }

    public void AddLolli(int amount)
    {
        lolliCount += amount;
        UpdateLolliUI();
    }

    private void UpdateLolliUI()
    {
        if (lolliCounterText != null)
        {
            lolliCounterText.text = lolliCount.ToString();
        }
    }

    public int GetLolliCount()
    {
        return lolliCount;
    }
}
