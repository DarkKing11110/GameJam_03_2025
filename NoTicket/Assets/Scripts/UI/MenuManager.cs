using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu;

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
    public void Play()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        isPaused = false;
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
