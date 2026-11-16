
using System;
using System.Collections;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    [Header("Board coordinate (0..6)")]
    public int column;
    public int row;

    [Header("Starting face symbols")]
    public RPSSymbol frontSymbol = RPSSymbol.Rock;
    public RPSSymbol backSymbol = RPSSymbol.Paper;
    public RPSSymbol leftSymbol = RPSSymbol.Scissors;
    public RPSSymbol rightSymbol = RPSSymbol.Rock;
    public RPSSymbol topSymbol = RPSSymbol.Paper;
    public RPSSymbol bottomSymbol = RPSSymbol.Scissors;

    [Header("Motion")]
    public float cellMoveDuration = 0.3f;
    public float cellSize = 1f;

    public BoardCell CurrentCell { get; private set; }
    public bool IsMoving { get; private set; }

    public bool MoveOneCell(
        Vector2Int direction,
        BoardCell[] cells,
        Action onComplete = null
    )
    {
        if (IsMoving) return false;

        int nextColumn = column + direction.x;
        int nextRow = row + direction.y;

        if (nextColumn < 0 || nextColumn > 6 ||
            nextRow < 0 || nextRow > 6)
            return false;

        BoardCell destination = FindCell(nextColumn, nextRow, cells);

        if (destination == null) return false;

        StartCoroutine(RollToCell(direction, destination, onComplete));
        return true;
    }

    IEnumerator RollToCell(
        Vector2Int direction,
        BoardCell destination,
        Action onComplete
    )
    {
        IsMoving = true;

        Vector3 moveDirection = new Vector3(
            direction.x,
            direction.y,
            0f
        ).normalized;

        Vector3 startPosition = transform.position;
        Vector3 pivot = startPosition + moveDirection * (cellSize / 2f);

        Vector3 axis = Vector3.Cross(
            moveDirection,
            Vector3.forward
        ).normalized;

        Quaternion startRotation = transform.rotation;
        float elapsed = 0f;

        while (elapsed < cellMoveDuration)
        {
            elapsed += Time.deltaTime;

            float angle = 90f * Mathf.Clamp01(
                elapsed / cellMoveDuration
            );

            transform.position = pivot +
                Quaternion.AngleAxis(angle, axis) *
                (startPosition - pivot);

            transform.rotation =
                Quaternion.AngleAxis(angle, axis) * startRotation;

            yield return null;
        }

        transform.position = destination.transform.position;

        transform.rotation =
            Quaternion.AngleAxis(90f, axis) * startRotation;

        column = destination.column;
        row = destination.row;
        CurrentCell = destination;

        RollFaceLabels(direction);

        IsMoving = false;

        if (onComplete != null)
        {
            onComplete();
        }
    }

    BoardCell FindCell(int c, int r, BoardCell[] cells)
    {
        if (cells == null) return null;

        foreach (BoardCell cell in cells)
        {
            if (cell != null &&
                cell.column == c &&
                cell.row == r)
            {
                return cell;
            }
        }

        return null;
    }

    void RollFaceLabels(Vector2Int direction)
    {
        RPSSymbol oldTop = topSymbol;
        RPSSymbol oldBottom = bottomSymbol;
        RPSSymbol oldFront = frontSymbol;
        RPSSymbol oldBack = backSymbol;
        RPSSymbol oldLeft = leftSymbol;
        RPSSymbol oldRight = rightSymbol;

        if (direction.x > 0)
        {
            topSymbol = oldLeft;
            bottomSymbol = oldRight;
            leftSymbol = oldBottom;
            rightSymbol = oldTop;
        }
        else if (direction.x < 0)
        {
            topSymbol = oldRight;
            bottomSymbol = oldLeft;
            leftSymbol = oldTop;
            rightSymbol = oldBottom;
        }
        else if (direction.y > 0)
        {
            topSymbol = oldFront;
            bottomSymbol = oldBack;
            frontSymbol = oldBottom;
            backSymbol = oldTop;
        }
        else if (direction.y < 0)
        {
            topSymbol = oldBack;
            bottomSymbol = oldFront;
            frontSymbol = oldTop;
            backSymbol = oldBottom;
        }
    }

    public RPSSymbol GetBackSymbol()
    {
        return backSymbol;
    }

    public void SetStartingCell(BoardCell cell)
    {
        CurrentCell = cell;

        if (cell != null)
        {
            column = cell.column;
            row = cell.row;
            transform.position = cell.transform.position;
        }
    }
}
