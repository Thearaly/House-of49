using UnityEngine;

public class BoardCell : MonoBehaviour
{
    [Header("Board coordinate (0..6)")]
    public int column;
    public int row;

    [Header("Cell symbol")]
    public RPSSymbol symbol;
}
