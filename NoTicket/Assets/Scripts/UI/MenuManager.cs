using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("MainMenu References")]
    [SerializeField] 
    private GameObject mainMenu;
    [SerializeField] 
    private GameObject shopMenu;
    [SerializeField] 
    private GameObject optionsMenu;
    [SerializeField] 
    private GameObject continueButton;

    [Header("Level References")]
    [SerializeField] 
    private GameObject pauseMenu;
    [SerializeField] 
    private GameObject gameOverMenu;
    private bool isPaused = false;

    private void Start()
    {
        if (continueButton != null)
        {
            continueButton.SetActive(PlayerPrefs.HasKey("SaveData"));
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            }
            else
            {
                Continue();
            }
        }
    }

    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void SaveGame()
    {
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        SaveGameManager.Instance.SaveGame(playerPos);
        Debug.Log("Game saved!");
    }

    public void LoadCheckpoint()
    {
        SaveGameManager.Instance.LoadCheckpoint();
    }

    public void GameOver()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
        }
    }

    public void Play()
    {
        //if (PlayerPrefs.HasKey("SaveData"))
        //{
        //    SaveGameManager.Instance.LoadGame();
        //}
        //else
        //{
        //    Debug.Log("Kein Speicherstand gefunden, neues Spiel starten.");
        //    SaveGameManager.Instance.ResetAllProgress();
        //}

        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SaveData"))
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Debug.Log("No Save Data found");
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ShopMenu()
    {
        mainMenu.SetActive(false);
        shopMenu.SetActive(true);
    }

    public void OptionsMenu()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        shopMenu.SetActive(false);
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
