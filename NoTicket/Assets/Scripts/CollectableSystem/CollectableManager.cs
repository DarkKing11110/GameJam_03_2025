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
    [SerializeField]
    private TextMeshProUGUI coinCounterText;
    [SerializeField]
    private Image coinIcon;

    private int lolliCount = 0;
    private int coinCount = 0;
    private int totalCoins = 0;
    private int totalLollis = 0;

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
        UpdateCoinUI();
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

    public void SetLolliCount(int amount)
    {
        lolliCount = amount;
        UpdateLolliUI();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        if (coinCounterText != null)
        {
            coinCounterText.text = coinCount.ToString();
        }
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void SetCoinCount(int amount)
    {
        coinCount = amount;
        UpdateCoinUI();
    }

    public void SetTotalCoins(int amount)
    {
        totalCoins = amount;
    }

    public int GetTotalCoins()
    {
        return totalCoins;
    }

    public void SetTotalLollis(int amount)
    {
        totalLollis = amount;
    }

    public int GetTotalLollis()
    {
        return totalLollis;
    }
}
