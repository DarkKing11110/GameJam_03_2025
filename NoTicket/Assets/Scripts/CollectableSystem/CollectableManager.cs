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

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Event abonnieren
        SaveSystem.OnSaveDataLoaded += UpdateCollectablesFromSave;
    }

    private void OnDestroy()
    {
        // Event abmelden, falls das Objekt zerstört wird
        SaveSystem.OnSaveDataLoaded -= UpdateCollectablesFromSave;
    }

    private void Start()
    {
        UpdateLolliUI();
        UpdateCoinUI();
    }

    private void UpdateCollectablesFromSave(int coins, int lollis, Vector3 position)
    {
        coinCount = coins;
        lolliCount = lollis;
        UpdateCoinUI();
        UpdateLolliUI();
        Debug.Log("UI mit geladenen Werten aktualisiert.");
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

    public int GetLolliCount() => lolliCount;

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

    public int GetCoinCount() => coinCount;

    public void SetCoinCount(int amount)
    {
        coinCount = amount;
        UpdateCoinUI();
    }
}
