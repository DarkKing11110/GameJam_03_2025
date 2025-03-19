using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }
    private string saveFilePath;
    private GameObject player;

    public static event Action<int, int, Vector3> OnSaveDataLoaded;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "savefile.json");

        // Szene-Event abonnieren
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Wird aufgerufen, sobald eine neue Szene geladen wurde
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Szene geladen: {scene.name}");

        StartCoroutine(TriggerEventAfterSceneLoad());
    }

    private IEnumerator TriggerEventAfterSceneLoad()
    {
        // Warte einen Frame, damit alle Objekte vollständig geladen sind
        yield return null;

        player = GameObject.Find("Player");
        if (player == null)
        {
            Debug.LogError("Player nicht gefunden.");
            yield break;
        }

        SaveData loadedData = Load();
        if (loadedData != null)
        {
            Debug.Log($"Gespeicherte Daten geladen: {loadedData.Coins}, {loadedData.Lollis}, {loadedData.PlayerPosition}");
            OnSaveDataLoaded?.Invoke(loadedData.Coins, loadedData.Lollis, loadedData.PlayerPosition);
        }
        else
        {
            Debug.LogWarning("Keine gespeicherten Daten gefunden.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Daten speichern
    public void Save(int coins, int lollis, Vector3 position)
    {
        SaveData saveData = new SaveData(coins, lollis, position);
        string json = JsonUtility.ToJson(saveData);

        try
        {
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"Daten erfolgreich gespeichert: {json}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Fehler beim Speichern der Datei: {e.Message}");
        }
    }

    // Daten laden
    public SaveData Load()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SaveData loadedData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log($"Daten geladen aus JSON: {json}");
            return loadedData;
        }
        else
        {
            Debug.LogWarning("Keine gespeicherte Datei gefunden.");
            return null;
        }
    }
}
