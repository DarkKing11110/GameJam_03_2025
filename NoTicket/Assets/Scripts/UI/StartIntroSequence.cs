using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class StartGame : MonoBehaviour
{
    public Image splashImage;
    public float displayTime = 2f;
    private bool isPaused = false;

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
}
