using UnityEngine;

public abstract class CollectableBase : MonoBehaviour
{
    [SerializeField] private string collectableName = "Collectable";

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    protected virtual void Collect()
    {
        Debug.Log($"{collectableName} collected!");
        OnCollect();
        Destroy(gameObject);
    }

    protected abstract void OnCollect();
}
