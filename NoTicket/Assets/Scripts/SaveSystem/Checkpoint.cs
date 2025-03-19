using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private SaveSystem saveSystem;

    private void Start()
    {
        // Sicherstellen, dass das SaveSystem im Spiel existiert und instanziiert wurde
        saveSystem = SaveSystem.Instance;
        if (saveSystem == null)
        {
            Debug.LogError("Kein SaveSystem gefunden!");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int currentCoins = 10; // Ersetze mit der aktuellen Anzahl von Coins
            int currentLollis = 5; // Ersetze mit der aktuellen Anzahl von Lollis
            Vector3 playerPosition = other.transform.position;

            saveSystem.Save(currentCoins, currentLollis, playerPosition);
            Debug.Log("Check reached");
        }
    }
}
