using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { Menu, Playing, Win, Lose }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public CountdownTimer timer;
    public GameObject mainMenuPanel;
    public GameObject winPanel;
    public GameObject losePanel;

    [Header("Testing")]
    public bool autoStartForTesting = true;

    public GameState CurrentState { get; private set; } = GameState.Menu;
    public bool HasKey { get; private set; }

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    void Start()
    {
        SetState(GameState.Menu);
        if (autoStartForTesting) StartGame();
    }

    public void StartGame()
    {
        HasKey = false;
        SetState(GameState.Playing);
        if (timer != null) timer.StartTimer();
    }

    public void GiveKey()
    {
        HasKey = true;
        Debug.Log("Player got the key!");
    }

    public void WinGame()
    {
        if (CurrentState != GameState.Playing) return;
        if (timer != null) timer.StopTimer();
        SetState(GameState.Win);
    }

    public void LoseGame()
    {
        if (CurrentState != GameState.Playing) return;
        if (timer != null) timer.StopTimer();
        SetState(GameState.Lose);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void SetState(GameState newState)
    {
        CurrentState = newState;

        if (mainMenuPanel != null) mainMenuPanel.SetActive(newState == GameState.Menu);
        if (winPanel != null) winPanel.SetActive(newState == GameState.Win);
        if (losePanel != null) losePanel.SetActive(newState == GameState.Lose);

        Debug.Log("Game state: " + newState);
    }
}