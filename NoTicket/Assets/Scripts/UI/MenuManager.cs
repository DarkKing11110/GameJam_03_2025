using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.Rendering.BoolParameter;

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

    public Image splashImage;
    public float displayTime = 3f;
    private void Start()
    {

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

    public void GameOver()
    {
        if (gameOverMenu != null)
        {
            gameOverMenu.SetActive(true);
        }
    }

    public void Play()
    {
        StartCoroutine(ShowSplashAndStart());
    }
    private IEnumerator ShowSplashAndStart()
    {
        splashImage.gameObject.SetActive(true);

        yield return new WaitForSecondsRealtime(displayTime);

        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void LoadGame()
    {
        SaveSystem saveSystem = SaveSystem.Instance;

        if (saveSystem == null)
        {
            Debug.LogError("SaveSystem ist nicht verfügbar!");
            return;
        }

        SaveData loadedData = saveSystem.Load();

        if (loadedData != null)
        {

            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {

                Vector3 loadedPosition = new Vector3(loadedData.PlayerPosition.x, loadedData.PlayerPosition.y, loadedData.PlayerPosition.z);

                player.transform.position = loadedPosition;
                Debug.Log("Spielerposition geladen: " + loadedPosition);

                CollectableManager.Instance.SetCoinCount(loadedData.Coins);
                CollectableManager.Instance.SetLolliCount(loadedData.Lollis);

                Debug.Log("Coins geladen: " + loadedData.Coins);
                Debug.Log("Lollis geladen: " + loadedData.Lollis);
            }


            StartCoroutine(LoadSceneAfterDelay(1));
        }
        else
        {
            Debug.Log("Kein Spiel gespeichert. Starte ein neues Spiel.");
            StartNewGame();
        }
    }

    private IEnumerator LoadSceneAfterDelay(int sceneIndex)
    {

        yield return new WaitForEndOfFrame(); 

        SceneManager.LoadScene(sceneIndex);
    }




    public void StartNewGame()
    {
        SaveSystem.Instance.Save(0, 0, Vector3.zero);
        SceneManager.LoadScene(1);
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
