
using UnityEngine;
using TMPro;

public enum RPSSymbol
{
    Rock,
    Paper,
    Scissors
}

public class RPSBoardManager : MonoBehaviour
{
    [Header("Board")]
    public BoardCell[] cells = new BoardCell[49];
    public CubeMovement symbolCube;
    public DiceRoller diceRoller;

    [Header("Rules")]
    public int pointsToGetKey = 3;
    public int maximumRolls = 10;

    [Header("UI (optional)")]
    public TMP_Text scoreText;
    public TMP_Text rollsText;
    public TMP_Text resultText;

    private int score;
    private int rollsUsed;
    private bool roundActive;
    private int stepsRemaining;
    private bool moveInProgress;
    private bool waitingForDice;

    void Start()
    {
        UpdateUI();
    }

    // Called when the player clicks the dice.
    public void RollDice()
    {
        if (GameManager.Instance != null &&
            GameManager.Instance.CurrentState != GameState.Playing)
            return;

        if (score >= pointsToGetKey ||
            roundActive ||
            waitingForDice ||
            rollsUsed >= maximumRolls ||
            moveInProgress)
            return;

        if (diceRoller == null)
        {
            Debug.LogError("DiceRoller is not assigned!");
            return;
        }

        waitingForDice = true;

        if (resultText != null)
            resultText.text = "Rolling dice...";

        diceRoller.Roll(OnDiceRollCompleted);
    }

    // Receives the actual number after the dice stops.
    void OnDiceRollCompleted(int result)
    {
        waitingForDice = false;

        if (result <= 0)
        {
            if (resultText != null)
                resultText.text = "Dice roll failed. Try again.";

            return;
        }

        stepsRemaining = result;
        rollsUsed++;
        roundActive = true;

        if (resultText != null)
        {
            resultText.text =
                "Rolled " + stepsRemaining +
                ". Move with WASD.";
        }

        UpdateUI();
    }

    public void MoveCube(Vector2Int direction)
    {
        if (!roundActive ||
            stepsRemaining <= 0 ||
            symbolCube == null ||
            moveInProgress)
            return;

        bool started = symbolCube.MoveOneCell(
            direction,
            cells,
            OnCubeMoveCompleted
        );

        if (started)
        {
            moveInProgress = true;
        }
        else
        {
            if (resultText != null)
            {
                resultText.text =
                    "Board edge! Choose another direction.";
            }
        }
    }

    void OnCubeMoveCompleted()
    {
        moveInProgress = false;
        stepsRemaining--;

        if (stepsRemaining == 0)
        {
            roundActive = false;
            CheckLandingResult();
        }
        else
        {
            if (resultText != null)
            {
                resultText.text =
                    "Steps remaining: " + stepsRemaining;
            }
        }

        UpdateUI();
    }

    void CheckLandingResult()
    {
        BoardCell cell = symbolCube.CurrentCell;

        if (cell == null)
        {
            if (resultText != null)
                resultText.text = "No destination cell assigned.";

            return;
        }

        RPSSymbol cubeSymbol = symbolCube.GetBackSymbol();
        RPSSymbol cellSymbol = cell.symbol;

        bool won = Beats(cubeSymbol, cellSymbol);

        if (won)
        {
            score++;

            if (resultText != null)
            {
                resultText.text =
                    cubeSymbol + " beats " + cellSymbol +
                    "! +1 point.";
            }
        }
        else if (cubeSymbol == cellSymbol)
        {
            if (resultText != null)
                resultText.text = "Tie! No point.";
        }
        else
        {
            if (resultText != null)
            {
                resultText.text =
                    cellSymbol + " beats " + cubeSymbol +
                    ". No point.";
            }
        }

        if (score >= pointsToGetKey)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.GiveKey();

            if (resultText != null)
                resultText.text += " You earned the key!";
        }
        else if (rollsUsed >= maximumRolls)
        {
            if (GameManager.Instance != null)
                GameManager.Instance.LoseGame();
        }

        UpdateUI();
    }

    bool Beats(RPSSymbol first, RPSSymbol second)
    {
        return
            (first == RPSSymbol.Paper &&
             second == RPSSymbol.Rock) ||

            (first == RPSSymbol.Rock &&
             second == RPSSymbol.Scissors) ||

            (first == RPSSymbol.Scissors &&
             second == RPSSymbol.Paper);
    }

    void UpdateUI()
    {
        if (scoreText != null)
        {
            scoreText.text =
                "Score: " + score + "/" + pointsToGetKey;
        }

        if (rollsText != null)
        {
            rollsText.text =
                "Rolls: " + rollsUsed + "/" + maximumRolls;
        }
    }

    public void ResetBoardGame()
    {
        score = 0;
        rollsUsed = 0;
        roundActive = false;
        stepsRemaining = 0;
        moveInProgress = false;
        waitingForDice = false;

        UpdateUI();
    }
}
