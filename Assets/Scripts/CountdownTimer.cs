using UnityEngine;
using TMPro;

// Attach to: an empty GameObject (e.g. "Timer") or the GameManager object
public class CountdownTimer : MonoBehaviour
{
    [Header("Settings")]
    public float totalTime = 300f;          // 5 minutes
    public float warningTime = 30f;         // text turns red under this

    [Header("UI (optional)")]
    public TMP_Text timerText;              // drag a TextMeshPro text here

    public float TimeRemaining { get; private set; }
    public bool IsRunning { get; private set; }

    void Start()
    {
        TimeRemaining = totalTime;
        UpdateUI();
    }

    void Update()
    {
        if (!IsRunning) return;

        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            IsRunning = false;
            UpdateUI();
            if (GameManager.Instance != null) GameManager.Instance.LoseGame();
            return;
        }
        UpdateUI();
    }

    public void StartTimer()
    {
        TimeRemaining = totalTime;
        IsRunning = true;
    }

    public void StopTimer() { IsRunning = false; }

    void UpdateUI()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(TimeRemaining / 60f);
        int seconds = Mathf.FloorToInt(TimeRemaining % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
        timerText.color = TimeRemaining <= warningTime ? Color.red : Color.white;
    }
}