using UnityEngine;

public class SaveGameManager : MonoBehaviour
{
    public static SaveGameManager Instance { get; private set; }

    private SaveData saveData = new SaveData();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadGame();
    }

    public void SaveGame(Vector3? checkpointPos = null)
    {
        saveData.Coins = CollectableManager.Instance.GetCoinCount();
        saveData.Lollis = CollectableManager.Instance.GetLolliCount();

        if (checkpointPos.HasValue)
        {
            saveData.CheckpointPosition = checkpointPos.Value;
        }

        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString("SaveData", json);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        string json = PlayerPrefs.GetString("SaveData", "");
        if (!string.IsNullOrEmpty(json))
        {
            saveData = JsonUtility.FromJson<SaveData>(json);

            CollectableManager.Instance.SetCoinCount(saveData.Coins);
            CollectableManager.Instance.SetLolliCount(saveData.Lollis);
        }
        else
        {
            saveData = new SaveData();
        }
    }

    public void LoadCheckpoint()
    {
        CollectableManager.Instance.SetCoinCount(saveData.Coins);
        CollectableManager.Instance.SetLolliCount(saveData.Lollis);

        Vector3 checkpointPos = saveData.CheckpointPosition;

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = checkpointPos;
        }
    }

    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteKey("SaveData");
        PlayerPrefs.Save();
        saveData = new SaveData();
    }
}
