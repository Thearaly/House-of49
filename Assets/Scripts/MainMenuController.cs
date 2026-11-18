
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject instructionPanel;

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("House-of49");
    }

    public void ShowInstructions()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false);

        if (instructionPanel != null)
            instructionPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        if (instructionPanel != null)
            instructionPanel.SetActive(false);

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }
}
