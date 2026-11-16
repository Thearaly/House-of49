using UnityEngine;

public class RPSBoardInput : MonoBehaviour
{
    [Header("References")]
    public RPSBoardManager boardManager;

    [Header("Board Mode")]
    public bool boardModeActive;

    void Update()
    {
        if (!boardModeActive || boardManager == null) return;

        if (Input.GetKeyDown(KeyCode.W))
            boardManager.MoveCube(new Vector2Int(0, 1));
        else if (Input.GetKeyDown(KeyCode.A))
            boardManager.MoveCube(new Vector2Int(-1, 0));
        else if (Input.GetKeyDown(KeyCode.S))
            boardManager.MoveCube(new Vector2Int(0, -1));
        else if (Input.GetKeyDown(KeyCode.D))
            boardManager.MoveCube(new Vector2Int(1, 0));
    }

    public void EnterBoardMode()
    {
        boardModeActive = true;
    }

    public void ExitBoardMode()
    {
        boardModeActive = false;
    }
}
