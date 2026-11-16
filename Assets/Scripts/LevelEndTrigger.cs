using UnityEngine;
using UnityEngine.SceneManagement;

// Attach to: an empty object just PAST the exit door (Box Collider, "Is Trigger" ticked)
public class LevelEndTrigger : MonoBehaviour
{
    [Tooltip("Name of the next scene. Leave empty to just show the Win screen.")]
    public string nextSceneName = "";

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() == null) return;
        if (GameManager.Instance == null || !GameManager.Instance.HasKey) return;

        if (string.IsNullOrEmpty(nextSceneName))
        {
            GameManager.Instance.WinGame();           // show Win screen
        }
        else
        {
            GameManager.Instance.WinGame();           // stops the timer
            SceneManager.LoadScene(nextSceneName);    // go to the next level
        }
    }
}