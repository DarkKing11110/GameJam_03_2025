using UnityEngine;

public class CoinCollectable : CollectableBase
{
    [SerializeField] 
    private int coinValue = 1;

    protected override void OnCollect()
    {
        CollectableManager.Instance.AddCoin(coinValue);
    }
}
