using UnityEngine;

public class PlayerSaveDataReceiver : MonoBehaviour
{
    private void OnEnable()
    {
        // Abo des Events
        SaveSystem.OnSaveDataLoaded += UpdatePlayerPositionFromSave;
    }

    private void OnDisable()
    {
        // Abo des Events aufheben, wenn das Objekt zerstört wird
        SaveSystem.OnSaveDataLoaded -= UpdatePlayerPositionFromSave;
    }

    // Diese Methode wird aufgerufen, wenn das Event ausgelöst wird
    private void UpdatePlayerPositionFromSave(int coins, int lollis, Vector3 position)
    {
        // Die Position des Spielers aktualisieren
        transform.position = position;
        Debug.Log($"Spielerposition auf {position} gesetzt.");
    }
}
