using Unity.VisualScripting;
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

    [Header("Level References")]
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject gameOverMenu;
    private bool isPaused = false;

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
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
            }
        }
    }
    public void Continue()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
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
    public void Play()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void GameOver()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
        }
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // Beendet das Spiel
#endif
    }
}
