using UnityEngine;

public class RPSBoardInteractable : MonoBehaviour, IInteractable
{
    [Header("Board UI")]
    public GameObject boardPanel;
    public RPSBoardInput boardInput;

    public string InteractionMessage => "Play Rock Paper Scissors";

    public void Interact()
    {
        if (boardPanel != null) boardPanel.SetActive(true);
        if (boardInput != null) boardInput.EnterBoardMode();
        Debug.Log("RPS board opened. Roll dice, then use WASD.");
    }

    public void CloseBoard()
    {
        if (boardPanel != null) boardPanel.SetActive(false);
        if (boardInput != null) boardInput.ExitBoardMode();
    }
}
