using UnityEngine;

public class LolliCollectable : CollectableBase
{
    [SerializeField] private int lolliValue = 1;

    protected override void OnCollect()
    {
        CollectableManager.Instance.AddLolli(lolliValue);
    }
}
