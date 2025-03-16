using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] 
    private TextMeshProUGUI timerText;

    private float elapsedTime = 0f;
    private bool isRunning = false;

    private void Start()
    {
        ResetTimer();
        StartTimer();
    }

    private void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        UpdateTimerUI();
    }

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");

    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerUI();
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }
}
